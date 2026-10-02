using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.FactoryMethod;

public sealed class Futbol5Creator : CourtCreator
{
    public override IReservationComponent CreateCourt() => new Futbol5Court();
}
