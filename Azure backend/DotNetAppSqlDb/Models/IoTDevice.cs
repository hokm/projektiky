using Foolproof;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DotNetAppSqlDb.Models
{
    public class IoTDevice
    {
        public int ID { get; private set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double MinTemp { get; set; }

        [Required]
        [GreaterThan("MinTemp", ErrorMessage ="MaxTemp must be greater than MinTemp")]
        public double MaxTemp { get; set; }

        public int UserID { get; set; }

        [JsonIgnore]
        public ICollection<Temperature> Temperatures { get; set; }

        public virtual Temperature ActualTemperature
        {
            get
            {
                if (Temperatures != null && Temperatures.Count > 0)
                    return Temperatures.Last();
                else
                    return null;
            }
        }

        [JsonIgnore]
        public virtual User User { get; set; }

        public IoTDevice()
        {

        }
    }
}