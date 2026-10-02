using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.Facade;

public sealed class InMemoryReservationStore
{
    private readonly List<ConfirmedReservation> _reservations = [];

    public IReadOnlyList<ConfirmedReservation> List() => _reservations.ToList();

    public void Add(ConfirmedReservation reservation) => _reservations.Add(reservation);

    public bool HasConflict(CourtType courtType, DateOnly date, TimeOnly startTime) =>
        _reservations.Any(r => r.CourtType == courtType && r.Date == date && r.StartTime == startTime);
}
