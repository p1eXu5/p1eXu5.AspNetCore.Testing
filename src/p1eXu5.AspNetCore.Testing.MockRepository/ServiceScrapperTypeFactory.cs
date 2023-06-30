namespace p1eXu5.AspNetCore.MockRepository;

/// <summary>
/// Factory methods for <see cref="IServiceScraper{TService}"/>.
/// </summary>
internal static class ServiceScrapperTypeFactory
{
    /// <summary>
    /// Creates <see cref="IServiceScraper{TService}"/> type dynamically.
    /// </summary>
    /// <param Name="serviceType"></param>
    /// <returns></returns>
    internal static Type MakeGenericFrom(Type serviceType)
    {
        Type t = typeof(IServiceScraper<>);
        return t.MakeGenericType(serviceType);
    }
}