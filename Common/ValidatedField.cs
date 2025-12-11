namespace Radiotech.Common;

public sealed class ValidatedField
{
    public Entry Control { get; }
    public Label ErrorLabel { get; }
    public DelegateValidator Validator { get; }

    public string? CurrentValue => Control.Text;
    public bool IsValid { get; private set; } = false;
    public string CurrentError { get; private set; } = string.Empty;

    public ValidatedField(Entry entry, Label errorLabel, DelegateValidator validator)
    {
        Control = entry;
        ErrorLabel = errorLabel;
        Validator = validator;

        Control.TextChanged += OnTextChanged;
        Validate();
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