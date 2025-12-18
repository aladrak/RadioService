namespace Radiotech.Common;

public delegate (bool isValid, string error) DelegateValidator(string? field);

public interface IInputControl
{
    View Control { get; }
    Label ErrorLabel { get; }
    bool IsValid { get; }
    object? GetValue();
    // void Validate();
    event Action<string?>? ValueChanged;
    void SetValue(object? value);
}