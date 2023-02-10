using SDE_Klima.Model;
using SDE_Klima.View;
using SDE_Klima.ViewModel;

namespace SDE_Klima;

public partial class MainPage : ContentPage
{
    SensorsViewModel sensorsViewModel;
	public MainPage(MainPageViewModel viewModel)
    {
        sensorsViewModel = new SensorsViewModel(viewModel);
        BindingContext = viewModel;
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
	{
        await Navigation.PushModalAsync(new SensorsView(sensorsViewModel),true);
    }
}

