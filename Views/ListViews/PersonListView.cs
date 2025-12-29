using Radiotech.Common;
using Radiotech.Data;
using Radiotech.ViewModels;

namespace Radiotech.Views.ListViews;

public class PersonListView : ListViewBase<TableData.Person>
{
    private static readonly PersonViewModel ViewModel = new();

    public PersonListView() : base(
	    "Физ. лица",
	    ["ID", "Фамилия", "Имя", "Отчество", "Адрес", "Телефон", "Карта"],
	    ViewModel.Persons,
	    CollectionItem)
    {
	    Title = "Список физических лиц";
    }

    private static Grid CollectionItem()
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
	    
	    var cardLabel = new Label();
	    cardLabel.SetBinding(Label.TextProperty, nameof(TableData.Person.DiscountCard));
	    grid.Add(cardLabel, 6, 0);

	    return grid;
    }
    protected override async Task ShowAddForm()
    {
	    var fields = new List<IInputControl>
	    {
		    new ValidatedEntry("Имя", Validators.LettersOnly),
		    new ValidatedEntry("Отчество", Validators.RequiredMidName),
		    new ValidatedEntry("Фамилия", Validators.LettersOnly),
		    new ValidatedEntry("Адрес", Validators.RequiredNotNull),
		    new ValidatedEntry("Телефон", s => Validators.DigitsOnlyFixedLength(s, 11)),
		    new ValidatedEntry("Дисконтная карта", Validators.RequiredDiscountCard),
	    };
	    var inputView = new InputView<TableData.Person>(
		    "Добавление Физ. лица",
		    fields,
		    ctrls => new TableData.Person
		    {
			    PersonID = ViewModel.FreeId,
			    FirstName = (string?)ctrls[0].GetValue() ?? "",
			    MidName = (string?)ctrls[1].GetValue() ?? "",
			    LastName = (string?)ctrls[2].GetValue() ?? "",
			    Address = (string?)ctrls[3].GetValue() ?? "",
			    Phone = (string?)ctrls[4].GetValue() ?? "",
			    DiscountCard = (string?)ctrls[5].GetValue() ?? ""
		    },
		    onSuccess: result =>
		    {
			    ViewModel.Add(result);
			    DisplayAlertAsync("Успех", $"Вы ввели: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
    protected override async Task ShowEditForm(TableData.Person item)
    {
	    var fields = new List<IInputControl>
	    {
		    new ValidatedEntry("Имя", Validators.LettersOnly, item.FirstName),
		    new ValidatedEntry("Отчество", Validators.RequiredMidName, item.MidName),
		    new ValidatedEntry("Фамилия", Validators.LettersOnly, item.LastName),
		    new ValidatedEntry("Адрес", Validators.RequiredNotNull, item.Address),
		    new ValidatedEntry("Телефон", 
			    s => Validators.DigitsOnlyFixedLength(s, 11), item.Phone),
		    new ValidatedEntry("Дисконтная карта", Validators.RequiredDiscountCard, item.DiscountCard)
	    };
	    var inputView = new InputView<TableData.Person>(
		    "Редактирование Физ. лица", 
		    fields,
		    ctrls => new TableData.Person
		    {
			    PersonID = item.PersonID,
			    FirstName = (string?)ctrls[0].GetValue() ?? "",
			    MidName = (string?)ctrls[1].GetValue() ?? "",
			    LastName = (string?)ctrls[2].GetValue() ?? "",
			    Address = (string?)ctrls[3].GetValue() ?? "",
			    Phone = (string?)ctrls[4].GetValue() ?? "",
			    DiscountCard = (string?)ctrls[5].GetValue() ?? ""
		    },
		    onSuccess: result =>
		    {
			    ViewModel.Update(item, result);
			    DisplayAlertAsync("Успешное редактирование", $"Вы ввели: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
    protected override void OnDelete(TableData.Person item) => ViewModel.Delete(item);
}