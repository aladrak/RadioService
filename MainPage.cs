using Microsoft.Maui.Controls;

namespace Radiotech;

public class MainPage : ContentPage
{
    public MainPage()
    {
        var header = new Label
        {
            Text = "Система учёта ремонта радиоаппаратуры",
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 5, 0, 10),
            TextColor = Colors.White,
        };

        // Главная кнопка
        var orderButton = new Button
        {
            Text = "➕ Заказы",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            BackgroundColor = Colors.MediumVioletRed,
            TextColor = Colors.White,
            CornerRadius = 24,
            Padding = new Thickness(20, 15),
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 280,
            HeightRequest = 60
        };
        orderButton.Clicked += (_, _) => Shell.Current.GoToAsync("OrderListView");

        var separator = new BoxView
        {
            HeightRequest = 1,
            Color = Colors.LightGray,
            Margin = new Thickness(0, 30),
            HorizontalOptions = LayoutOptions.Fill
        };

        // Мелкие кнопки
        var otherButtons = new VerticalStackLayout
        {
            Spacing = 12,
            Margin = new Thickness(40, 5)
        };

        var entities = new (string Name, string Route)[]
        {
            ("Сотрудники", "EmployeeListView"),
            ("Изделия", "ProductListView"),
            ("Компании", "CompanyListView"),
            ("Частные лица", "PersonListView")
        };

        foreach (var (name, route) in entities)
        {
            var btn = new Button
            {
                Text = name,
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.SteelBlue,
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 5)
            };
            btn.Clicked += (_, _) => Shell.Current.GoToAsync(route);
            otherButtons.Children.Add(btn);
        }

        var mainLayout = new VerticalStackLayout
        {
            Padding = new Thickness(20, 5),
            Spacing = 20,
            Children =
            {
                header,
                orderButton,
                separator,
                otherButtons
            }
        };
        // BackgroundColor = ;

        Content = mainLayout;
    }
}