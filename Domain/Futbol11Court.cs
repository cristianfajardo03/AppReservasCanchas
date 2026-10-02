namespace AppReservasCanchas.Domain;

public sealed class Futbol11Court : IReservationComponent
{
    public const decimal BasePrice = 180_000m;

    public string GetDescription() => "Fútbol 11: cancha reglamentaria para partidos 11 vs 11.";

    public decimal GetPrice() => BasePrice;
}
