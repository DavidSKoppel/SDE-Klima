using SDE_Klima.ViewModel;

namespace SDE_Klima;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();
		MainPage = new NavigationPage(new MainPage());
    }
}