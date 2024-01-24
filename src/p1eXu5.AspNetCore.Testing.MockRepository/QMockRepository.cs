using Microsoft.Extensions.DependencyInjection;

namespace p1eXu5.AspNetCore.Testing.MockRepository;

/// <summary>
/// Extension for <see cref="MockRepository"/> to work with Moq.
/// </summary>
public sealed class QMockRepository : MockRepository
{
    public QMockRepository(ITestLogWriter testLogWriter) : base(testLogWriter)
    {
    }

    public new object? Substitute<TService>()
    {
        if (MockServiceProxyMap.TryGetValue(typeof(TService), out var decorator))
        {
            return decorator.Substitute;
        }

        throw new InvalidOperationException($"{typeof(TService).Name} service was not replaced within decorator");
    }

    protected override Func<MockServiceProxy, IServiceProvider, object> Resolve { get; }
        = (decorator, sp) => GetObject(decorator.Resolve(sp));

    private static object GetObject(object instance)
    {
        var objectPropertyInfo = instance.GetType()
            .GetProperties(
                System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.GetField)
            .FirstOrDefault(pi =>
                pi.Name.Equals("Object", StringComparison.Ordinal) && pi.PropertyType != typeof(object));

        if (objectPropertyInfo is { })
        {
            return objectPropertyInfo.GetValue(instance)!;
        }

        return instance;
    }

    public static new QMockRepository Initialize(in IServiceCollection services,
                                                 in IReadOnlyCollection<IServiceType> substitutedServiceTypes,
                                                 in ITestLogWriter testLogWriter)
    {
        QMockRepository mockRepository = new(testLogWriter);
        BuildMockServiceProxies(services, substitutedServiceTypes, testLogWriter, mockRepository);
        return mockRepository;
    }
}
