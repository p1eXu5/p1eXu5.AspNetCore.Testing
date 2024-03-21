using System.Diagnostics.CodeAnalysis;
using NSubstitute;

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

    [return: NotNull]
    public static TMock ForceSubstitute<TMock>(this MockRepository.MockRepository nMockRepository)
        where TMock : class
    {
        var mock = Substitute.For<TMock>();
        nMockRepository.Substitute<TMock>(mock);
        return mock;
    }
}
