using Radiotech.Views;
using Radiotech.Views.ListViews;

namespace Radiotech;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("PersonListView", typeof(PersonListView));
        Routing.RegisterRoute("InputView", typeof(InputView));
    }
}