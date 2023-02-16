using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SDE_Klima.Model
{
    public class TemperatureSensorData
    {
        public string temperature { get; set; }
        public string humidity { get; set; }
        public string name { get; set; }
        public string zone { get; set; }
    }
}
