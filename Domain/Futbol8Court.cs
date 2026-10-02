namespace AppReservasCanchas.Domain;

public sealed class Futbol8Court : IReservationComponent
{
    public const decimal BasePrice = 120_000m;

    public string GetDescription() => "Fútbol 8: cancha mediana para partidos 8 vs 8.";

    public decimal GetPrice() => BasePrice;
}
