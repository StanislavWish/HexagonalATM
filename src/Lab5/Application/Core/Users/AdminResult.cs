namespace Application.Core.Users;

public abstract record AdminResult
{
    public record Success : AdminResult;

    public record Failure(string Message) : AdminResult;
}