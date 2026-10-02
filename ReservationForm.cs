using AppReservasCanchas.Domain;
using AppReservasCanchas.Patterns.Facade;

namespace AppReservasCanchas;

public sealed class ReservationForm : Form
{
    private readonly ReservationFacade _facade = new();
    private readonly ComboBox _courtCombo = new();
    private readonly DateTimePicker _datePicker = new();
    private readonly ComboBox _timeCombo = new();
    private readonly CheckBox _lightingCheck = new();
    private readonly CheckBox _refereeCheck = new();
    private readonly CheckBox _equipmentCheck = new();
    private readonly Label _descriptionLabel = new();
    private readonly Label _totalLabel = new();
    private readonly Label _statusLabel = new();
    private readonly ListBox _reservationsList = new();
    private readonly Button _payButton = new();
    private readonly Button _confirmButton = new();

    public ReservationForm()
    {
        Text = "Reservas de canchas";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        ClientSize = new Size(420, 720);
        Font = new Font("Segoe UI", 10F);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            ColumnCount = 1,
            AutoSize = true
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        layout.Controls.Add(Title("Reserva tu cancha"));
        layout.Controls.Add(Hint("Prototipo de sesión (sin base de datos)."));

        layout.Controls.Add(LabelFor("Tipo de cancha"));
        _courtCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        _courtCombo.Dock = DockStyle.Top;
        foreach (var (type, name) in _facade.GetAvailableCourtTypes())
        {
            _courtCombo.Items.Add(new CourtOption(type, name));
        }

        _courtCombo.DisplayMember = nameof(CourtOption.Name);
        _courtCombo.SelectedIndex = 0;
        layout.Controls.Add(_courtCombo);

        layout.Controls.Add(LabelFor("Fecha"));
        _datePicker.Format = DateTimePickerFormat.Short;
        _datePicker.Dock = DockStyle.Top;
        layout.Controls.Add(_datePicker);

        layout.Controls.Add(LabelFor("Hora de inicio (1 hora)"));
        _timeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        _timeCombo.Dock = DockStyle.Top;
        foreach (TimeOnly slot in _facade.GetTimeSlots())
        {
            _timeCombo.Items.Add(slot);
        }

        _timeCombo.FormatString = "HH:mm";
        _timeCombo.FormattingEnabled = true;
        _timeCombo.SelectedIndex = 0;
        layout.Controls.Add(_timeCombo);

        layout.Controls.Add(LabelFor("Servicios opcionales"));
        _lightingCheck.Text = "Iluminación (+15.000)";
        _refereeCheck.Text = "Árbitro (+40.000)";
        _equipmentCheck.Text = "Equipamiento deportivo (+25.000)";
        layout.Controls.Add(_lightingCheck);
        layout.Controls.Add(_refereeCheck);
        layout.Controls.Add(_equipmentCheck);

        _descriptionLabel.AutoSize = true;
        _descriptionLabel.MaximumSize = new Size(380, 0);
        layout.Controls.Add(_descriptionLabel);

        _totalLabel.Font = new Font(Font, FontStyle.Bold);
        _totalLabel.AutoSize = true;
        layout.Controls.Add(_totalLabel);

        _payButton.Text = "Simular pago";
        _payButton.Dock = DockStyle.Top;
        _payButton.Height = 36;
        layout.Controls.Add(_payButton);

        _confirmButton.Text = "Confirmar reserva";
        _confirmButton.Dock = DockStyle.Top;
        _confirmButton.Height = 36;
        layout.Controls.Add(_confirmButton);

        _statusLabel.AutoSize = true;
        _statusLabel.MaximumSize = new Size(380, 0);
        layout.Controls.Add(_statusLabel);

        layout.Controls.Add(LabelFor("Reservas de esta sesión"));
        _reservationsList.Dock = DockStyle.Fill;
        _reservationsList.Height = 140;
        layout.Controls.Add(_reservationsList);

        Controls.Add(layout);

        _courtCombo.SelectedIndexChanged += (_, _) => RefreshPreview();
        _datePicker.ValueChanged += (_, _) => RefreshPreview();
        _timeCombo.SelectedIndexChanged += (_, _) => RefreshPreview();
        _lightingCheck.CheckedChanged += (_, _) => RefreshPreview();
        _refereeCheck.CheckedChanged += (_, _) => RefreshPreview();
        _equipmentCheck.CheckedChanged += (_, _) => RefreshPreview();
        _payButton.Click += (_, _) => SimulatePayment();
        _confirmButton.Click += (_, _) => ConfirmReservation();

        RefreshPreview();
    }

    private void RefreshPreview()
    {
        PreviewResult preview = _facade.Preview(CurrentDraft());
        if (preview.Success)
        {
            _descriptionLabel.Text = preview.Description;
            _totalLabel.Text = $"Total: {preview.Total:N0}";
            _statusLabel.Text = string.Empty;
        }
        else
        {
            _descriptionLabel.Text = preview.Message;
            _totalLabel.Text = "Total: —";
        }
    }

    private void SimulatePayment()
    {
        DialogResult answer = MessageBox.Show(
            this,
            "¿Autorizar el cobro simulado del total actual?",
            "Pago simulado",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        OperationResult result = _facade.AuthorizePayment(CurrentDraft(), answer == DialogResult.Yes);
        _statusLabel.Text = result.Message;
        if (!result.Success)
        {
            MessageBox.Show(this, result.Message, "Pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void ConfirmReservation()
    {
        ConfirmResult result = _facade.Confirm(CurrentDraft());
        _statusLabel.Text = result.Message;
        if (result.Success && result.Reservation is not null)
        {
            MessageBox.Show(this, result.Reservation.FormatReceipt(), "Reserva confirmada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshReservationList();
        }
        else
        {
            MessageBox.Show(this, result.Message, "No se pudo confirmar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RefreshReservationList()
    {
        _reservationsList.Items.Clear();
        foreach (ConfirmedReservation reservation in _facade.ListConfirmed())
        {
            _reservationsList.Items.Add(reservation.FormatListItem());
        }
    }

    private ReservationDraft CurrentDraft()
    {
        var draft = new ReservationDraft
        {
            Date = DateOnly.FromDateTime(_datePicker.Value.Date),
            Lighting = _lightingCheck.Checked,
            Referee = _refereeCheck.Checked,
            Equipment = _equipmentCheck.Checked
        };

        if (_courtCombo.SelectedItem is CourtOption option)
        {
            draft.CourtType = option.Type;
        }

        if (_timeCombo.SelectedItem is TimeOnly time)
        {
            draft.StartTime = time;
        }

        return draft;
    }

    private static Label Title(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Font = new Font("Segoe UI", 16F, FontStyle.Bold)
    };

    private static Label Hint(string text) => new()
    {
        Text = text,
        AutoSize = true,
        ForeColor = Color.DimGray
    };

    private static Label LabelFor(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Padding = new Padding(0, 8, 0, 0)
    };

    private sealed class CourtOption(CourtType type, string name)
    {
        public CourtType Type { get; } = type;

        public string Name { get; } = name;

        public override string ToString() => Name;
    }
}
