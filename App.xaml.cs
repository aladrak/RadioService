using Microsoft.UI.Windowing;

namespace Radiotech
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
        
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new AppShell();
            // window.Window.
            return new Window(window);
        }
    }
}
