using SDE_Klima.Model;
using System.Net;
using System.Text.Json;

namespace SDE_Klima.ViewModel
{
    public class MainPageViewModel : BindableObject
    {
        public Command RefreshCommand { get; }
        Color color;
        string temperature, humidity, name, zone, time, date;
        bool isRefreshing = false;

        public bool IsRefreshing { get => isRefreshing; set { if (isRefreshing == value) return; isRefreshing = value; OnPropertyChanged(); } }
        public Color Colour { get => color; set { if (color == value) return; color = value; OnPropertyChanged(); } }
        public string Temperature { get => temperature; set {if (temperature == value) return; temperature = value; OnPropertyChanged();} }
        public string Humidity { get => humidity; set {if (humidity == value) return; humidity = value; OnPropertyChanged();} }
        public string Name { get => name; set {if (name == value) return; name = value; OnPropertyChanged();} }
        public string Zone { get => zone; set {if (zone == value) return; zone = value; OnPropertyChanged();} }
        public string Time { get => time; set {if (time == value) return; time = value; OnPropertyChanged();} }
        public string Date { get => date; set {if (date == value) return; date = value; OnPropertyChanged();} }

        public MainPageViewModel()
        {
            RefreshCommand = new Command(GetTemperatureAsync);
            TemperatureSensorData sensorData = new TemperatureSensorData
            {
                temperature = Preferences.Default.Get("temperature", "?°C"), 
                humidity = Preferences.Default.Get("humidity", "?%"),
                name = Preferences.Default.Get("name", "mu?"),
                zone = Preferences.Default.Get("zone", "zone-?"),
                updated_time = Preferences.Default.Get("time", "??:??:??"),
                updated_date = Preferences.Default.Get("date", "??:??:????")

            };
            ChangeData(sensorData);
        }
        public MainPageViewModel(TemperatureSensorData sensorData)
        {
            ChangeData(sensorData);
        }

        public async void GetTemperatureAsync()
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

                        var jsonList = JsonSerializer.Deserialize<List<TemperatureSensorData>>(content);
                        foreach (var record in jsonList)
                        {
                            if (record.name == name && record.zone == zone) 
                            {
                                ChangeData(record);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occured please try again later", "OK");
            }
            IsRefreshing = false;
        }

        public void ChangeData(TemperatureSensorData sensorData)
        {
            Preferences.Default.Set("temperature", sensorData.temperature);
            Preferences.Default.Set("humidity", sensorData.humidity);
            Preferences.Default.Set("name", sensorData.name);
            Preferences.Default.Set("zone", sensorData.zone);
            Preferences.Default.Set("time", sensorData.updated_time);
            Preferences.Default.Set("date", sensorData.updated_date);

            Temperature = sensorData.temperature;
            Humidity = sensorData.humidity;
            Name = sensorData.name;
            Zone = sensorData.zone;
            Time = sensorData.updated_time;
            Date = sensorData.updated_date;
            try
            {
                float temp = float.Parse(Temperature.Replace("°C", ""));
                if (temp < 20)
                {
    #pragma warning disable CS0618 // Type or member is obsolete
                    Colour = Color.FromHex("#FFFFFF");//Cold
    #pragma warning restore CS0618 // Type or member is obsolete
                }
                else if (temp >= 20 && temp <= 25)
                {
    #pragma warning disable CS0618 // Type or member is obsolete
                    Colour = Color.FromHex("#6ea5ff");//Mild
    #pragma warning restore CS0618 // Type or member is obsolete
                }
                else if (temp > 25)
                {
    #pragma warning disable CS0618 // Type or member is obsolete
                    Colour = Color.FromHex("#ff0019");//Hot
    #pragma warning restore CS0618 // Type or member is obsolete
                }
            }
            catch
            {
#pragma warning disable CS0618 // Type or member is obsolete
                Colour = Color.FromHex("#000000");//None
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
    }
}
