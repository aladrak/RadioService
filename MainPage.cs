namespace Radiotech;

public class MainPage : ContentPage
{
	public MainPage()
    {
        // Title = "Main";
        var layout = new VerticalStackLayout() { Padding = 30 };
        
        var personBtn = new Button { Text = "Person", HorizontalOptions = LayoutOptions.Start };
        personBtn.Clicked += (_, _) => Shell.Current.GoToAsync("PersonListView");
        layout.Children.Add(personBtn);
 
        var employeeBtn = new Button { Text = "Employee", HorizontalOptions = LayoutOptions.Start };
        employeeBtn.Clicked += (_, _) => Shell.Current.GoToAsync("EmployeeListView");
        layout.Children.Add(employeeBtn);
        
        var productBtn = new Button { Text = "Product", HorizontalOptions = LayoutOptions.Start };
        productBtn.Clicked += (_, _) => Shell.Current.GoToAsync("ProductListView");
        layout.Children.Add(productBtn);
        
        var companyBtn = new Button { Text = "Company", HorizontalOptions = LayoutOptions.Start };
        companyBtn.Clicked += (_, _) => Shell.Current.GoToAsync("CompanyListView");
        layout.Children.Add(companyBtn);
        
        var orderBtn = new Button { Text = "Order", HorizontalOptions = LayoutOptions.Start };
        orderBtn.Clicked += (_, _) => Shell.Current.GoToAsync("OrderListView");
        layout.Children.Add(orderBtn);
        
        Content = layout;
    }
}