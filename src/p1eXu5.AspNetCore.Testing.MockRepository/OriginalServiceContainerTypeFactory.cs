namespace p1eXu5.AspNetCore.Testing.MockRepository;

/// <summary>
/// Factory methods for <see cref="IOriginalServiceContainer{TService}"/>.
/// </summary>
internal static class OriginalServiceContainerTypeFactory
{
    /// <summary>
    /// Creates <see cref="IOriginalServiceContainer{TService}"/> type dynamically.
    /// </summary>
    /// <param Name="serviceType"></param>
    /// <returns></returns>
    internal static Type MakeGenericFrom(Type serviceType)
    {
        Type t = typeof(IOriginalServiceContainer<>);
        return t.MakeGenericType(serviceType);
    }
}