using CommunityToolkit.Maui.Views;

namespace Radiotech.Views;

public class InputPopup<T> : Popup
{
    public T? Result { get; private set; }
    private readonly Entry[] _entries;
    private readonly Func<Entry[], T> _factory;
    private readonly Button _okButton;
    public InputPopup(string title, string[] labels, Func<Entry[], T> factory)
    {
        Result = default(T);
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _entries = new Entry[labels.Length];

        var layout = new VerticalStackLayout { Padding = 20 };
        layout.Add(new Label { Text = title, FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 0, 0, 10) });
        for (int i = 0; i < labels.Length; i++)
        {
            var entry = new Entry
            {
                Placeholder = labels[i], 
                WidthRequest = 45,
            };
            _entries[i] = entry;
            layout.Add(entry);
        }
        
        var okbtn = new Button { Text = "OK" };
        okbtn.Clicked += OnClickedOk;
        layout.Add(okbtn);
        
        Content = layout;
    }

    private void OnClickedOk(object? sender, EventArgs e)
    {
        foreach (var entry in _entries)
            if (string.IsNullOrEmpty(entry.Text)) break;
        try
        {
            Result = _factory(_entries);
            CloseAsync();
        }
        catch (Exception ex)
        {
            _okButton.Text = $"Ошибка: {ex.Message}";
        }
    }
}