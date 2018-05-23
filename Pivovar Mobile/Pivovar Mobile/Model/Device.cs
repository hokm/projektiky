using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pivovar_Mobile.Model
{
    class Device : INotifyPropertyChanged
    {
        private int _id;
        public int ID { get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        private string _name;
        public string Name { get => _name;
            set {
                if(_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
                
            }
        }
        private double _minTemp;
        public double MinTemp { get => _minTemp;
            set
            {
                if (_minTemp != value)
                {
                    _minTemp = value;
                    OnPropertyChanged("MinTemp");
                }
            }
        }
        private double _maxTemp;
        public double MaxTemp { get => _maxTemp;
            set
            {
                if (_maxTemp != value)
                {
                    _maxTemp = value;
                    OnPropertyChanged("MaxTemp");
                }
            }
        }
        private Temperature _actualTemperature;
        public Temperature ActualTemperature { get => _actualTemperature;
            set
            {
                _actualTemperature = value;
                OnPropertyChanged("ActualTemperature");
                OnPropertyChanged("Status");
            }
        }
        //public List<Temperature> Temperatures { get; set; }
        public int UserID { get; set; }
        public string Status { get {
                if(ActualTemperature == null)
                {
                    return "\uEA39";
                }
                else if(DateTime.Now - TimeSpan.FromHours(1) > ActualTemperature.Date)
                {
                    return "\uE823";
                }
                else if (ActualTemperature.Celsius > MaxTemp)
                {
                    return "\uEC4A";
                }
                else if (ActualTemperature.Celsius < MinTemp)
                {
                    return "\uEC48";
                }
                else
                {
                    return "";
                }
            }
        }

        public Device()
        {
            //Temperatures = new List<Temperature>();
        }

        public void Update(Device device)
        {
            if(device != null)
            {
                ID = device.ID;
                MinTemp = device.MinTemp;
                MaxTemp = device.MaxTemp;
                Name = device.Name;
                ActualTemperature = device.ActualTemperature;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
