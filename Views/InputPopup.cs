using CommunityToolkit.Maui.Views;

namespace Radiotech.Views;

public class InputPopup<T> : Popup
{
    public T Result { get; private set; }
    public InputPopup(string title)
    {
        Result = default(T);
        var entryList = new List<Entry>();
        foreach (var field in typeof(T).GetProperties())
        {
            entryList.Add(new Entry
            {
                Text = field.Name,
                Placeholder = string.Empty,
            });
        }

        var collection = new CollectionView()
        {
            ItemsSource = entryList,
        };

        var okbtn = new Button
        {
            Text = "OK",
        };
        okbtn.Clicked += (s, a) =>
        {
            foreach (var entry in entryList)
                
            CloseAsync();
        };
        Content = new Border
        {
            Content = new VerticalStackLayout
            {
                Children = { 
                    new Label { Text = title, Margin = new Thickness(10) }, 
                    collection, 
                    okbtn 
                }
            },
            Padding = 0
        };
    }
}