using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.FactoryMethod;

public sealed class Futbol11Creator : CourtCreator
{
    public override IReservationComponent CreateCourt() => new Futbol11Court();
}
