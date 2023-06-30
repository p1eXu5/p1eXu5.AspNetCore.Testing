using Microsoft.Extensions.DependencyInjection;

namespace p1eXu5.AspNetCore.MockRepository;

public class MockServiceProxy
{
    private readonly Type _wrappedType;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockServiceProxy"/> class.
    /// </summary>
    /// <param name="serviceScraperType"></param>
    internal MockServiceProxy(Type serviceScraperType)
    {
        _wrappedType = serviceScraperType;
    }

    public object? Substitute { get; internal set; }

    public object Resolve(IServiceProvider serviceProvider)
    {
        if (Substitute is null)
        {
            if (_wrappedType is null)
            {
                throw new InvalidOperationException(
                    "Resolving service type is unknown. " +
                    "Check that you don't forget to call MockRepository.Substitute() method " +
                    "and initialize with correct mocking service list. .");
            }

            var service = serviceProvider.GetRequiredService(_wrappedType);
            return service;
        }

        return Substitute;
    }
}