using Radiotech.Views;

namespace Radiotech;

public class MainPage : ContentPage
{
	public MainPage()
    {
        // Title = "Main";
        Button toCommonPageBtn = new Button
        {
            Text = "Person",
            HorizontalOptions = LayoutOptions.Start
        };
        toCommonPageBtn.Clicked += ToPersonPage;
 
        Content = new StackLayout { Children = { toCommonPageBtn } };
    }
    private async void ToPersonPage(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new PersonListView());
    }
}