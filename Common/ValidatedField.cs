namespace Radiotech.Common;

public sealed class ValidatedEntry : IInputControl
{
    public Entry Control { get; } = new();
    public Label ErrorLabel { get; } = new() { TextColor = Colors.Red, 
        //IsVisible = false
    };
    public object? GetValue() => string.IsNullOrEmpty(_lastNotifiedValue) ? Control.Text : _lastNotifiedValue;
    public bool IsValid { get; private set; } = false;  
    public void SetValue(object? value)
    {
        var str = value?.ToString() ?? "";
        if (str != _lastNotifiedValue)
        {
            Control.Text = str;
        }
    }
    public event Action<string?>? ValueChanged;

    private readonly DelegateValidator _validator;
    private string? _lastNotifiedValue;

    // public ValidatedField(Entry entry, Label errorLabel, DelegateValidator validator)
    // {   
    //     Control = entry;
    //     ErrorLabel = errorLabel;
    //     Validator = validator;
    //
    //     Control.TextChanged += (_, _) => Validate();
    // }
    public ValidatedEntry(
        string placeholder, 
        DelegateValidator validator, 
        string? initialValue = null)
    {
        _validator = validator;
        Control.Placeholder = placeholder;
        if (initialValue != null)
        {
            Control.Text = initialValue;
            IsValid = true;
        }
        Control.TextChanged += OnTextChanged;
    }
    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        (IsValid, var error) = _validator(e.NewTextValue ?? "");
        ErrorLabel.Text = error;
        ErrorLabel.IsVisible = !string.IsNullOrEmpty(error);

        if (e.NewTextValue != _lastNotifiedValue)
        {
            _lastNotifiedValue = e.NewTextValue ?? string.Empty;
            ValueChanged?.Invoke(_lastNotifiedValue);
        }
    }
    View IInputControl.Control => Control;
}

public sealed class ValidatedPicker : IInputControl
{
    public Picker Control { get; } = new();
    public Label ErrorLabel { get; } = new() { TextColor = Colors.Red, IsVisible = false };
    public bool IsValid { get; private set; } = false;
    public object? GetValue() => _items[Control.SelectedIndex];
    public event Action<string?>? ValueChanged;

    private readonly string[] _items;
    public DelegateValidator Validator { get; } = Validators.RequiredNotNull;

    public ValidatedPicker(
        string title,
        string[] items,
        DelegateValidator validator,
        string? initialValue = null)
    {
        _items = items;
        Control.Title = title;
        Control.ItemsSource = items;
        if (initialValue != null)
        {
            var idx = Array.IndexOf(items, initialValue);
            if (idx >= 0) Control.SelectedIndex = idx;
        }
        Control.SelectedIndexChanged += OnSelectionChanged;
    }
    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        (IsValid, var error) = Validator((string?)Control.SelectedItem);
        ErrorLabel.Text = error;
        ErrorLabel.IsVisible = !string.IsNullOrEmpty(error);
        ValueChanged?.Invoke(_items[Control.SelectedIndex]);
        // ? _items[Control.SelectedIndex]
        //     : "";
        // Validate();
    }
    public void SetValue(object? value)
    {
        var idx = Array.IndexOf(_items, value?.ToString());
        Control.SelectedIndex = idx;
    }
    View IInputControl.Control => Control;
}

public class ValidatedDateField : IInputControl
{
    public DatePicker Control { get; } = new() { Format = "dd.MM.yyyy" };
    public Label ErrorLabel { get; } = new() { TextColor = Colors.Red, IsVisible = false };
    public bool IsValid { get; private set; } = true;
    public event Action<string?>? ValueChanged;

    private readonly DelegateValidator _validator;
    private string? _lastNotifiedValue;
    private string _currentError;

    public ValidatedDateField(
        string title, 
        DelegateValidator validator, 
        DateTime? initialValue = null)
    {
        // Control.;
        _validator = validator;
        Control.Date = initialValue ?? DateTime.Today;
        Control.DateSelected += OnDateChanged;
    }

    private void OnDateChanged(object? sender, DateChangedEventArgs e)
    {
        (IsValid, var error) = _validator(e.NewDate?.ToString() ?? "");
        ErrorLabel.Text = error;
        ErrorLabel.IsVisible = !string.IsNullOrEmpty(error);

        if (e.NewDate.ToString() != _lastNotifiedValue)
        {
            _lastNotifiedValue = e.NewDate.ToString() ?? string.Empty;
            ValueChanged?.Invoke(_lastNotifiedValue);
        }
    }
    public object? GetValue() => new DateOnly(
        Control.Date.Value.Year, 
        Control.Date.Value.Month, 
        Control.Date.Value.Day);
    public void SetValue(object? value) => Control.Date = value is DateTime dt ? dt : DateTime.Today;
    View IInputControl.Control => Control;
}