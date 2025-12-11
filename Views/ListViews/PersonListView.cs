using CommunityToolkit.Maui.Extensions;
using Radiotech.Common;
using Radiotech.Data;
using Radiotech.ViewModels;

namespace Radiotech.Views.ListViews;

public class PersonListView : ContentPage
{
	private readonly CollectionView _collectionView;
    private readonly PersonViewModel _viewModel;

    public PersonListView()
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
		var addButton = new Button{ Text = "Add Person" };
		addButton.Clicked += ShowInputView;
		Content = new ScrollView
		{
			Content = new VerticalStackLayout
			{
				Spacing = 24,
				Padding = 4,
				Children = { _collectionView, addButton }
			}
		};
    }
    
    private async void ShowInputView(object? sender, EventArgs e)
    {
	    List<(string, DelegateValidator)> configFields = [
		    ("ID", s => Validators.RequiredMinLength(s, 2)), 
		    ("Имя", Validators.RequiredLettersOnly),
		    ("Отчество", Validators.RequiredLettersOnly),
		    ("Фамилия", Validators.RequiredLettersOnly),
		    ("Адрес", s => (true, string.Empty)),
		    ("Телефон", s => (true, string.Empty)) 
	    ];
	    var inputView = new InputView<TableData.Person>(
		    "New Person", 
		    configFields,
		    fields => new TableData.Person
		    {
			    PersonID = int.Parse(fields[0].Control.Text ?? "0"),
			    FirstName = fields[1].Control.Text ?? "",
			    MidName = fields[2].Control.Text ?? "",
			    LastName = fields[3].Control.Text ?? "",
			    Address = fields[4].Control.Text ?? "",
			    Phone = fields[5].Control.Text ?? ""
		    },
		    onSuccess: result =>
		    {
			    _viewModel.Add(result);
			    DisplayAlertAsync("Success", $"You entered: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
		// DisplayAlertAsync("Error", "Nothing to add.", "OK").Wait();
    }
}