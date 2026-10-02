namespace AppReservasCanchas.Patterns.Facade;

public sealed class PreviewResult
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public decimal Total { get; init; }

    public static PreviewResult Ok(string description, decimal total) => new()
    {
        Success = true,
        Description = description,
        Total = total
    };

    public static PreviewResult Fail(string message) => new()
    {
        Success = false,
        Message = message
    };
}
