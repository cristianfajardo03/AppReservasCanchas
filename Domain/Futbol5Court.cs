namespace AppReservasCanchas.Domain;

public sealed class Futbol5Court : IReservationComponent
{
    public const decimal BasePrice = 80_000m;

    public string GetDescription() => "Fútbol 5: cancha pequeña para partidos 5 vs 5.";

    public decimal GetPrice() => BasePrice;
}
