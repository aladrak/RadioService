using System.Collections.ObjectModel;
using Radiotech.Data;
using Radiotech.Ui;
using Radiotech.ViewModels;

namespace Radiotech.Views.ListViews;

internal class OrderDisplayItem
{
	public TableData.Order Source { get; }
	public string ProductName { get; }
	public string EmployeeName { get; }
	public string CustomerName { get; }
	public string FaultsDisplay { get; }

	public OrderDisplayItem(TableData.Order order, 
		IReadOnlyDictionary<int, string> productDict,
		IReadOnlyDictionary<int, string> empDict,
		IReadOnlyDictionary<int, string> compDict,
		IReadOnlyDictionary<int, string> persDict)
	{
		Source = order;
		ProductName = productDict.GetValueOrDefault(order.ProductID, $"[? product {order.ProductID}]");
		EmployeeName = empDict.GetValueOrDefault(order.EmployeeID, $"[? emp {order.EmployeeID}]");
		CustomerName = order.CompanyID.HasValue
			? compDict.GetValueOrDefault(order.CompanyID.Value, $"[? company {order.CompanyID}]")
			: order.PersonID.HasValue
				? persDict.GetValueOrDefault(order.PersonID.Value, $"[? person {order.PersonID}]")
				: "[Заказчик не указан]";
		FaultsDisplay = string.Join(", ", order.FaultsList);
	}
}

public class OrderListView : ContentPage
{
    private readonly CollectionView _collectionView;
    private readonly OrderViewModel _viewModel;
    private readonly ObservableCollection<OrderDisplayItem> _displayItems;

    // Справочники для отображения
    private readonly IReadOnlyDictionary<int, string> _productDict;
    private readonly IReadOnlyDictionary<int, string> _empDict;
    private readonly IReadOnlyDictionary<int, string> _compDict;
    private readonly IReadOnlyDictionary<int, string> _persDict;

    public OrderListView()
    {
        _viewModel = new OrderViewModel();

        _productDict = _viewModel.Products.ToDictionary(p => p.ProductID, p => $"{p.Type} {p.Mark}");
        _empDict = _viewModel.Employees.ToDictionary(e => e.EmployeeID, e => $"{e.LastName} {e.FirstName}");
        _compDict = _viewModel.Companies.ToDictionary(c => c.CompanyID, c => c.Name);
        _persDict = _viewModel.Persons.ToDictionary(p => p.PersonID, p => $"{p.LastName} {p.FirstName}");

        // Создание отображаемых элементов
        _displayItems = new ObservableCollection<OrderDisplayItem>(
            _viewModel.Orders.Select(o => new OrderDisplayItem(o, _productDict, _empDict, _compDict, _persDict))
        );

        _collectionView = new CollectionView
        {
            SelectionMode = SelectionMode.Single,
            ItemsSource = _displayItems,
            ItemTemplate = new DataTemplate(CreateItemTemplate)
        };
        _collectionView.SelectionChanged += OnSelectionChanged;

        // Кнопка "Добавить"
        var addButton = new Button { Text = "Добавить заказ" };
        addButton.Clicked += async (_, _) => await ShowAddForm();

        // UI
        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing = 12,
                Padding = 4,
                Children =
                {
                    UiTemplates.HeaderGrid([
                        "ID", "Изделие", "Сотрудник", "Заказчик", 
                        "Начало", "Окончание", "Неисправности", "Стоимость"
                    ]),
                    _collectionView,
                    addButton
                }
            }
        };
    }

    private static Grid CreateItemTemplate()
    {
        var grid = new Grid
        {
            Padding = 10,
            ColumnSpacing = 10,
            ColumnDefinitions =
            [
                new(20), // ID
                new(GridLength.Star), // Изделие
                new(GridLength.Star), // Сотрудник
                new(GridLength.Star), // Заказчик
                new(GridLength.Star), // Начало
                new(GridLength.Star), // Окончание
                new(GridLength.Star), // Неисправности
                new(GridLength.Star)
            ]
        };

        // ID заказа (из Source)
        var idLabel = new Label();
        idLabel.SetBinding(Label.TextProperty, nameof(OrderDisplayItem.Source) + "." + nameof(TableData.Order.OrderID));
        grid.Add(idLabel, 0, 0);

        // Изделие
        var productLabel = new Label();
        productLabel.SetBinding(Label.TextProperty, nameof(OrderDisplayItem.ProductName));
        grid.Add(productLabel, 1, 0);

        // Сотрудник
        var empLabel = new Label();
        empLabel.SetBinding(Label.TextProperty, nameof(OrderDisplayItem.EmployeeName));
        grid.Add(empLabel, 2, 0);

        // Заказчик
        var custLabel = new Label();
        custLabel.SetBinding(Label.TextProperty, nameof(OrderDisplayItem.CustomerName));
        grid.Add(custLabel, 3, 0);

        // Даты
        var startLabel = new Label();
        startLabel.SetBinding(Label.TextProperty, nameof(OrderDisplayItem.Source) + "." + nameof(TableData.Order.StartDate));
        grid.Add(startLabel, 4, 0);

        var finishLabel = new Label();
        finishLabel.SetBinding(Label.TextProperty, nameof(OrderDisplayItem.Source) + "." + nameof(TableData.Order.FinishDate));
        grid.Add(finishLabel, 5, 0);

        // Неисправности
        var faultsLabel = new Label();
        faultsLabel.SetBinding(Label.TextProperty, nameof(OrderDisplayItem.FaultsDisplay));
        grid.Add(faultsLabel, 6, 0);

        // Стоимость
        var priceLabel = new Label();
        priceLabel.SetBinding(Label.TextProperty, nameof(OrderDisplayItem.Source) + "." + nameof(TableData.Order.Price));
        grid.Add(priceLabel, 7, 0);

        return grid;
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not OrderDisplayItem selectedItem)
        {
            _collectionView.SelectedItem = null;
            return;
        }
        _collectionView.SelectedItem = null;

        string action = await Shell.Current.DisplayActionSheetAsync(
            $"Заказ №{selectedItem.Source.OrderID}",
            "Отмена", null, "Изменить", "Удалить");

        if (action == "Изменить")
        {
            await ShowEditForm(selectedItem.Source);
        }
        else if (action == "Удалить")
        {
            bool confirm = await DisplayAlertAsync("Подтверждение", "Удалить заказ?", "Да", "Нет");
            if (confirm)
            {
                _viewModel.Delete(selectedItem.Source);
                await ReloadDisplayItems();
            }
        }
    }

    private async Task ShowAddForm()
    {
        await Navigation.PushModalAsync(new OrderInputPage(_viewModel, async order =>
        {
            _viewModel.Add(order);
            await ReloadDisplayItems();
        }));
    }

    private async Task ShowEditForm(TableData.Order order)
    {
        await Navigation.PushModalAsync(new OrderInputPage(_viewModel, async updatedOrder =>
        {
            _viewModel.Update(order, updatedOrder);
            await ReloadDisplayItems();
        }, order));
    }

    private async Task ReloadDisplayItems()
    {
        var newItems = _viewModel.Orders
            .Select(o => new OrderDisplayItem(o, _productDict, _empDict, _compDict, _persDict))
            .ToList();

        _displayItems.Clear();
        foreach (var item in newItems)
            _displayItems.Add(item);
    }
}