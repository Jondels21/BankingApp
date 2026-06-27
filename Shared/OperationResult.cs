
namespace BankingApp;

public class OperationResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;

    public static OperationResult Ok(string message = "")
        => new() {Success = true, Message = message };

    public static OperationResult Fail(string message)
        => new() {Success = false, Message = message };

}

public class OperationResult<T>
{
    public bool Success {get; init; }
    public string Message {get; init; } = string.Empty;
    public T? Data { get; init; }


    public static OperationResult<T> Ok(T data, string message = "")
        => new() {Success = true, Message = message, Data = data};

    public static OperationResult<T> Fail(string message)
        => new() {Success = false, Message = message, Data = default};
        
}