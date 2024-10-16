/*
 * Source code grabbed from https://github.com/PapGroup/NSubstitute.FluentAssertionsBridge
 */

using System.Runtime.ExceptionServices;
using FluentAssertions.Execution;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;

namespace p1eXu5.AspNetCore.Testing;

internal class FluentAssertionsMatcherAdapter<T> : IArgumentMatcher<T>, IDescribeNonMatches
{
    private readonly Action<T?> _assertion;

    private List<string> _errors = new();
    private Exception? _exception;

    public FluentAssertionsMatcherAdapter(Action<T?> assertion)
    {
        _assertion = assertion;
    }

    public bool IsSatisfiedBy(T? argument)
    {
        using var scope = new AssertionScope();

        try
        {
            _assertion(argument);
            _errors = scope.Discard().ToList();
        }
        catch (Exception ex)
        {
            _exception = ex;
            _errors.Add(ex.Message);
        }

        return _errors.Count == 0;
    }

    public string DescribeFor(object? argument)
    {
        if (_exception is { })
        {
            ExceptionDispatchInfo.Capture(_exception).Throw();
        }

        return string.Join(Environment.NewLine, _errors);
    }
}