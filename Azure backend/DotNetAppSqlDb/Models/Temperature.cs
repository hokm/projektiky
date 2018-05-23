using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace DotNetAppSqlDb.Models
{
    public class Temperature
    {
        public int ID { get; private set; }

        [Required]
        public Double Celsius { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        public int IoTDeviceID { get; set; }
        [JsonIgnore]
        public virtual IoTDevice IoTDevice { get; set; }

        /*CONSTRUCTORS*/
        public Temperature(Double celsius)
        {
            this.Celsius = celsius;
            Date = DateTime.Now;
        }

        public Temperature()
        {

        }
    }
}