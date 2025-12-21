using Radiotech.Common;
using Radiotech.Data;
using Radiotech.ViewModels;
namespace Radiotech.Views.ListViews;

public class ProductListView : ListViewBase<TableData.Product>
{
    private static readonly ProductViewModel ViewModel = new();

    public ProductListView() : base(
	    "Изделие",
	    ["ID", "Тип", "Марка", "Страна", "Производитель", "Фото", "Срок службы"],
	    ViewModel.Products,
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
		    new ValidatedEntry("Тип", Validators.LettersSpacesCommasOnly),
		    new ValidatedEntry("Марка", Validators.LettersSpacesCommasOnly),
		    new ValidatedEntry("Страна", Validators.LettersOnly),
		    new ValidatedEntry("Производитель", Validators.LettersSpacesCommasOnly),
		    new ValidatedEntry("Изображение", Validators.RequiredNotNull),
		    new ValidatedDateField("Срок службы", Validators.RequiredNotNull)
	    };
	    var inputView = new InputView<TableData.Product>(
		    "Добавление изделия", 
		    fields,
		    ctrls => new TableData.Product
		    {
			    ProductID = ViewModel.FreeId,
			    Type = (string?)ctrls[0].GetValue() ?? "",
			    Mark = (string?)ctrls[1].GetValue() ?? "",
			    Country = (string?)ctrls[2].GetValue() ?? "",
			    Manufacturer = (string?)ctrls[3].GetValue() ?? "",
			    Photo = (string?)ctrls[4].GetValue() ?? "",
			    ServiceLife = (DateOnly?)ctrls[5].GetValue() ?? new DateOnly(1999, 01, 01)
		    },
		    onSuccess: result =>
		    {
			    ViewModel.Add(result);
			    DisplayAlertAsync("Успешное добавление", $"Вы ввели: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
    protected override async Task ShowEditForm(TableData.Product item)
    {
	    var fields = new List<IInputControl>
	    {
		    new ValidatedEntry("Тип", Validators.LettersSpacesCommasOnly, item.Type),
		    new ValidatedEntry("Марка", Validators.LettersSpacesCommasOnly, item.Mark),
		    new ValidatedEntry("Страна", Validators.LettersOnly, item.Country),
		    new ValidatedEntry("Производитель", Validators.LettersSpacesCommasOnly, item.Manufacturer),
		    new ValidatedEntry("Изображение", Validators.RequiredNotNull, item.Photo),
		    new ValidatedDateField("Срок службы", Validators.RequiredNotNull, 
			    item.ServiceLife.ToDateTime(TimeOnly.MinValue)),
	    };
	    var inputView = new InputView<TableData.Product>(
		    "Редактирование изделия", 
		    fields,
		    ctrls => new TableData.Product
		    {
			    ProductID = item.ProductID,
			    Type = (string?)ctrls[0].GetValue() ?? "",
			    Mark = (string?)ctrls[1].GetValue() ?? "",
			    Country = (string?)ctrls[2].GetValue() ?? "",
			    Manufacturer = (string?)ctrls[3].GetValue() ?? "",
			    Photo = (string?)ctrls[4].GetValue() ?? "",
			    ServiceLife = (DateOnly?)ctrls[5].GetValue() ?? new DateOnly(1999, 01, 01)
		    },
		    onSuccess: result =>
		    {
			    ViewModel.Update(item, result);
			    DisplayAlertAsync("Успешное изменение", $"Вы ввели: {result}", "OK");
		    }
	    );
	    await Navigation.PushModalAsync(inputView);
    }
    protected override void OnDelete(TableData.Product item) => ViewModel.Delete(item);
}