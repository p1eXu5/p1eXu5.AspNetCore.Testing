namespace p1eXu5.AspNetCore.Testing.Logging;

/// <summary>
/// Place where writers are set is important. For example,
/// if writers are set in SetUpFixture then logs in produced within
/// setup method but not within test method. <br/>
/// For test method logs writers need to be set in SetUp method or
/// in test method itself.
/// </summary>
public class TestContextWriters : ITestContextWriters
{
    public TextWriter? Progress { get; set; }

    public TextWriter? Out { get; set; }
}
