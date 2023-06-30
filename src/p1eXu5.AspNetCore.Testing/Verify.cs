/*
 * Source code grabbed from https://github.com/PapGroup/NSubstitute.FluentAssertionsBridge
 *
 */

using NSubstitute.Core.Arguments;

namespace p1eXu5.AspNetCore.Testing;

/// <summary>
/// The class that integrates NSubstitute with FluentAssertions
/// </summary>
public static class Verify
{
    /// <summary>
    /// The method that verifies the arguments of called methods using FluentAssertions.
    /// <para>
    /// In nullable context use nullable forgiving operator:
    /// <code>
    /// Verify.That&lt;Foo&gt;(req => ...)!
    /// </code>
    /// </para>
    /// </summary>
    /// <typeparam name="T">type of arguments to verify</typeparam>
    /// <param name="action">delegate that contains assertion</param>
    /// <returns>parameter of called method</returns>
    public static T? That<T>(Action<T?> action)
    {
        return ArgumentMatcher.Enqueue(new FluentAssertionsMatcherAdapter<T>(action));
    }
}