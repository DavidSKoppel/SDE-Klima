using SDE_Klima.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDE_Klima.ViewModel
{
    public class MainPageViewModel : BindableObject
    {
        public TemperatureSensorData sensorData;
        Color color;
        string humidity, temperature, place;

        public Color Colour { get => color; set { if (color == value) return; color = value; OnPropertyChanged(); } }
        public string Humidity { get => humidity; set {if (humidity == value) return; humidity = value; OnPropertyChanged();} }
        public string Temperature { get => temperature; set {if (temperature == value) return; temperature = value; OnPropertyChanged();} }
        public string Place { get => place; set { if (place == value) return; place = value; OnPropertyChanged(); } }

        public MainPageViewModel()
        {
            sensorData = new TemperatureSensorData { humidity = "test", name = "test", temperature = "21", zone = "test" };
            ChangeData(sensorData);
        }
        public MainPageViewModel(TemperatureSensorData sensorData)
        {
            ChangeData(sensorData);
        }

        public void ChangeData(TemperatureSensorData sensorData)
        {
            Place = sensorData.name + ": " + sensorData.zone;
            Temperature = sensorData.temperature;
            Humidity = sensorData.humidity + " Luftfugtighed";
            double temp = Convert.ToDouble(Temperature.Replace("°C", ""));
            if (temp < 20) 
            {
                Colour = Color.FromHex("#FFFFFF");//Cold
            }
            else if(temp >= 20 && temp <= 25)
            {
                Colour = Color.FromHex("#6ea5ff");//Mild
            }
            else if (temp > 25)
            {
                Colour = Color.FromHex("#ff0019");//Hot
            }
        }
    }
}
