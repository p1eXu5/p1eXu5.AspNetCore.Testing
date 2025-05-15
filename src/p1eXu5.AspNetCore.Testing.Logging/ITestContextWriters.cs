namespace p1eXu5.AspNetCore.Testing.Logging;

/// <summary>
/// Place where writers are set is important. For example,
/// if writers are set in SetUpFixture then logs in produced within
/// setup method but not within test method.
/// <para>
/// For test method logs writers need to be set in SetUp method or
/// in test method itself.
/// </para>
/// </summary>
public interface ITestContextWriters : IDisposable
{
    /// <summary>
    /// Output console. <br/>
    /// If NUnit is used writers are may be different in setup and test methods.<br/>
    /// The value may need to be reassigned.
    /// </summary>
    ITestTextWriter? Progress { get; }

    /// <summary>
    /// If NUnit is used writers are may be different in setup and test methods.<br/>
    /// The value may need to be reassigned.
    /// </summary>
    ITestTextWriter? Out { get; }

    ITestContextWriters Freeze();
}
