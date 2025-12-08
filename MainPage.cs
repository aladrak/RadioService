using CommunityToolkit.Maui.Extensions;
using Radiotech.Data;
using Radiotech.ViewModels;
using Radiotech.Views;

namespace Radiotech;

public class MainPage : ContentPage
{
	private readonly CollectionView _collectionView;
    private readonly PersonViewModel _viewModel;

    public MainPage()
    {
        _viewModel = new PersonViewModel();

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
		var btn = new Button() { Text = "Add Person" };
		btn.Clicked += OnShowPopupClicked;
		// Content = _collectionView;
		Content = new ScrollView
		{
			Content = new VerticalStackLayout
			{
				Spacing = 24,
				Padding = 4,
				Children = { _collectionView, btn }
			}
		};
    }
    [Obsolete("Obsolete")]
    private async void OnShowPopupClicked(object? sender, EventArgs e)
    {
	    string[] labels = ["ID", "Имя", "Отчество", "Фамилия", "Адрес", "Телефон"];
	    var popup = new InputPopup<TableData.Person>(
		    "New Person", 
		    labels,
		    entries => new TableData.Person
		    {
			    PersonID = int.Parse(entries[0].Text ?? "0"),
			    FirstName = entries[1].Text ?? "",
			    MidName = entries[2].Text ?? "",
			    LastName = entries[3].Text ?? "",
			    Address = entries[4].Text ?? "",
			    Phone = entries[5].Text ?? ""
		    }
	    );

	    await this.ShowPopupAsync(popup);
	    var result = popup.Result;
	    if (result != null)
	    {
		    await DisplayAlert("Успех", $"Вы ввели: {result}", "OK");
		    _viewModel.Persons.Add(result);
	    }
	    else
	    {
		    await DisplayAlert("Ошибка", "Ошибка", "OK");
	    }
    }
}