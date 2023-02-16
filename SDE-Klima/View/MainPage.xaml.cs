using SDE_Klima.Model;
using SDE_Klima.View;
using SDE_Klima.ViewModel;

namespace SDE_Klima;

public partial class MainPage : ContentPage
{
    MainPageViewModel mainPage = new MainPageViewModel();
    SensorsViewModel sensorsViewModel;
	public MainPage()
    {
        BindingContext = mainPage;
        sensorsViewModel = new SensorsViewModel(mainPage, true);
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
	{
        await Navigation.PushModalAsync(new SensorsView(sensorsViewModel),true);
    }
}

