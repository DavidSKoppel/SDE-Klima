using Microsoft.Maui.ApplicationModel.Communication;
using SDE_Klima.Model;
using SDE_Klima.ViewModel;
using System.Text.Json;

namespace SDE_Klima.View;

public partial class SensorsView : ContentPage
{
    SensorsViewModel viewModel;
    public SensorsView(SensorsViewModel sensorsViewModel)
    {
        sensorsViewModel.Navigation = Navigation;
        viewModel = sensorsViewModel;
        BindingContext = sensorsViewModel;
        InitializeComponent();
    }

    private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        TemperatureSensorData data = (TemperatureSensorData)e.SelectedItem;
        viewModel.ShowTemperatureAsync(data);
    }
}