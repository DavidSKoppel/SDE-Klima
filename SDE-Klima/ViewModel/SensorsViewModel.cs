using SDE_Klima.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SDE_Klima.ViewModel
{
    public class SensorsViewModel : BindableObject
    {
        public INavigation Navigation;
        MainPageViewModel mainPageViewModel;
        private ObservableCollection<TemperatureSensorData> _temperatures;
        public ObservableCollection<TemperatureSensorData> temperatures 
        { 
            get 
            { 
                return _temperatures; 
            } 
            set 
            { 
                _temperatures = value; 
                OnPropertyChanged(); 
            } 
        }
        public Command GetTemperaturesCommand { get; }
        public SensorsViewModel(MainPageViewModel main)
        {
            mainPageViewModel = main;
            GetTemperaturesCommand = new Command(GetTemperaturesAsync);
            temperatures = new ObservableCollection<TemperatureSensorData>();
            GetTemperaturesAsync();
        }
        public async void ShowTemperatureAsync(TemperatureSensorData sensorData)
        {
            mainPageViewModel.ChangeData(sensorData);
            await Navigation.PopModalAsync();
        }
        public async void GetTemperaturesAsync()
        {
            using (var _client = new HttpClient())
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("what", "temperatures-json")
            });

                HttpResponseMessage response = await _client.PostAsync("https://itd-skp.sde.dk/api/find.php", formContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    var jsonList = JsonSerializer.Deserialize<List<TemperatureSensorData>>(content);
                    foreach (var record in jsonList)
                    {
                        temperatures.Add(new TemperatureSensorData
                        {
                            temperature = record.temperature + "°C",
                            humidity = record.humidity + "%",
                            zone = "Zone-" + record.zone,
                            name = record.name
                        });
                    }
                }
            }
        }
    }
}
