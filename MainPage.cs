namespace Radiotech;

public class MainPage : ContentPage
{
	private CollectionView collectionView;
	public MainPage()
	{
		Title = "Radio-Service";
		collectionView = new CollectionView
		{
			IsGrouped = true,
			SelectionMode = SelectionMode.None,
			BackgroundColor = Colors.Transparent,
			// ItemTemplate = new DataTemplate( () =>
			// {
			// 	var border = new Border
			// 	{
			// 		BackgroundColor = Colors.White,
			// 		Padding = 10,
			// 		Margin = new Thickness(16, 8),
			// 		InputTransparent = false
			// 	};
			// } )
		};
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { 
				HorizontalOptions = LayoutOptions.Center, 
				VerticalOptions = LayoutOptions.Center, 
				Text = "Welcome to .NET MAUI!"
				}
			}
		};
	}
}