using AppReservasCanchas.Domain;
using AppReservasCanchas.Patterns.Decorator;
using AppReservasCanchas.Patterns.Facade;
using AppReservasCanchas.Patterns.FactoryMethod;

namespace AppReservasCanchas;

internal static class ReservationFlowVerifier
{
    public static int Run()
    {
        var failures = new List<string>();

        IReservationComponent decorated = new EquipmentDecorator(
            new RefereeDecorator(
                new LightingDecorator(new Futbol5Creator().CreateCourt())));
        if (decorated.GetPrice() != 160_000m)
        {
            failures.Add($"Decorator total expected 160000, got {decorated.GetPrice()}");
        }

        var facade = new ReservationFacade();
        DateOnly date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        TimeOnly time = new(10, 0);

        var futbol5 = new ReservationDraft
        {
            CourtType = CourtType.Futbol5,
            Date = date,
            StartTime = time
        };

        PreviewResult preview = facade.Preview(futbol5);
        Check(failures, preview.Success && preview.Total == 80_000m, "Fútbol 5 sin extras debe costar 80000");

        Check(failures, facade.AuthorizePayment(futbol5, true).Success, "Pago Fútbol 5 debe ser exitoso");
        ConfirmResult first = facade.Confirm(futbol5);
        Check(failures, first.Success && first.Reservation is not null, "Confirmación Fútbol 5");
        Check(
            failures,
            first.Reservation is { } receipt
            && receipt.Id != Guid.Empty
            && receipt.CourtType == CourtType.Futbol5
            && receipt.Date == date
            && receipt.StartTime == time
            && receipt.Total == 80_000m
            && receipt.FormatReceipt().Contains(receipt.Id.ToString("N"), StringComparison.Ordinal),
            "Comprobante con identificador, tipo, fecha, hora, extras y total");

        Check(failures, facade.AuthorizePayment(futbol5, true).Success, "Pago del conflicto");
        ConfirmResult conflict = facade.Confirm(futbol5);
        Check(failures, !conflict.Success && conflict.Message == ReservationFacade.ConflictMessage, "Conflicto mismo tipo/fecha/hora");

        var futbol8 = new ReservationDraft
        {
            CourtType = CourtType.Futbol8,
            Date = date,
            StartTime = time
        };
        Check(failures, facade.AuthorizePayment(futbol8, true).Success, "Pago Fútbol 8");
        Check(failures, facade.Confirm(futbol8).Success, "Fútbol 8 en el mismo horario debe confirmarse");

        var extra = new ReservationDraft
        {
            CourtType = CourtType.Futbol11,
            Date = date,
            StartTime = new TimeOnly(11, 0)
        };
        Check(failures, !facade.AuthorizePayment(extra, false).Success, "Cancelar cobro");
        ConfirmResult canceled = facade.Confirm(extra);
        Check(failures, !canceled.Success && canceled.Message == ReservationFacade.MissingPaymentMessage, "Cancelar cobro no crea reserva");

        ConfirmResult withoutPay = facade.Confirm(extra);
        Check(failures, !withoutPay.Success && withoutPay.Message == ReservationFacade.MissingPaymentMessage, "Confirmar sin pago");

        var incomplete = new ReservationDraft();
        Check(
            failures,
            facade.Confirm(incomplete).Message == ReservationFacade.IncompleteDataMessage,
            "Datos incompletos");

        var past = new ReservationDraft
        {
            CourtType = CourtType.Futbol5,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            StartTime = time
        };
        Check(failures, facade.Preview(past).Message == ReservationFacade.InvalidDateMessage, "Fecha pasada");

        Console.WriteLine(failures.Count == 0 ? "VERIFY_OK" : "VERIFY_FAIL");
        foreach (string failure in failures)
        {
            Console.WriteLine(failure);
        }

        return failures.Count == 0 ? 0 : 1;
    }

    private static void Check(List<string> failures, bool condition, string name)
    {
        if (!condition)
        {
            failures.Add(name);
        }
    }
}
