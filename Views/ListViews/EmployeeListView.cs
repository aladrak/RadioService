using Radiotech.Common;
using Radiotech.Data;
using Radiotech.Ui;
using Radiotech.ViewModels;

namespace Radiotech.Views.ListViews;

public class EmployeeListView : ListViewBase<TableData.Employee>
{
	// private class EmployeeDisplayItem
	// {
	// 	public TableData.Employee Source { get; }
	// 	public string Specialty { get; }
	// 	public string EmployeeName { get; }
	// 	public string CustomerName { get; }
	// 	public string FaultsDisplay { get; }
	//
	// 	public EmployeeDisplayItem(
	// 		TableData.Employee employee, 
	// 		IReadOnlyDictionary<int, string> productDict)
	// 	{
	// 		Source = employee;
	// 		Speciatly = specialtyDict.GetValueOrDefault(employee.SpecialtyID, $"[? specialty {employee.SpecialtyID}]");
	// 	}
	// }
	
    private static readonly EmployeeViewModel ViewModel = new();
    public EmployeeListView() : base(
	    "Сотрудника",
	    ["ID", "Специальность", "Фамилия", "Имя", "Отчество", "Адрес", "Телефон", "Возраст", "Стаж"],
	    ViewModel.Employees,
	    CollectionItem)
    {
	    Title = "Список сотрудников";
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
			    new ColumnDefinition { Width = GridLength.Star }
		    }
	    };

	    var idLabel = new Label();
	    idLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.EmployeeID));
	    grid.Add(idLabel, 0, 0);
	    
	    var idSpecialtyLabel = new Label();
	    idSpecialtyLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.SpecialtyID));
	    grid.Add(idSpecialtyLabel, 1, 0);

	    var lastNameLabel = new Label();
	    lastNameLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.LastName));
	    grid.Add(lastNameLabel, 2, 0);

	    var firstNameLabel = new Label();
	    firstNameLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.FirstName));
	    grid.Add(firstNameLabel, 3, 0);

	    var midNameLabel = new Label();
	    midNameLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.MidName));
	    grid.Add(midNameLabel, 4, 0);

	    var addressLabel = new Label();
	    addressLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.Address));
	    grid.Add(addressLabel, 5, 0);

	    var phoneLabel = new Label();
	    phoneLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.Phone));
	    grid.Add(phoneLabel, 6, 0);
	    
	    var ageLabel = new Label();
	    ageLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.Age));
	    grid.Add(ageLabel, 7, 0);
	    
	    var skillLabel = new Label();
	    skillLabel.SetBinding(Label.TextProperty, nameof(TableData.Employee.Skill));
	    grid.Add(skillLabel, 8, 0);

	    return grid;
    }
    protected override async Task ShowAddForm()
    {
	    var fields = new List<IInputControl>
	    {
		    new ValidatedPicker("Специальность", ViewModel.Specialties
			    .Select<TableData.Specialty, string>(n => n.Name).ToArray(), Validators.RequiredNotNull),
		    new ValidatedEntry("Имя", Validators.LettersOnly),
		    new ValidatedEntry("Отчество", Validators.RequiredMidName),
		    new ValidatedEntry("Фамилия", Validators.LettersOnly),
		    new ValidatedEntry("Адрес", Validators.RequiredNotNull),
		    new ValidatedEntry("Телефон", s => Validators.DigitsOnlyFixedLength(s, 11)),
		    new ValidatedEntry("Возраст", Validators.RequiredDigitsOnly),
		    new ValidatedEntry("Стаж", Validators.RequiredDigitsOnly)
	    };
	    var inputView = new InputView<TableData.Employee>(
		    "Добавить сотрудника",
		    fields,
		    ctrls => new TableData.Employee
		    {
			    EmployeeID = ViewModel.FreeId,
			    SpecialtyID = 
				    ViewModel.Specialties.FirstOrDefault(a => a.Name == ((string?)ctrls[0].GetValue() ?? "1"))!.SpecialtyID,
			    FirstName = (string?)ctrls[1].GetValue() ?? "",
			    MidName = (string?)ctrls[2].GetValue() ?? "",
			    LastName = (string?)ctrls[3].GetValue() ?? "",
			    Address = (string?)ctrls[4].GetValue() ?? "",
			    Phone = (string?)ctrls[5].GetValue() ?? "",
			    Age =  int.Parse((string?)ctrls[6].GetValue() ?? "18"),
			    Skill = int.Parse((string?)ctrls[7].GetValue() ?? "1")
		    },
		    onSuccess: result =>
		    {
			    ViewModel.Add(result);
			    DisplayAlertAsync("Успех", $"Вы ввели: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
    protected override async Task ShowEditForm(TableData.Employee item)
    {
	    var fields = new List<IInputControl>
	    {
		    new ValidatedPicker("Специальность", ViewModel.Specialties
			    .Select<TableData.Specialty, string>(n => n.Name).ToArray(), Validators.RequiredNotNull,
			    ViewModel.Specialties.FirstOrDefault(i => item.SpecialtyID == i.SpecialtyID)!.Name),
		    new ValidatedEntry("Имя", Validators.LettersOnly, item.FirstName),
		    new ValidatedEntry("Отчество", Validators.RequiredMidName, item.MidName),
		    new ValidatedEntry("Фамилия", Validators.LettersOnly, item.LastName),
		    new ValidatedEntry("Адрес", Validators.RequiredNotNull, item.Address),
		    new ValidatedEntry("Телефон", 
			    s => Validators.DigitsOnlyFixedLength(s, 11), item.Phone),
		    new ValidatedEntry("Возраст", Validators.RequiredDigitsOnly, item.Age.ToString()),
		    new ValidatedEntry("Стаж", Validators.RequiredDigitsOnly, item.Skill.ToString())
	    };
	    var inputView = new InputView<TableData.Employee>(
		    "Редактирование данных сотрудника", 
		    fields,
		    ctrls => new TableData.Employee
		    {
			    EmployeeID = item.EmployeeID,
			    SpecialtyID = 
				    ViewModel.Specialties.FirstOrDefault(a => a.Name == ((string?)ctrls[0].GetValue() ?? "1"))!.SpecialtyID,
			    FirstName = (string?)ctrls[1].GetValue() ?? "",
			    MidName = (string?)ctrls[2].GetValue() ?? "",
			    LastName = (string?)ctrls[3].GetValue() ?? "",
			    Address = (string?)ctrls[4].GetValue() ?? "",
			    Phone = (string?)ctrls[5].GetValue() ?? "",
			    Age =  int.Parse((string?)ctrls[6].GetValue() ?? "18"),
			    Skill = int.Parse((string?)ctrls[7].GetValue() ?? "1")
		    },
		    onSuccess: result =>
		    {
			    ViewModel.Update(item, result);
			    DisplayAlertAsync("Успешное редактирование", $"Вы ввели: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
    protected override void OnDelete(TableData.Employee item) => ViewModel.Delete(item);
}