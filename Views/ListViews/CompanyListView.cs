using Radiotech.Common;
using Radiotech.Data;
using Radiotech.ViewModels;

namespace Radiotech.Views.ListViews;

public class CompanyListView : ListViewBase<TableData.Company>
{
    private static readonly CompanyViewModel ViewModel = new CompanyViewModel();

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
		    new ValidatedEntry("Название", Validators.RequiredLettersOnly),
		    new ValidatedEntry("ФИО Руководителя", Validators.RequiredLettersOnly),
		    new ValidatedEntry("Банк", Validators.RequiredLettersOnly),
		    new ValidatedEntry("Расчетный счет", Validators.RequiredNotNull),
		    new ValidatedEntry("ИНН", Validators.RequiredNotNull),
		    new ValidatedEntry("Адрес", Validators.RequiredNotNull),
		    new ValidatedEntry("Телефон", Validators.RequiredNotNull),
		    new ValidatedEntry("Дисконтная карта", Validators.RequiredNotNull)
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
		    new ValidatedEntry("Название", Validators.RequiredLettersOnly, item.Name),
		    new ValidatedEntry("ФИО Руководителя", Validators.RequiredLettersOnly, item.ManagerFullName),
		    new ValidatedEntry("Банк", Validators.RequiredLettersOnly, item.Bank),
		    new ValidatedEntry("Расчетный счет", Validators.RequiredNotNull, item.AccountNum),
		    new ValidatedEntry("ИНН", Validators.RequiredNotNull, item.ITN),
		    new ValidatedEntry("Адрес", Validators.RequiredNotNull, item.Address),
		    new ValidatedEntry("Телефон", Validators.RequiredNotNull, item.Phone),
		    new ValidatedEntry("Дисконтная карта", Validators.RequiredNotNull, item.DiscountCard),
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