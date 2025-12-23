using Microsoft.Maui.Controls;
using Radiotech.Common;
using Radiotech.Data;
using Radiotech.ViewModels;

namespace Radiotech.Views;

public class OrderInputPage : ContentPage
{
    private readonly OrderViewModel _viewModel;
    private readonly Action<TableData.Order>? _onResult;
    private readonly TableData.Order? _existingOrder;

    private readonly ValidatedPicker _productPicker;
    private readonly ValidatedPicker _employeePicker;
    private readonly ValidatedPicker _customerTypePicker;
    private readonly ValidatedPicker _companyPicker;
    private readonly ValidatedPicker _personPicker;
    private readonly ValidatedDateField _startDateField;
    private readonly ValidatedDateField _finishDateField;
    private readonly ValidatedEntry _faultsEntry;
    private readonly ValidatedEntry _priceEntry;

    private readonly Label _companyPickerLabel;
    private readonly Label _personPickerLabel;

    public OrderInputPage(OrderViewModel viewModel, Action<TableData.Order> onResult, TableData.Order? existing = null)
    {
        _viewModel = viewModel;
        _onResult = onResult;
        _existingOrder = existing;

        // Picker
        var products = viewModel.Products.Select(p => $"{p.Type} {p.Mark} (ID: {p.ProductID})").ToArray();
        var employees = viewModel.Employees
            .Select(e => $"{viewModel.Specialties.FirstOrDefault(a => a.SpecialtyID == e.SpecialtyID)!.Name}, {e.LastName} {e.FirstName} (ID: {e.EmployeeID})").ToArray();
        var companies = viewModel.Companies.Select(c => $"{c.Name} (ID: {c.CompanyID})").ToArray();
        var persons = viewModel.Persons.Select(p => $"{p.LastName} {p.FirstName} (ID: {p.PersonID})").ToArray();

        var layout = new VerticalStackLayout { Padding = 20, Spacing = 15 };

        layout.Add(new Label { Text = existing == null ? "Новый заказ" : "Редактирование заказа", FontSize = 20, HorizontalOptions = LayoutOptions.Center });

        // Изделие
        _productPicker = new ValidatedPicker("Изделие", products, Validators.RequiredNotNull);
        layout.Add(_productPicker.Control);
        layout.Add(_productPicker.ErrorLabel);

        // Сотрудник
        _employeePicker = new ValidatedPicker("Сотрудник", employees, Validators.RequiredNotNull);
        layout.Add(_employeePicker.Control);
        layout.Add(_employeePicker.ErrorLabel);

        // Тип заказчика
        _customerTypePicker = new ValidatedPicker("Тип заказчика", ["Компания", "Физическое лицо"], Validators.RequiredNotNull);
        _customerTypePicker.ValueChanged += OnCustomerTypeChanged;
        layout.Add(_customerTypePicker.Control);
        layout.Add(_customerTypePicker.ErrorLabel);

        // Компания (скрыта)
        _companyPickerLabel = new Label { Text = "Компания", FontAttributes = FontAttributes.Bold, IsVisible = false };
        _companyPicker = new ValidatedPicker("Выберите компанию", companies, v => (string?)_customerTypePicker.GetValue() == "Компания" ? Validators.RequiredNotNull(v) : (true, ""));
        _companyPicker.Control.IsVisible = false;
        layout.Add(_companyPickerLabel);
        layout.Add(_companyPicker.Control);
        layout.Add(_companyPicker.ErrorLabel);

        // Физ. лицо (скрыто)
        _personPickerLabel = new Label { Text = "Физическое лицо", FontAttributes = FontAttributes.Bold, IsVisible = false };
        _personPicker = new ValidatedPicker("Выберите лицо", persons, v => (string?)_customerTypePicker.GetValue() == "Физическое лицо" ? Validators.RequiredNotNull(v) : (true, ""));
        _personPicker.Control.IsVisible = false;
        layout.Add(_personPickerLabel);
        layout.Add(_personPicker.Control);
        layout.Add(_personPicker.ErrorLabel);

        // Даты
        _startDateField = new ValidatedDateField("Дата начала", Validators.RequiredBeforeToday);
        _finishDateField = new ValidatedDateField("Дата окончания", Validators.RequiredNotNull);
        layout.Add(new Label{Text = "Дата начала", TextColor = Colors.LightGrey});
        layout.Add(_startDateField.Control);
        layout.Add(_startDateField.ErrorLabel);
        layout.Add(new Label{Text = "Дата окончания", TextColor = Colors.LightGrey});
        layout.Add(_finishDateField.Control);
        layout.Add(_finishDateField.ErrorLabel);

        // Список неисправностей — пока как одна строка, через запятую
        _faultsEntry = new ValidatedEntry("Неисправности (через запятую)", Validators.LettersSpacesCommasOnly, initialValue: "");
        layout.Add(_faultsEntry.Control);
        layout.Add(_faultsEntry.ErrorLabel);

        // Стоимость
        _priceEntry = new ValidatedEntry("Стоимость", Validators.RequiredPositiveDouble, initialValue: "");
        layout.Add(_priceEntry.Control);
        layout.Add(_priceEntry.ErrorLabel);

        // Кнопки
        var saveButton = new Button { Text = "Сохранить", BackgroundColor = Colors.Green };
        saveButton.Clicked += OnSaveClicked;
        var cancelButton = new Button { Text = "Отмена" };
        cancelButton.Clicked += async (_, _) => await Shell.Current.Navigation.PopModalAsync();

        layout.Add(new HorizontalStackLayout { Children = { saveButton, cancelButton } });

        Content = new ScrollView{ Content = layout };

        // При редактировании
        if (existing != null)
        {
            // Изделие
            var pIndex = viewModel.Products.FindIndex(p => p.ProductID == existing.ProductID);
            if (pIndex >= 0) _productPicker.Control.SelectedIndex = pIndex;

            // Сотрудник
            var eIndex = viewModel.Employees.FindIndex(e => e.EmployeeID == existing.EmployeeID);
            if (eIndex >= 0) _employeePicker.Control.SelectedIndex = eIndex;

            // Заказчик
            if (existing.CompanyID.HasValue)
            {
                _customerTypePicker.Control.SelectedIndex = 0; // Компания
                var cIndex = viewModel.Companies.FindIndex(c => c.CompanyID == existing.CompanyID.Value);
                if (cIndex >= 0) _companyPicker.Control.SelectedIndex = cIndex;
                ShowCompanyPicker();
            }
            else if (existing.PersonID.HasValue)
            {
                _customerTypePicker.Control.SelectedIndex = 1; // Физ. лицо
                var pIndex2 = viewModel.Persons.FindIndex(p => p.PersonID == existing.PersonID.Value);
                if (pIndex2 >= 0) _personPicker.Control.SelectedIndex = pIndex2;
                ShowPersonPicker();
            }

            _startDateField.Control.Date = existing.StartDate.ToDateTime(TimeOnly.MinValue);
            _finishDateField.Control.Date = existing.FinishDate.ToDateTime(TimeOnly.MinValue);

            _faultsEntry.Control.Text = string.Join(", ", existing.FaultsList);

            _priceEntry.Control.Text = existing.Price.ToString("F2");
        }
    }

