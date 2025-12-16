using Radiotech.Common;
using Radiotech.Data;
using Radiotech.Ui;
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
            ItemTemplate = PersonItem()
        };
        _collectionView.SelectionChanged += async (_, e) =>
        {
	        if (e.CurrentSelection.FirstOrDefault() is not TableData.Person selectedItem)
	        {
		        _collectionView.SelectedItem = null;
		        return;
	        }
	        
	        _collectionView.SelectedItem = null;
	        
	        string action = await Shell.Current.DisplayActionSheetAsync(
		        $"Action on «{selectedItem}»",
		        "Cancel",
		        null,
		        "Edit",
		        "Delete"
	        );

	        if (action == "Edit")
	        {
		        // TODO: Edit
	        }
	        else if (action == "Delete")
	        {
		        bool confirm = await Shell.Current.DisplayAlertAsync(
			        "Confirmation",
			        $"Delete «{selectedItem}»?",
			        "Yes", "No"
		        );

		        if (confirm)
		        {
			        _viewModel.Delete(selectedItem);
		        }
	        }
        };
        var addButton = new Button{ Text = "Add Person" };
		addButton.Clicked += ShowInputView;
		Content = new ScrollView
		{
			Content = new VerticalStackLayout
			{
				Spacing = 24,
				Padding = 4,
				Children =
				{
					UiTemplates.HeaderGrid(["ID", "Фамилия", "Имя", "Отчество", "Адрес", "Телефон"]),
					_collectionView, 
					addButton
				}
			}
		};
    }

    private static DataTemplate PersonItem()
    {
	    return new DataTemplate(() =>
	    {
		    var grid = new Grid
		    {
			    VerticalOptions = LayoutOptions.Center,
			    Padding = 10,
			    ColumnSpacing = 10,
			    ColumnDefinitions =
			    {
				    new ColumnDefinition { Width = 20 },
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
	    });
    }
    private async void ShowInputView(object? sender, EventArgs e)
    {
	    List<(string, string[]?, DelegateValidator)> configFields = [
		    ("Имя", null, Validators.RequiredLettersOnly),
		    ("Отчество", null, Validators.RequiredMidName),
		    ("Фамилия", null, Validators.RequiredLettersOnly),
		    ("Адрес", null, Validators.RequiredNotNull),
		    ("Телефон", null, s => Validators.RequiredDigitsOnlyFixedLength(s, 11))
	    ];
	    var inputView = new InputView<TableData.Person>(
		    "New Person", 
		    configFields,
		    (fields, _) => new TableData.Person
		    {
			    PersonID = _viewModel.FreeId,
			    FirstName = fields[0].CurrentValue ?? "",
			    MidName = fields[1].CurrentValue ?? "",
			    LastName = fields[2].CurrentValue ?? "",
			    Address = fields[3].CurrentValue ?? "",
			    Phone = fields[4].CurrentValue ?? ""
		    },
		    onSuccess: result =>
		    {
			    _viewModel.Add(result);
			    DisplayAlertAsync("Success", $"You entered: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
}