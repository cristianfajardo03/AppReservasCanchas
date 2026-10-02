using AppReservasCanchas.Domain;
using AppReservasCanchas.Patterns.Adapter;
using AppReservasCanchas.Patterns.Decorator;
using AppReservasCanchas.Patterns.FactoryMethod;

namespace AppReservasCanchas.Patterns.Facade;

public sealed class ReservationFacade
{
    public const string IncompleteDataMessage = "Datos incompletos.";
    public const string InvalidDateMessage = "Fecha inválida.";
    public const string InvalidTimeSlotMessage = "El sistema no acepta esa franja.";
    public const string MissingPaymentMessage = "Falta el pago.";
    public const string ConflictMessage = "Horario no disponible.";
    public const string PaymentCanceledMessage = "Pago cancelado.";

    private readonly IPaymentProcessor _paymentProcessor;
    private readonly InMemoryReservationStore _store;
    private readonly IReadOnlyDictionary<CourtType, CourtCreator> _creators;
    private string? _authorizedFingerprint;

    public ReservationFacade(
        IPaymentProcessor? paymentProcessor = null,
        InMemoryReservationStore? store = null)
    {
        _paymentProcessor = paymentProcessor ?? new SimulatedPaymentAdapter(new SimulatedPaymentGateway());
        _store = store ?? new InMemoryReservationStore();
        _creators = new Dictionary<CourtType, CourtCreator>
        {
            [CourtType.Futbol5] = new Futbol5Creator(),
            [CourtType.Futbol8] = new Futbol8Creator(),
            [CourtType.Futbol11] = new Futbol11Creator()
        };
    }

    public IReadOnlyList<(CourtType Type, string Name)> GetAvailableCourtTypes() =>
    [
        (CourtType.Futbol5, "Fútbol 5"),
        (CourtType.Futbol8, "Fútbol 8"),
        (CourtType.Futbol11, "Fútbol 11")
    ];

    public IReadOnlyList<TimeOnly> GetTimeSlots()
    {
        var slots = new List<TimeOnly>();
        for (int hour = 8; hour <= 21; hour++)
        {
            slots.Add(new TimeOnly(hour, 0));
        }

        return slots;
    }

    public PreviewResult Preview(ReservationDraft draft)
    {
        InvalidateTokenIfDraftChanged(draft);

        if (draft.CourtType is null)
        {
            return PreviewResult.Fail(IncompleteDataMessage);
        }

        if (draft.Date == default || draft.StartTime == default)
        {
            return PreviewResult.Fail(IncompleteDataMessage);
        }

        if (draft.Date < DateOnly.FromDateTime(DateTime.Today))
        {
            return PreviewResult.Fail(InvalidDateMessage);
        }

        if (draft.StartTime.Hour < 8 || draft.StartTime.Hour > 21 || draft.StartTime.Minute != 0)
        {
            return PreviewResult.Fail(InvalidTimeSlotMessage);
        }

        IReservationComponent component = Compose(draft);
        return PreviewResult.Ok(component.GetDescription(), component.GetPrice());
    }

    public OperationResult AuthorizePayment(ReservationDraft draft, bool customerApproved)
    {
        InvalidateTokenIfDraftChanged(draft);

        PreviewResult preview = Preview(draft);
        decimal amount = preview.Success ? preview.Total : 0m;
        PaymentResult charge = _paymentProcessor.Charge(amount, Guid.NewGuid().ToString("N"), customerApproved);

        if (!customerApproved)
        {
            _authorizedFingerprint = null;
            return OperationResult.Fail(PaymentCanceledMessage);
        }

        if (!preview.Success)
        {
            return OperationResult.Fail(preview.Message);
        }

        if (preview.Total <= 0)
        {
            return OperationResult.Fail(MissingPaymentMessage);
        }
        if (!charge.Success)
        {
            _authorizedFingerprint = null;
            return OperationResult.Fail(MissingPaymentMessage);
        }

        _authorizedFingerprint = Fingerprint(draft, preview.Total);
        return OperationResult.Ok($"Cobro simulado exitoso por {preview.Total:N0}.");
    }

    public ConfirmResult Confirm(ReservationDraft draft)
    {
        if (draft.CourtType is null || draft.Date == default || draft.StartTime == default)
        {
            return ConfirmResult.Fail(IncompleteDataMessage);
        }

        PreviewResult preview = Preview(draft);
        if (!preview.Success)
        {
            return ConfirmResult.Fail(preview.Message);
        }

        string fingerprint = Fingerprint(draft, preview.Total);
        if (_authorizedFingerprint is null || _authorizedFingerprint != fingerprint)
        {
            return ConfirmResult.Fail(MissingPaymentMessage);
        }

        if (_store.HasConflict(draft.CourtType.Value, draft.Date, draft.StartTime))
        {
            return ConfirmResult.Fail(ConflictMessage);
        }

        var reservation = new ConfirmedReservation
        {
            Id = Guid.NewGuid(),
            CourtType = draft.CourtType.Value,
            Date = draft.Date,
            StartTime = draft.StartTime,
            Lighting = draft.Lighting,
            Referee = draft.Referee,
            Equipment = draft.Equipment,
            Total = preview.Total,
            Description = preview.Description
        };

        _store.Add(reservation);
        _authorizedFingerprint = null;
        return ConfirmResult.Ok(reservation);
    }

    public IReadOnlyList<ConfirmedReservation> ListConfirmed() => _store.List();

    private IReservationComponent Compose(ReservationDraft draft)
    {
        CourtCreator creator = _creators[draft.CourtType!.Value];
        IReservationComponent component = creator.CreateCourt();

        if (draft.Lighting)
        {
            component = new LightingDecorator(component);
        }

        if (draft.Referee)
        {
            component = new RefereeDecorator(component);
        }

        if (draft.Equipment)
        {
            component = new EquipmentDecorator(component);
        }

        return component;
    }

    private void InvalidateTokenIfDraftChanged(ReservationDraft draft)
    {
        if (_authorizedFingerprint is null || draft.CourtType is null)
        {
            return;
        }

        IReservationComponent component = Compose(draft);
        string current = Fingerprint(draft, component.GetPrice());
        if (current != _authorizedFingerprint)
        {
            _authorizedFingerprint = null;
        }
    }

    private static string Fingerprint(ReservationDraft draft, decimal total) =>
        $"{draft.CourtType}|{draft.Date:yyyy-MM-dd}|{draft.StartTime:HH\\:mm}|{draft.Lighting}|{draft.Referee}|{draft.Equipment}|{total}";
}
