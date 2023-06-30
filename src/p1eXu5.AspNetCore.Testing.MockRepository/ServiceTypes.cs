namespace p1eXu5.AspNetCore.MockRepository;

/// <summary>
/// Service <see cref="System.Type"/> decorator. Recognize a
/// service that will be substituted or mocked in tests.
/// </summary>
public interface IServiceType
{
    Type Type { get; }
}

/// <summary>
/// Service <see cref="System.Type"/> decorator. Recognize a
/// service that will be substituted or mocked in tests.
/// </summary>
/// <param Name="Value"></param>
public record ServiceType(Type Type) : IServiceType;

/// <summary>
/// Service <see cref="Type"/> decorator. Recognize a
/// service that will be substituted or mocked in tests.
/// </summary>
/// <typeparam name="Type"></typeparam>
public record Service<TType>() : ServiceType(typeof(TType));
