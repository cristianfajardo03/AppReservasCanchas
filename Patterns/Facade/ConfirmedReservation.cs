using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.Facade;

public sealed class ConfirmedReservation
{
    public Guid Id { get; init; }

    public CourtType CourtType { get; init; }

    public DateOnly Date { get; init; }

    public TimeOnly StartTime { get; init; }

    public bool Lighting { get; init; }

    public bool Referee { get; init; }

    public bool Equipment { get; init; }

    public decimal Total { get; init; }

    public string Description { get; init; } = string.Empty;

    public string FormatReceipt()
    {
        string extras = FormatExtras();
        return $"Comprobante {Id:N} | {DisplayName(CourtType)} | {Date:yyyy-MM-dd} {StartTime:HH\\:mm} | Extras: {extras} | Total: {Total:N0}";
    }

    public string FormatListItem() => FormatReceipt();

    private string FormatExtras()
    {
        var parts = new List<string>();
        if (Lighting)
        {
            parts.Add("Iluminación");
        }

        if (Referee)
        {
            parts.Add("Árbitro");
        }

        if (Equipment)
        {
            parts.Add("Equipamiento");
        }

        return parts.Count == 0 ? "Ninguno" : string.Join(", ", parts);
    }

    private static string DisplayName(CourtType type) => type switch
    {
        CourtType.Futbol5 => "Fútbol 5",
        CourtType.Futbol8 => "Fútbol 8",
        CourtType.Futbol11 => "Fútbol 11",
        _ => type.ToString()
    };
}
