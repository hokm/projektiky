using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivovar_Mobile.Model
{
    public class Temperature
    {
        public int ID { get; set; }
        public double Celsius { get; set; }
        public DateTime Date { get; set; }
        public int IoTDeviceID { get; set; }
    }
}
