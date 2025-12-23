using Radiotech.Common;
using Radiotech.Data;
using Radiotech.ViewModels;

namespace Radiotech.Views.ListViews;

public class OrderListView : ListViewBase<TableData.Order>
{
    private static readonly OrderViewModel ViewModel = new();

    public OrderListView() : base(
	    "Заказ",
	    ["ID", "Изделие", "Сотрудник", "Заказчик", "Дата начала", "Дата окончания", "Список проблем", "Стоимость"],
	    ViewModel.Orders,
	    CollectionItem)
    {
	    var productDict = ViewModel.Products.ToDictionary(p => p.ProductID, p => $"{p.Type} {p.Mark}");
	    var empDict = ViewModel.Employees.ToDictionary(e => e.EmployeeID, e => $"{e.LastName} {e.FirstName}");
	    var compDict = ViewModel.Companies.ToDictionary(c => c.CompanyID, c => c.Name);
	    var persDict = ViewModel.Persons.ToDictionary(p => p.PersonID, p => $"{p.LastName} {p.FirstName}");

	    var displayOrders = ViewModel.Orders.Select(o => new
	    {
		    o.OrderID,
		    o.StartDate,
		    o.FinishDate,
		    o.Price,
		    ProductDisplay = productDict.GetValueOrDefault(o.ProductID, "[? product]"),
		    EmployeeDisplay = empDict.GetValueOrDefault(o.EmployeeID, "[? emp]"),
		    CustomerDisplay = o.CompanyID.HasValue 
			    ? compDict.GetValueOrDefault(o.CompanyID.Value, "[? comp]") 
			    : o.PersonID.HasValue 
				    ? persDict.GetValueOrDefault(o.PersonID.Value, "[? person]") 
				    : "[?]",
		    FaultsDisplay = string.Join(", ", o.FaultsList)
	    }).ToList();
    }

    private static Grid CollectionItem()
    {
	    var grid = new Grid { /* колонки */ };

	    // ID заказа
	    var idLabel = new Label();
	    idLabel.SetBinding(Label.TextProperty, nameof(TableData.Order.OrderID));
	    grid.Add(idLabel, 0, 0);

	    // Изделие (по ProductID)
	    var productLabel = new Label();
	    productLabel.SetBinding(Label.TextProperty, "ProductDisplay");
	    grid.Add(productLabel, 1, 0);

	    // Сотрудник
	    var empLabel = new Label();
	    empLabel.SetBinding(Label.TextProperty, "EmployeeDisplay");
	    grid.Add(empLabel, 2, 0);

	    // Заказчик
	    var customerLabel = new Label();
	    customerLabel.SetBinding(Label.TextProperty, "CustomerDisplay");
	    grid.Add(customerLabel, 3, 0);

	    // Даты
	    var startLabel = new Label();
	    startLabel.SetBinding(Label.TextProperty, nameof(TableData.Order.StartDate));
	    grid.Add(startLabel, 4, 0);

	    var finishLabel = new Label();
	    finishLabel.SetBinding(Label.TextProperty, nameof(TableData.Order.FinishDate));
	    grid.Add(finishLabel, 5, 0);

	    // Неисправности
	    var faultsLabel = new Label();
	    faultsLabel.SetBinding(Label.TextProperty, "FaultsDisplay");
	    grid.Add(faultsLabel, 6, 0);

	    // Цена
	    var priceLabel = new Label();
	    priceLabel.SetBinding(Label.TextProperty, nameof(TableData.Order.Price));
	    grid.Add(priceLabel, 7, 0);

	    return grid;
    }
    protected override async Task ShowAddForm()
    {
	    await Navigation.PushModalAsync(new OrderInputPage(ViewModel, order =>
	    {
		    ViewModel.Add(order);
	    }));
    }

    protected override async Task ShowEditForm(TableData.Order item)
    {
	    await Navigation.PushModalAsync(new OrderInputPage(ViewModel, updatedOrder =>
	    {
		    ViewModel.Update(item, updatedOrder);
	    }, item));
    }
    protected override void OnDelete(TableData.Order item) => ViewModel.Delete(item);
}