    private void OnCustomerTypeChanged(string? value)
    {
        switch (value)
        {
            case "Компания":
                ShowCompanyPicker();
                break;
            case "Физическое лицо":
                ShowPersonPicker();
                break;
        }
    }

    private void ShowCompanyPicker()
    {
        _companyPickerLabel.IsVisible = true;
        _companyPicker.Control.IsVisible = true;
        _personPickerLabel.IsVisible = false;
        _personPicker.Control.IsVisible = false;
    }

    private void ShowPersonPicker()
    {
        _companyPickerLabel.IsVisible = false;
        _companyPicker.Control.IsVisible = false;
        _personPickerLabel.IsVisible = true;
        _personPicker.Control.IsVisible = true;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if ((string?)_customerTypePicker.GetValue() == "Компания")
        {
            if (!_companyPicker.IsValid) return;
        }
        else if ((string?)_customerTypePicker.GetValue() == "Физическое лицо")
        {
            if (!_personPicker.IsValid) return;
        }

        if (!_productPicker.IsValid || !_employeePicker.IsValid ||
            !_startDateField.IsValid || !_finishDateField.IsValid ||
            !_faultsEntry.IsValid || !_priceEntry.IsValid)
        {
            await DisplayAlertAsync("Ошибка", "Проверьте введённые данные", "OK");
            return;
        }

        if ((DateOnly?)_startDateField.GetValue() > (DateOnly?)_finishDateField.GetValue())
        {
            await DisplayAlertAsync("Ошибка", "Дата начала больше даты окончания", "OK");
            return;
        }

        try
        {
            var productId = _viewModel.Products[_productPicker.Control.SelectedIndex].ProductID;
            var employeeId = _viewModel.Employees[_employeePicker.Control.SelectedIndex].EmployeeID;

            int? companyId = null;
            int? personId = null;
            if ((string?)_customerTypePicker.GetValue() == "Компания")
                companyId = _viewModel.Companies[_companyPicker.Control.SelectedIndex].CompanyID;
            else if ((string?)_customerTypePicker.GetValue() == "Физическое лицо")
                personId = _viewModel.Persons[_personPicker.Control.SelectedIndex].PersonID;

            var startDate = DateOnly.FromDateTime(_startDateField.Control.Date ?? new DateTime());
            var finishDate = DateOnly.FromDateTime(_finishDateField.Control.Date ?? new DateTime());
            var faults = _faultsEntry.Control.Text
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();
            var price = double.Parse(_priceEntry.Control.Text);

            var order = new TableData.Order
            {
                OrderID = _existingOrder?.OrderID ?? _viewModel.FreeId,
                ProductID = productId,
                EmployeeID = employeeId,
                CompanyID = companyId,
                PersonID = personId,
                StartDate = startDate,
                FinishDate = finishDate,
                FaultsList = faults,
                Price = price
            };

            _onResult?.Invoke(order);
            await Shell.Current.Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
    }
}