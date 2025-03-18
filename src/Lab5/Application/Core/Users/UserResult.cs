namespace Application.Core.Users;

public abstract record UserResult
{
    public record Success(decimal Money) : UserResult;

    public record Failure(string Message) : UserResult;
}