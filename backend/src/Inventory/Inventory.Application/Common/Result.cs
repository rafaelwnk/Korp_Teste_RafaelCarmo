namespace Inventory.Application.Common;

public class Result<T>(T? data, string message = "")
{
    public T? Data { get; private set; } = data;
    public string Message { get; private set; } = message;

    public static Result<T> Success(T data) => new(data);

    public static Result<T> Error(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Error message cannot be empty.", nameof(message));

        return new(default, message);
    }
}