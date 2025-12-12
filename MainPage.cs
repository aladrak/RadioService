namespace Radiotech;

public class MainPage : ContentPage
{
	public MainPage()
    {
        // Title = "Main";
        Button toPersonPageBtn = new Button
        {
            Text = "Person",
            HorizontalOptions = LayoutOptions.Start
        };
        toPersonPageBtn.Clicked += ToPersonPage;
 
        Button toEmployeePageBtn = new Button
        {
            Text = "Employee",
            HorizontalOptions = LayoutOptions.Start
        };
        toEmployeePageBtn.Clicked += ToEmployeePage;
        Content = new VerticalStackLayout() { Padding = 30, Children = { toPersonPageBtn, toEmployeePageBtn } };
    }
    private async void ToPersonPage(object? sender, EventArgs e) => 
        await Shell.Current.GoToAsync("PersonListView");
    private async void ToEmployeePage(object? sender, EventArgs e) =>
        await Shell.Current.GoToAsync("EmployeeListView");
    
}