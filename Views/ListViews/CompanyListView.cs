using Radiotech.Common;
using Radiotech.Data;
using Radiotech.ViewModels;

namespace Radiotech.Views.ListViews;

public class CompanyListView : ListViewBase<TableData.Company>
{
    private static readonly CompanyViewModel ViewModel = new();

    public CompanyListView() : base(
	    "Компания",
	    ["ID", "Название", "ФИО Руководителя", "Банк", "Р.Счет", "ИНН", "Адрес", "Телефон", "Карта"],
	    ViewModel.Companies,
	    CollectionItem)
    {
	    // Nothing now
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
			    new ColumnDefinition { Width = GridLength.Star },
			    new ColumnDefinition { Width = GridLength.Star },
			    new ColumnDefinition { Width = GridLength.Star }
		    }
	    };

	    var idLabel = new Label();
	    idLabel.SetBinding(Label.TextProperty, nameof(TableData.Company.CompanyID));
	    grid.Add(idLabel, 0, 0);
	    
	    var nameLabel = new Label();
	    nameLabel.SetBinding(Label.TextProperty, nameof(TableData.Company.Name));
	    grid.Add(nameLabel, 1, 0);

	    var managerLabel = new Label();
	    managerLabel.SetBinding(Label.TextProperty, nameof(TableData.Company.ManagerFullName));
	    grid.Add(managerLabel, 2, 0);

	    var bankLabel = new Label();
	    bankLabel.SetBinding(Label.TextProperty, nameof(TableData.Company.Bank));
	    grid.Add(bankLabel, 3, 0);

	    var accountLabel = new Label();
	    accountLabel.SetBinding(Label.TextProperty, nameof(TableData.Company.AccountNum));
	    grid.Add(accountLabel, 4, 0);

	    var itnLabel = new Label();
	    itnLabel.SetBinding(Label.TextProperty, nameof(TableData.Company.ITN));
	    grid.Add(itnLabel, 5, 0);
	    
	    var addressLabel = new Label();
	    addressLabel.SetBinding(Label.TextProperty, nameof(TableData.Company.Address));
	    grid.Add(addressLabel, 6, 0);
	    
	    var phoneLabel = new Label();
	    phoneLabel.SetBinding(Label.TextProperty, nameof(TableData.Company.Phone));
	    grid.Add(phoneLabel, 7, 0);
	    
	    var cardLabel = new Label();
	    cardLabel.SetBinding(Label.TextProperty, nameof(TableData.Company.DiscountCard));
	    grid.Add(cardLabel, 8, 0);

	    return grid;
    }
    protected override async Task ShowAddForm()
    {
	    var fields = new List<IInputControl>
	    {
		    new ValidatedEntry("Название", Validators.LettersSpacesCommasOnly),
		    new ValidatedEntry("ФИО Руководителя", Validators.LettersSpacesCommasOnly),
		    new ValidatedEntry("Банк", Validators.LettersSpacesCommasOnly),
		    new ValidatedEntry("Расчетный счет", 
			    s => Validators.DigitsOnlyFixedLength(s, 20)),
		    new ValidatedEntry("ИНН", 
			    s => Validators.DigitsOnlyFixedLength(s, 10)),
		    new ValidatedEntry("Адрес", Validators.RequiredNotNull),
		    new ValidatedEntry("Телефон", 
			    s => Validators.DigitsOnlyFixedLength(s, 11)),
		    new ValidatedEntry("Дисконтная карта", Validators.RequiredDiscountCard)
	    };
	    var inputView = new InputView<TableData.Company>(
		    "Добавить Юр. лицо",
		    fields,
		    ctrls => new TableData.Company
		    {
			    CompanyID = ViewModel.FreeId,
			    Name = (string?)ctrls[0].GetValue() ?? "",
			    ManagerFullName = (string?)ctrls[1].GetValue() ?? "",
			    Bank = (string?)ctrls[2].GetValue() ?? "",
			    AccountNum = (string?)ctrls[3].GetValue() ?? "",
			    ITN = (string?)ctrls[4].GetValue() ?? "",
			    Address = (string?)ctrls[5].GetValue() ?? "",
			    Phone = (string?)ctrls[6].GetValue() ?? "",
			    DiscountCard = (string?)ctrls[7].GetValue() ?? "",
		    },
		    onSuccess: result =>
		    {
			    ViewModel.Add(result);
			    DisplayAlertAsync("Успех", $"Вы ввели: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
    protected override async Task ShowEditForm(TableData.Company item)
    {
	    var fields = new List<IInputControl>
	    {
		    new ValidatedEntry("Название", Validators.LettersSpacesCommasOnly, item.Name),
		    new ValidatedEntry("ФИО Руководителя", Validators.LettersSpacesCommasOnly, item.ManagerFullName),
		    new ValidatedEntry("Банк", Validators.LettersSpacesCommasOnly, item.Bank),
		    new ValidatedEntry("Расчетный счет", 
			    s => Validators.DigitsOnlyFixedLength(s, 20), item.AccountNum),
		    new ValidatedEntry("ИНН", 
			    s => Validators.DigitsOnlyFixedLength(s, 10), item.ITN),
		    new ValidatedEntry("Адрес", Validators.RequiredNotNull, item.Address),
		    new ValidatedEntry("Телефон", 
			    s => Validators.DigitsOnlyFixedLength(s, 11), item.Phone),
		    new ValidatedEntry("Дисконтная карта", Validators.RequiredDiscountCard, item.DiscountCard),
	    };
	    var inputView = new InputView<TableData.Company>(
		    "Редактирование Юр. лица", 
		    fields,
		    ctrls => new TableData.Company
		    {
			    CompanyID = item.CompanyID,
			    Name = (string?)ctrls[0].GetValue() ?? "",
			    ManagerFullName = (string?)ctrls[1].GetValue() ?? "",
			    Bank = (string?)ctrls[2].GetValue() ?? "",
			    AccountNum = (string?)ctrls[3].GetValue() ?? "",
			    ITN = (string?)ctrls[4].GetValue() ?? "",
			    Address = (string?)ctrls[5].GetValue() ?? "",
			    Phone = (string?)ctrls[6].GetValue() ?? "",
			    DiscountCard = (string?)ctrls[7].GetValue() ?? "",
		    },
		    onSuccess: result =>
		    {
			    ViewModel.Update(item, result);
			    DisplayAlertAsync("Успешное редактирование", $"Вы ввели: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
    protected override void OnDelete(TableData.Company item) => ViewModel.Delete(item);
}