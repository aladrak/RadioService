using System.Net.Mime;
using Radiotech.Common;

namespace Radiotech.Views;

public class InputView<T> : ContentPage
{
    private readonly Action<T>? _onSuccess;
    private readonly List<ValidatedField> _fields = [];
    private readonly List<ValidatedPicker> _pickers = [];
    private readonly Func<ValidatedField[], ValidatedPicker[], T> _factory;
    private readonly Button _okButton;
    public InputView(
        string title,
        List<(string, string[]?, DelegateValidator)> fieldConfigs, 
        Func<ValidatedField[], ValidatedPicker[], T> factory, 
        Action<T>? onSuccess = null)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _onSuccess = onSuccess;

        var layout = new VerticalStackLayout { Padding = 20 };
        layout.Add(new Label { Text = title, FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 0, 0, 10) });
        foreach (var (placeholder, pickerItems, validator) in fieldConfigs)
        {
            var error = new Label {TextColor =  Colors.Red, IsVisible = false};
            if (pickerItems == null)
            {
                var entry = new Entry
                {
                    Placeholder = placeholder,
                    WidthRequest = 240,
                };
                _fields.Add(new ValidatedField(entry, error, validator));
                layout.Children.Add(entry);
            }
            else
            {
                var picker = new Picker
                {
                    Title = placeholder,
                    WidthRequest = 240,
                };
                _pickers.Add(new ValidatedPicker(picker, pickerItems, error, validator));
                layout.Children.Add(picker);
            }
            
            layout.Children.Add(error);
        }

        _okButton = new Button { Text = "Save", WidthRequest = 100, BackgroundColor = Colors.Green, IsEnabled = false };
        _okButton.Clicked += OnClickedSave;
        var cancelButton = new Button { Text = "Cancel", WidthRequest = 100 };
        cancelButton.Clicked += (_, _) => Navigation.PopModalAsync();
        
        var btnLayout = new HorizontalStackLayout() 
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
            // _results.Add();
            // Result = _factory(_fields.ToArray());
            _onSuccess?.Invoke(_factory(_fields.ToArray(), _pickers.ToArray()));
            Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            _okButton.Text = $"Ошибка: {ex.Message}";
        }
    }
    private void UpdateSubmitButton()
    {
        var allValid = _fields.All(f => f.IsValid);
        _okButton.IsEnabled = allValid;
    }
}