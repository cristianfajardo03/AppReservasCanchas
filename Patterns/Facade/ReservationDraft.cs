using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.Facade;

public sealed class ReservationDraft
{
    public CourtType? CourtType { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public bool Lighting { get; set; }

    public bool Referee { get; set; }

    public bool Equipment { get; set; }
}
