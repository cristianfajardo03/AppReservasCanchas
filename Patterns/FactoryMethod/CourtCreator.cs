using AppReservasCanchas.Domain;

namespace AppReservasCanchas.Patterns.FactoryMethod;

public abstract class CourtCreator
{
    public abstract IReservationComponent CreateCourt();
}
