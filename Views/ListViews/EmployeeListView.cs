using Radiotech.Common;
using Radiotech.Data;
using Radiotech.ViewModels;

namespace Radiotech.Views.ListViews;

public class EmployeeListView : ContentPage
{
    private readonly CollectionView _collectionView;
    private readonly EmployeeViewModel _viewModel;

    public EmployeeListView()
    {
        _viewModel = new EmployeeViewModel();

        _collectionView = new CollectionView
        {
            SelectionMode = SelectionMode.Single,
            ItemsSource = _viewModel.Employees,
            ItemTemplate = CollectionItem()
        };
        _collectionView.SelectionChanged += async (_, e) =>
        {
	        if (e.CurrentSelection.FirstOrDefault() is not TableData.Employee selectedItem)
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
        var addButton = new Button{ Text = "Add Employee" };
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

    private static DataTemplate CollectionItem()
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
				    new ColumnDefinition { Width = 40 },
				    new ColumnDefinition { Width = GridLength.Star },
				    new ColumnDefinition { Width = GridLength.Star },
				    new ColumnDefinition { Width = GridLength.Star },
				    new ColumnDefinition { Width = GridLength.Star },
				    new ColumnDefinition { Width = GridLength.Star },
				    new ColumnDefinition { Width = GridLength.Star },
				    new ColumnDefinition { Width = GridLength.Star }
			    }
		    };

		    var idLabel = new Label();
		    idLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.EmployeeID));
		    grid.Add(idLabel, 0, 0);

		    var lastNameLabel = new Label();
		    lastNameLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.LastName));
		    grid.Add(lastNameLabel, 1, 0);

		    var firstNameLabel = new Label();
		    firstNameLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.FirstName));
		    grid.Add(firstNameLabel, 2, 0);

		    var midNameLabel = new Label();
		    midNameLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.MidName));
		    grid.Add(midNameLabel, 3, 0);

		    var addressLabel = new Label();
		    addressLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.Address));
		    grid.Add(addressLabel, 4, 0);

		    var phoneLabel = new Label();
		    phoneLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.Phone));
		    grid.Add(phoneLabel, 5, 0);
		    
		    var ageLabel = new Label();
		    ageLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.Age));
		    grid.Add(ageLabel, 6, 0);
		    
		    var skillLabel = new Label();
		    phoneLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.Skill));
		    grid.Add(skillLabel, 7, 0);

		    return grid;
	    });
    }
    private async void ShowInputView(object? sender, EventArgs e)
    {
	    List<(string, string[]?, DelegateValidator)> configFields = [
		    ("Специальность", ["Рыбак", "Кучер"], Validators.RequiredNotNull),
		    ("Имя", null, Validators.RequiredLettersOnly),
		    ("Отчество", null, Validators.RequiredMidName),
		    ("Фамилия", null, Validators.RequiredLettersOnly),
		    ("Адрес", null, Validators.RequiredNotNull),
		    ("Телефон", null, s => Validators.RequiredDigitsOnlyFixedLength(s, 11))
	    ];
	    var inputView = new InputView<TableData.Employee>(
		    "New Employee", 
		    configFields,
		    (fields, _) => new TableData.Employee
		    {
			    EmployeeID = _viewModel.FreeId,
			    SpecialtyID = -1,
			    FirstName = fields[0].CurrentValue ?? "",
			    MidName = fields[1].CurrentValue ?? "",
			    LastName = fields[2].CurrentValue ?? "",
			    Address = fields[3].CurrentValue ?? "",
			    Phone = fields[4].CurrentValue ?? "",
			    Age = -1,
			    Skill = -1
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