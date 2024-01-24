using Microsoft.Extensions.DependencyInjection;

namespace p1eXu5.AspNetCore.Testing.MockRepository;

public class MockServiceProxy
{
    private readonly Type _originalServiceContainerType;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockServiceProxy"/> class.
    /// </summary>
    /// <param name="originalServiceContainerType"></param>
    internal MockServiceProxy(Type originalServiceContainerType)
    {
        _originalServiceContainerType = originalServiceContainerType;
    }

    public object? Substitute { get; internal set; }

    public object Resolve(IServiceProvider serviceProvider)
    {
        if (Substitute is null)
        {
            if (_originalServiceContainerType is null)
            {
                throw new InvalidOperationException(
                    "Resolving service type is unknown. " +
                    "Check that you don't forget to call MockRepository.Substitute() method " +
                    "and initialize with correct mocking service list. .");
            }

            var service = serviceProvider.GetRequiredService(_originalServiceContainerType);
            return service;
        }

        return Substitute;
    }
}