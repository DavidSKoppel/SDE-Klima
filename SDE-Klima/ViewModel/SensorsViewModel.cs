using SDE_Klima.Model;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SDE_Klima.ViewModel
{
    public class SensorsViewModel : BindableObject
    {
        public INavigation Navigation;
        MainPageViewModel mainPageViewModel;
        public Command RefreshCommand { get; }

        bool isRefreshing = false;
        public bool IsRefreshing { get => isRefreshing; set { if (isRefreshing == value) return; isRefreshing = value; OnPropertyChanged(); } }

        private ObservableCollection<TemperatureSensorData> temperatures;
        public ObservableCollection<TemperatureSensorData> Temperatures 
        { 
            get 
            { 
                return temperatures; 
            } 
            set 
            { 
                temperatures = value; 
                OnPropertyChanged(); 
            } 
        }
        public SensorsViewModel(MainPageViewModel main, bool refresh)
        {
            IsRefreshing = refresh;
            temperatures = new ObservableCollection<TemperatureSensorData>();
            RefreshCommand = new Command(GetTemperaturesAsync);
            mainPageViewModel = main;
        }

        ~SensorsViewModel()
        {
        }

        public async void ShowTemperatureAsync(TemperatureSensorData sensorData)
        {
            mainPageViewModel.ChangeData(sensorData);
            await Navigation.PopModalAsync();
        }
        public async void GetTemperaturesAsync()
        {
            try
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
                        if (content != "{\"Error\":503}") 
                        {
                            var jsonList = JsonSerializer.Deserialize<List<TemperatureSensorData>>(content);
                            temperatures.Clear();
                            foreach (var record in jsonList)
                            {
                                temperatures.Add(new TemperatureSensorData
                                {
                                    temperature = record.temperature,
                                    humidity = record.humidity,
                                    zone = record.zone,
                                    name = record.name,
                                    updated_time = record.updated_time,
                                    updated_date = record.updated_date
                                });
                            }
                        } 
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Servers Down", "The servers are experiencing difficulties at the moment, please try again later", "OK");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Connection Error", "Failed to connect to services, please try again later", "OK");
                    }
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occured please try again later", "OK");
            }
            IsRefreshing = false;
        }
    }
}
