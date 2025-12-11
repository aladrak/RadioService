namespace Radiotech.Views;

public class InputView<T> : ContentPage
{
    private T? Result { get; set; }
    private readonly Action<T>? _onSuccess;
    private readonly Entry[] _entries;
    private readonly Func<Entry[], T> _factory;
    private readonly Button _okButton;
    public InputView(
        string title, 
        string[] labels, 
        Func<Entry[], T> factory, 
        Action<T>? onSuccess = null)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _entries = new Entry[labels.Length];
        _onSuccess = onSuccess;

        var layout = new VerticalStackLayout { Padding = 20 };
        layout.Add(new Label { Text = title, TextColor = Colors.Black, FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 0, 0, 10) });
        for (int i = 0; i < labels.Length; i++)
        {
            var entry = new Entry
            {
                TextColor = Colors.Black,
                Placeholder = labels[i], 
                WidthRequest = 90,
            };
            _entries[i] = entry;
            layout.Add(entry);
        }
        
        _okButton = new Button { Text = "OK" };
        _okButton.Clicked += OnClickedSave;
        layout.Add(_okButton);
        
        Content = layout;
    }

    private void OnClickedSave(object? sender, EventArgs e)
    {
        try
        {
            foreach (var entry in _entries)
                if (string.IsNullOrEmpty(entry.Text))
                {
                    DisplayAlertAsync("Erorr", "Empty area.", "OK");
                    break;
                }
            Result = _factory(_entries);
            _onSuccess?.Invoke(Result);
            Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            _okButton.Text = $"Ошибка: {ex.Message}";
        }
    }
    private void OnClickedCancel(object? sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
}