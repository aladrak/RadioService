using Radiotech.Common;

namespace Radiotech.Views;

public class InputView<T> : ContentPage
{
    private List<IInputControl> _controls;
    private readonly Func<IInputControl[], T> _factory;
    private readonly Action<T>? _onSuccess;
    private readonly Button _saveButton;
    
    public InputView(
        string? title,
        List<IInputControl> controls, 
        Func<IInputControl[], T> factory, 
        Action<T>? onSuccess = null)
    {
        _controls = controls;
        _factory = factory;
        _onSuccess = onSuccess;
        
        var layout = new VerticalStackLayout { Padding = 20 };
        layout.Add(new Label { Text = title, FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 0, 0, 10) });
        
        foreach (var control in _controls)
        {
            layout.Add(control.Control);
            layout.Add(control.ErrorLabel);
        }

        _saveButton = new Button { Text = "Save", WidthRequest = 100, BackgroundColor = Colors.Green, 
            IsEnabled = false 
        };
        _saveButton.Clicked += OnSave;

        var cancel = new Button { Text = "Cancel", WidthRequest = 100 };
        cancel.Clicked += (_, _) => Navigation.PopModalAsync();

        layout.Add(new HorizontalStackLayout { Children = { _saveButton, cancel } });
        Content = layout;
        
        foreach (var ctrl in _controls)
            ctrl.ValueChanged += _ => UpdateSaveButton();
    }

    private async void OnSave(object? sender, EventArgs e)
    {
        try
        {
            var result = _factory(_controls.ToArray());
            _onSuccess?.Invoke(result);
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ошибка:", ex.Message,"OK");
        }
    }
    private void UpdateSaveButton()
    {
        var allValid = _controls.All(f => f.IsValid);
        _saveButton.IsEnabled = allValid;
    }
}