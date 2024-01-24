namespace p1eXu5.AspNetCore.Testing.MockRepository;


/// <summary>
/// Scraper service for duplicating registering service descriptor. Used in <see cref="MockServiceProxy"/>
/// to obtain real service.
/// </summary>
/// <typeparam name="TService"></typeparam>
internal interface IOriginalServiceContainer<TService>
{
    /// <inheritdoc cref="MakeGenericServiceProxyType(Type)"/>
    internal static Type MakeGenericType()
    {
        return OriginalServiceContainerTypeFactory.MakeGenericFrom(typeof(TService));
    }
}
