using SDE_Klima.ViewModel;

namespace SDE_Klima;

public partial class App : Application
{
    MainPageViewModel m = new MainPageViewModel();
    public App()
	{
		InitializeComponent();
		MainPage = new NavigationPage(new MainPage(m));
    }
}