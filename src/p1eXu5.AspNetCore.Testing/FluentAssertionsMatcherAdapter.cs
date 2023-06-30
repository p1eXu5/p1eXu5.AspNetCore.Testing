/*
 * Source code grabbed from https://github.com/PapGroup/NSubstitute.FluentAssertionsBridge
 */

using FluentAssertions.Execution;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;

namespace p1eXu5.AspNetCore.Testing;

internal class FluentAssertionsMatcherAdapter<T> : IArgumentMatcher<T>, IDescribeNonMatches
{
    private readonly Action<T?> _assertion;

    private List<string> _errors = new();

    public FluentAssertionsMatcherAdapter(Action<T?> assertion)
    {
        _assertion = assertion;
    }

    public bool IsSatisfiedBy(T? argument)
    {
        using var scope = new AssertionScope();

        _assertion(argument);
        _errors = scope.Discard().ToList();

        return _errors.Count == 0;
    }

    public string DescribeFor(object? argument)
    {
        return string.Join(Environment.NewLine, _errors);
    }
}