using Radiotech.Common;
using Radiotech.Data;
using Radiotech.ViewModels;

namespace Radiotech.Views.ListViews;

public class OrderListView : ListViewBase<TableData.Order>
{
    private static readonly OrderViewModel ViewModel = new();

    public OrderListView() : base(
	    "Заказ",
	    ["ID", "Изделие", "Сотрудник", "Заказчик", "Дата начала", "Дата конца", "Список проблем", "Цена"],
	    ViewModel.Orders,
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
			    new ColumnDefinition { Width = GridLength.Star }
		    }
	    };

	    var idLabel = new Label();
	    idLabel.SetBinding(Label.TextProperty, nameof(TableData.Product.ProductID));
	    grid.Add(idLabel, 0, 0);
	    
	    var typeLabel = new Label();
	    typeLabel.SetBinding(Label.TextProperty, nameof(TableData.Product.Type));
	    grid.Add(typeLabel, 1, 0);
	    
	    var markLabel = new Label();
	    markLabel.SetBinding(Label.TextProperty, nameof(TableData.Product.Mark));
	    grid.Add(markLabel, 2, 0);

	    var countryLabel = new Label();
	    countryLabel.SetBinding(Label.TextProperty, nameof(TableData.Product.Country));
	    grid.Add(countryLabel, 3, 0);

	    var manufactLabel = new Label();
	    manufactLabel.SetBinding(Label.TextProperty, nameof(TableData.Product.Manufacturer));
	    grid.Add(manufactLabel, 4, 0);

	    var photoLabel = new Label();
	    photoLabel.SetBinding(Label.TextProperty, nameof(TableData.Product.Photo));
	    grid.Add(photoLabel, 5, 0);

	    var serviceLabel = new Label();
	    serviceLabel.SetBinding(Label.TextProperty, nameof(TableData.Product.ServiceLife));
	    grid.Add(serviceLabel, 6, 0);

	    return grid;
    }
    protected override async Task ShowAddForm()
    {
	    var fields = new List<IInputControl>
	    {
		    new ValidatedPicker("Изделие", 
			    ViewModel.Products.Select<TableData.Product, string>(product => product.Type).ToArray()),
		    new ValidatedPicker("Сотрудник", 
			    ViewModel.Employees.Select<TableData.Employee, string>(e => e.FirstName).ToArray()),
		    new ValidatedPicker("Заказчик", []),
		    
		    new ValidatedDateField("Дата начала работы", Validators.RequiredNotNull),
		    new ValidatedDateField("Дата окончания работы", Validators.RequiredNotNull),
		    // Список поломок
		    new ValidatedEntry("Стоимость", Validators.RequiredNotNull)
	    };
	    var inputView = new InputView<TableData.Order>(
		    "Добавление заказа", 
		    fields,
		    ctrls => new TableData.Order
		    {
			    OrderID = ViewModel.FreeId,
			    ProductID = (int?)ctrls[0].GetValue() ?? 0,
			    EmployeeID = (int?)ctrls[1].GetValue() ?? 0,
			    CompanyID = (int?)ctrls[2].GetValue(),
			    PersonID = (int?)ctrls[3].GetValue(),
			    StartDate = (DateOnly?)ctrls[4].GetValue() ?? new DateOnly(1999, 01, 01),
			    FinishDate = (DateOnly?)ctrls[5].GetValue() ?? new DateOnly(1999, 01, 01),
			    FaultsList = (List<string>?)ctrls[6].GetValue() ?? [],
			    Price = (double?)ctrls[7].GetValue() ?? 0.1
		    },
		    onSuccess: result =>
		    {
			    ViewModel.Add(result);
			    DisplayAlertAsync("Успешное добавление", $"Вы ввели: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
    protected override async Task ShowEditForm(TableData.Order item)
    {
	    var fields = new List<IInputControl>
	    {
		   
	    };
	    var inputView = new InputView<TableData.Order>(
		    "Редактирование заказа", 
		    fields,
		    ctrls => new TableData.Order
		    {
			    OrderID = item.OrderID,
			    ProductID = (int?)ctrls[0].GetValue() ?? 0,
			    EmployeeID = (int?)ctrls[1].GetValue() ?? 0,
			    CompanyID = (int?)ctrls[2].GetValue(),
			    PersonID = (int?)ctrls[3].GetValue(),
			    StartDate = (DateOnly?)ctrls[4].GetValue() ?? new DateOnly(1999, 01, 01),
			    FinishDate = (DateOnly?)ctrls[5].GetValue() ?? new DateOnly(1999, 01, 01),
			    FaultsList = (List<string>?)ctrls[6].GetValue() ?? [],
			    Price = (double?)ctrls[7].GetValue() ?? 0.1
		    },
		    onSuccess: result =>
		    {
			    ViewModel.Update(item, result);
			    DisplayAlertAsync("Успешное изменение", $"Вы ввели: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
    protected override void OnDelete(TableData.Order item) => ViewModel.Delete(item);
}