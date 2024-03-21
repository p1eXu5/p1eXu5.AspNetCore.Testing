using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using p1eXu5.AspNetCore.Testing.MockRepository;

namespace p1eXu5.AspNetCore.Testing;

public static class DependencyInjectionExtensions
{
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

    public static void AddMockRepository(
        this IHostApplicationBuilder hostAppBuilder,
        IReadOnlyCollection<IServiceType> substitutedServiceTypes,
        ITestLogWriter testLogWriter,
        Action<MockRepository.MockRepository> setMockRepository)
    {
        var mockRepository = MockRepository.MockRepository.Initialize(hostAppBuilder.Services, in substitutedServiceTypes, in testLogWriter);
        setMockRepository(mockRepository);
    }
}
