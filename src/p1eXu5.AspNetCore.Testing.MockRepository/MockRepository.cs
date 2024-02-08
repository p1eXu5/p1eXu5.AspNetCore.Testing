using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace p1eXu5.AspNetCore.Testing.MockRepository;

public class MockRepository
{
    private ImmutableDictionary<Type, MockServiceProxy> _mockServiceProxyMap = default!;
    private readonly ITestLogWriter _testLogWriter;


    protected MockRepository(ITestLogWriter testLogWriter)
    {
        _testLogWriter = testLogWriter;
    }


    protected IReadOnlyDictionary<Type, MockServiceProxy> MockServiceProxyMap => _mockServiceProxyMap;

    /// <summary>
    /// Invokes <see cref="MockServiceProxy.Resolve(IServiceProvider)"/>
    /// </summary>
    protected virtual Func<MockServiceProxy, IServiceProvider, object> Resolve { get; }
        = (mockServiceProxy, serviceProvider) => mockServiceProxy.Resolve(serviceProvider);

    /// <summary>
    /// Initializes a new instance of the <see cref="MockRepository"/> class and built <see cref="MockServiceProxy"/> for the <paramref name="substitutedServiceTypes"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="substitutedServiceTypes"></param>
    /// <param name="testLogWriter"></param>
    /// <returns></returns>
    public static MockRepository Initialize(in IServiceCollection services, in IReadOnlyCollection<IServiceType> substitutedServiceTypes, in ITestLogWriter testLogWriter)
    {
        MockRepository mockRepository = new(testLogWriter);
        BuildMockServiceProxies(services, substitutedServiceTypes, testLogWriter, mockRepository);
        return mockRepository;
    }


    #region - unsafe accessors -

    /// <summary>
    /// Set substitution for <typeparamref name="TService"/> type <see cref="MockServiceProxy"/>.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="substitute"></param>
    public void Substitute<TService>(object substitute)
    {
        if (_mockServiceProxyMap.TryGetValue(typeof(TService), out var decorator))
        {
            decorator.Substitute = substitute;
            return;
        }

        _testLogWriter.LogDebug("Could not substitute {0} service!", typeof(TService));
    }

    /// <summary>
    /// Returns <typeparamref name="TService"/> substitute.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidCastException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public TService? Substitute<TService>()
    {
        if (_mockServiceProxyMap.TryGetValue(typeof(TService), out var decorator))
        {
            if (decorator.Substitute is null)
            {
                _testLogWriter.LogWarning("Substitute of type {0} is null!");
            }

            return (TService?)decorator.Substitute;
        }

        throw new InvalidOperationException($"{typeof(TService).Name} service was not replaced within mock service proxy.");
    }

    #endregion -------------------


    protected static void BuildMockServiceProxies(IServiceCollection services, IReadOnlyCollection<IServiceType> substitutedServiceTypes, ITestLogWriter testLogWriter, MockRepository mockRepository)
    {
        Dictionary<Type, MockServiceProxy> scrapperProxyMap = new();

        foreach (ServiceDescriptor sd in services.ToArray())
        {
            IServiceType? mockingService = null;
            try
            {
                mockingService = substitutedServiceTypes.SingleOrDefault(st => st.Type == sd.ServiceType);
            }
            catch (InvalidOperationException ex)
            {
                testLogWriter.LogError(ex, "Exception on selecting {0} service: {1}\n.Check mocking service list.", sd.ServiceType);
                throw;
            }

            if (mockingService is null)
            {
                continue;
            }

            Type originalServiceContainerType = AddOriginalServiceContainer(services, sd);
            MockServiceProxy mockServiceProxy = new(originalServiceContainerType);

            mockRepository.ReplaceServiceWithProxy(services, sd, mockServiceProxy);
            scrapperProxyMap.Add(sd.ServiceType, mockServiceProxy);
        }

        mockRepository._mockServiceProxyMap = scrapperProxyMap.ToImmutableDictionary();
    }

    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="sd"></param>
    /// <returns></returns>
    private static Type AddOriginalServiceContainer(IServiceCollection services, ServiceDescriptor sd)
    {
        Type originalServiceContainerType = OriginalServiceContainerTypeFactory.MakeGenericFrom(sd.ServiceType);

        ServiceDescriptor wrapperServiceDescriptor;

        if (sd.ImplementationFactory is not null)
        {
            wrapperServiceDescriptor =
                new ServiceDescriptor(
                    originalServiceContainerType,
                    sd.ImplementationFactory,
                    sd.Lifetime);
        }
        else if (sd.ImplementationType is not null)
        {
            wrapperServiceDescriptor =
                new ServiceDescriptor(
                    originalServiceContainerType,
                    sp => ConstructType(sd, sp),
                    sd.Lifetime);
        }
        else
        {
            wrapperServiceDescriptor =
                new ServiceDescriptor(
                    originalServiceContainerType,
                    sd.ImplementationInstance!);
        }

        services.Add(wrapperServiceDescriptor);
        return originalServiceContainerType;
    }

    private static object ConstructType(ServiceDescriptor sd, IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var ctorParameters =
            sd.ImplementationType
                !.GetConstructors()
                .First()
                .GetParameters()
                .Select(pi =>
                {
                    return scope.ServiceProvider.GetRequiredService(pi.ParameterType);
                })
                .ToArray();

        var instance = Activator.CreateInstance(sd.ImplementationType, ctorParameters)!;
        return instance;
    }

    /// <summary>
    /// Replace real service with implementation factory used <see cref="Resolve"/>. <br/>
    /// Add <see cref="Type"/> of scrapper as key and <see cref="MockServiceProxy"/> as
    /// value to the <paramref name="scrapperProxyMap"/>.
    /// </summary>
    /// <param name="scrapperProxyMap"></param>
    /// <param name="services"></param>
    /// <param name="sd"></param>
    /// <param name="testLogWriter"></param>
    private void ReplaceServiceWithProxy(
        in IServiceCollection services,
        in ServiceDescriptor sd,
        MockServiceProxy mockServiceProxy)
    {
        try
        {
            switch (sd.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.Replace(ServiceDescriptor.Singleton(
                        sd.ServiceType,
                        sp => Resolve(mockServiceProxy, sp)));
                    break;

                case ServiceLifetime.Scoped:
                    services.Replace(ServiceDescriptor.Scoped(
                        sd.ServiceType,
                        sp => Resolve(mockServiceProxy, sp)));
                    break;

                case ServiceLifetime.Transient:
                    services.Replace(ServiceDescriptor.Transient(
                        sd.ServiceType,
                        sp => Resolve(mockServiceProxy, sp)));
                    break;
            }
            _testLogWriter.LogDebug("MockServiceProxy for {0} service has been added.", sd.ServiceType);
        }
        catch (Exception ex)
        {
            _testLogWriter.LogError(ex, "Exception on adding decorator for {0} service: {1}!", sd.ServiceType);
            throw;
        }
    }
}