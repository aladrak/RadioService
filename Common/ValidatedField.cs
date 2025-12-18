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
    private string _currentError;
    // private readonly Func<string, (bool, string)> _validator;

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
    public bool IsValid { get; private set; }
    public object? GetValue() => Control.SelectedItem?.ToString();
    public event Action<string?>? ValueChanged;
    public void Validate() => _validator(Control.SelectedItem?.ToString());
    public void SetValue(object? value)
    {
        var idx = Array.IndexOf(_items, value?.ToString());
        Control.SelectedIndex = idx;
    }

    private readonly string[] _items;
    private readonly Func<string?, (bool, string)> _validator;
    // public int SelectedIndex { get => Control.SelectedIndex; }
    // public DelegateValidator Validator { get; }

    public ValidatedPicker(
        string title, 
        string[] items, 
        Func<string?, (bool, string)> validator, 
        string? initialValue = null)
    {
        _items = items;
        _validator = validator;
        Control.Title = title;
        Control.ItemsSource = items;
        if (initialValue != null)
        {
            var idx = Array.IndexOf(items, initialValue);
            if (idx >= 0) Control.SelectedIndex = idx;
        }
    }
    
    // public ValidatedPicker(
    //     Picker picker,
    //     string[] items,
    //     Label errorLabel,
    //     DelegateValidator validator)
    // {
    //     Control = picker;
    //     _items = items;
    //     ErrorLabel = errorLabel ?? throw new ArgumentNullException(nameof(errorLabel));
    //     Validator = validator ?? throw new ArgumentNullException(nameof(validator));
    //     Control.ItemsSource = items.ToList();
    //     Control.SelectedIndexChanged += OnSelectionChanged;
    //     
    //     Validate();
    // }

    // private void OnSelectionChanged(object? sender, EventArgs e)
    // {
    //     SelectedValue = Control.SelectedIndex >= 0
    //         ? _items[Control.SelectedIndex]
    //         : "";
    //     Validate();
    // }

    // public void Validate()
    // {
    //     (IsValid, _currentError) = Validator(SelectedValue);
    //     ErrorLabel.Text = _currentError;
    //     ErrorLabel.IsVisible = !string.IsNullOrEmpty(_currentError);
    // }
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

    public ValidatedDateField(string title, DelegateValidator validator, DateTime? initialValue = null)
    {
        // Control.;
        _validator = validator;
        Control.Date = initialValue ?? DateTime.Today;
    }

    public object? GetValue() => Control.Date;
    public void SetValue(object? value) => Control.Date = value is DateTime dt ? dt : DateTime.Today;
    View IInputControl.Control => Control;
}