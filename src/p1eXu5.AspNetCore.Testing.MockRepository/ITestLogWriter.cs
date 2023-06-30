namespace p1eXu5.AspNetCore.MockRepository;

public interface ITestLogWriter
{
    void WriteLine(string message);
    void WriteLine(string message, object arg);
    void WriteLine(string message, object arg, Exception arg2);
}