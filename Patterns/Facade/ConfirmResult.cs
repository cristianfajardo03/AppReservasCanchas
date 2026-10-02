namespace AppReservasCanchas.Patterns.Facade;

public sealed class ConfirmResult
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public ConfirmedReservation? Reservation { get; init; }

    public static ConfirmResult Ok(ConfirmedReservation reservation) => new()
    {
        Success = true,
        Message = reservation.FormatReceipt(),
        Reservation = reservation
    };

    public static ConfirmResult Fail(string message) => new()
    {
        Success = false,
        Message = message
    };
}
