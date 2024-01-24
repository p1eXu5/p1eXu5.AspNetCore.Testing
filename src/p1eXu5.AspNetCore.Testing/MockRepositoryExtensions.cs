using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using NSubstitute;
using p1eXu5.AspNetCore.Testing.MockRepository;

namespace p1eXu5.AspNetCore.Testing;

public static class MockRepositoryExtensions
{
    [return: NotNull]
    public static TMock TrySubstitute<TMock>(this MockRepository.MockRepository nMockRepository)
        where TMock : class
    {
        TMock? mock = nMockRepository.Substitute<TMock>();

        if (mock is null)
        {
            mock = Substitute.For<TMock>();
            nMockRepository.Substitute<TMock>(mock);
        }

        return mock;
    }

    public static void AddMockRepository(
        this IWebHostBuilder webHostBuilder,
        IReadOnlyCollection<IServiceType> substitutedServiceTypes,
        ITestLogWriter testLogWriter,
        Action<MockRepository.MockRepository> setMockRepository)
    {

        webHostBuilder.ConfigureServices(services =>
        {
            var mockRepository = MockRepository.MockRepository.Initialize(services, in substitutedServiceTypes, in testLogWriter);
            setMockRepository(mockRepository);
        });
    }
}
