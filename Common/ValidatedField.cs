namespace Radiotech.Common;

// public abstract class AbstractField
// {
//     public abstract View Control { get; }
//     public Label ErrorLabel { get; }
//     public abstract void Validate();
//     public abstract object GetValue();
//     public abstract bool IsValid { get; }
// }

public sealed class ValidatedField
{
    public Entry Control { get; }
    public Label ErrorLabel { get; }
    public DelegateValidator Validator { get; }
    public string CurrentValue => Control.Text;
    public bool IsValid { get; private set; } = false;
    private string CurrentError { get; set; } = string.Empty;

    public ValidatedField(Entry entry, Label errorLabel, DelegateValidator validator)
    {   
        Control = entry;
        ErrorLabel = errorLabel;
        Validator = validator;

        Control.TextChanged += OnTextChanged;
        // Validate();
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        Validate();
    }

    public void Validate()
    {
        (IsValid, CurrentError) = Validator(Control.Text);
        ErrorLabel.Text = CurrentError;
        ErrorLabel.IsVisible = !string.IsNullOrEmpty(CurrentError);
    }
}

public sealed class ValidatedPicker
{
    public Picker Control { get; }
    public Label ErrorLabel { get; }
    public bool IsValid { get; private set; }
    public string? SelectedValue { get; private set; }
    public DelegateValidator Validator { get; }
    private string[] _items;
    private string _currentError = string.Empty;

    public ValidatedPicker(
        Picker picker,
        string[] items,
        Label errorLabel,
        DelegateValidator validator,
        string? initialValue = default)
    {
        Control = picker;
        ErrorLabel = errorLabel ?? throw new ArgumentNullException(nameof(errorLabel));
        Validator = validator ?? throw new ArgumentNullException(nameof(validator));
        Control.ItemsSource = items.ToList();
        Control.SelectedIndexChanged += OnSelectionChanged;
        
        Validate();
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        SelectedValue = Control.SelectedIndex >= 0
            ? _items[Control.SelectedIndex]
            : "";
        Validate();
    }

    public void Validate()
    {
        (IsValid, _currentError) = Validator(SelectedValue);
        ErrorLabel.Text = _currentError;
        ErrorLabel.IsVisible = !string.IsNullOrEmpty(_currentError);
    }
}