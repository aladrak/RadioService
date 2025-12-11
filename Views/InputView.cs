using Radiotech.Common;

namespace Radiotech.Views;

public class InputView<T> : ContentPage
{
    private T? Result { get; set; }
    private readonly Action<T>? _onSuccess;
    private readonly List<ValidatedField> _fields = [];
    private readonly Func<ValidatedField[], T> _factory;
    private readonly Button _okButton;
    public InputView(
        string title,
        List<(string, DelegateValidator)> fieldConfigs, 
        Func<ValidatedField[], T> factory, 
        Action<T>? onSuccess = null)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _onSuccess = onSuccess;

        var layout = new VerticalStackLayout { Padding = 20 };
        layout.Add(new Label { Text = title, FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 0, 0, 10) });
        foreach (var (placeholder, validator) in fieldConfigs)
        {
            var entry = new Entry
            {
                Placeholder = placeholder, 
                WidthRequest = 90,
            };
            var error = new Label {TextColor =  Colors.Red, IsVisible = false};
            var validatedField = new ValidatedField(entry, error, validator);
            _fields.Add(validatedField);
            
            layout.Children.Add(entry);
            layout.Children.Add(error);
        }

        _okButton = new Button { Text = "Save", IsEnabled = false };
        _okButton.Clicked += OnClickedSave;
        var cancelButton = new Button { Text = "Cancel" };
        cancelButton.Clicked += (s, e) => Navigation.PopModalAsync();
        
        var btnLayout = new StackLayout 
        { 
            Padding = 20 , 
            Children = { _okButton, cancelButton }
        };
        
        layout.Add(btnLayout);
        Content = layout;
        
        foreach (var field in _fields)
        {
            field.Control.TextChanged += (_, _) => UpdateSubmitButton();
        }
    }

    private void OnClickedSave(object? sender, EventArgs e)
    {
        try
        {
            Result = _factory(_fields.ToArray());
            _onSuccess?.Invoke(Result);
            Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            _okButton.Text = $"Ошибка: {ex.Message}";
        }
    }
    private void UpdateSubmitButton()
    {
        bool allValid = _fields.All(f => f.IsValid);
        _okButton.IsEnabled = allValid;
    }
    
    private async void OnSubmit(object sender, EventArgs e)
    {
        // Собираем данные
        var values = _fields.Select(f => f.CurrentValue).ToArray();
        await DisplayAlertAsync("Успех", $"Данные: {string.Join(", ", values)}", "OK");
    }
}