using Radiotech.Data;
using Radiotech.ViewModels;

namespace Radiotech;

public class MainPage : ContentPage
{
	private readonly CollectionView _collectionView;
    private readonly MainViewModel _viewModel;

    public MainPage()
    {
        _viewModel = new MainViewModel();

        _collectionView = new CollectionView
        {
            SelectionMode = SelectionMode.Single,
            ItemsSource = _viewModel.Persons,
            ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    Padding = 10,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 40 },
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Star }
                    }
                };

                var idLabel = new Label();
                idLabel.SetBinding(Label.TextProperty, nameof(TableData.Person.PersonID));
                grid.Add(idLabel, 0, 0);

                var lastNameLabel = new Label();
                lastNameLabel.SetBinding(Label.TextProperty, nameof(TableData.Person.LastName));
                grid.Add(lastNameLabel, 1, 0);

                var firstNameLabel = new Label();
                firstNameLabel.SetBinding(Label.TextProperty, nameof(TableData.Person.FirstName));
                grid.Add(firstNameLabel, 2, 0);

                var midNameLabel = new Label();
                midNameLabel.SetBinding(Label.TextProperty, nameof(TableData.Person.MidName));
                grid.Add(midNameLabel, 3, 0);

                var addressLabel = new Label();
                addressLabel.SetBinding(Label.TextProperty, nameof(TableData.Person.Address));
                grid.Add(addressLabel, 4, 0);

                var phoneLabel = new Label();
                phoneLabel.SetBinding(Label.TextProperty, nameof(TableData.Person.Phone));
                grid.Add(phoneLabel, 5, 0);

                return grid;
            })
        };

        Content = _collectionView;
    }
	// private CollectionView collectionView;
	// public MainPage()
	// {
	// 	Title = "Radio-Service";
	// 	collectionView = new CollectionView
	// 	{
	// 		IsGrouped = true,
	// 		SelectionMode = SelectionMode.None,
	// 		BackgroundColor = Colors.Transparent,
	// 		// ItemTemplate = new DataTemplate( () =>
	// 		// {
	// 		// 	var border = new Border
	// 		// 	{
	// 		// 		BackgroundColor = Colors.White,
	// 		// 		Padding = 10,
	// 		// 		Margin = new Thickness(16, 8),
	// 		// 		InputTransparent = false
	// 		// 	};
	// 		// } )
	// 	};
	// 	Content = new VerticalStackLayout
	// 	{
	// 		Children = {
	// 			new Label { 
	// 			HorizontalOptions = LayoutOptions.Center, 
	// 			VerticalOptions = LayoutOptions.Center, 
	// 			Text = "Welcome to .NET MAUI!"
	// 			}
	// 		}
	// 	};
	// }
}