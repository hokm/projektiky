using Newtonsoft.Json;
using Pivovar_Mobile.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Pivovar_Mobile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DevicePage : Page
    {
        private Device Device;
        private ObservableCollection<Temperature> Temperatures;

        private double minTemp;
        private double maxTemp;

        public DevicePage()
        {
            this.InitializeComponent();
            Temperatures = new ObservableCollection<Temperature>();
            ToDatePicker.Date = DateTime.Now;
            FromDatePicker.Date = DateTime.Now - TimeSpan.FromDays(2);
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Device = e.Parameter as Device;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainPage parentFrame = ControlHelper.FindParentControl<MainPage>(this) as MainPage;
            var MainTitle = ControlHelper.FindChildControl<TextBlock>(parentFrame, typeof(TextBlock), "MainTitle") as TextBlock;
            MainTitle.Text = Device.Name;
            minTemp = Device.MinTemp;
            maxTemp = Device.MaxTemp;
            await DownloadTemperaturesInfo();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Device.Update(await DownloadDeviceInfo());
            if (Device.MinTemp != minTemp || Device.MaxTemp != maxTemp)
            {
                string username = ApplicationData.Current.LocalSettings.Values["Username"].ToString();
                minTemp = Device.MinTemp;
                maxTemp = Device.MaxTemp;
                //await NotificationHelper.SendPush("wns", "device:" + Device.Name, "{'minTemp':" + Device.MinTemp + ",'maxTemp':" + Device.MaxTemp + "}"); //ORIGO
                await NotificationHelper.SendPush("wns/raw", "device:" + username, "{'minTemp':" + Device.MinTemp + ",'maxTemp':" + Device.MaxTemp + "}");
            }
            await DownloadTemperaturesInfo();
        }

        private async Task<Device> DownloadDeviceInfo()
        {
            ProgressRing1.IsActive = true;
            try
            {
                var response = await API.Devices.Put(Device);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    ProgressRing1.IsActive = false;
                    return JsonConvert.DeserializeObject<Device>(jsonString);
                }
                else
                {
                    new MessageDialog(response.StatusCode + ": "+ await response.Content.ReadAsStringAsync(), "Failed to download Device Info").ShowAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                new MessageDialog(ex.Message, "Failed to download Device Info").ShowAsync();
            }
            ProgressRing1.IsActive = false;
            return null;
        }

        private async Task DownloadTemperaturesInfo()
        {
            Temperatures.Clear();
            ProgressRing2.IsActive = true;
            try
            {
                var response = await API.Temperatures.Get(DeviceID: Device.ID, from: FromDatePicker.Date.Value.DateTime, to: ToDatePicker.Date.Value.DateTime);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    foreach (var temperature in JsonConvert.DeserializeObject<List<Temperature>>(jsonString))
                    {
                        Temperatures.Add(temperature);
                    }
                }
                else
                {
                    new MessageDialog(response.StatusCode + ": " + await response.Content.ReadAsStringAsync(), "Failed to download Temperatures").ShowAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                new MessageDialog(ex.Message, "Failed to download Temperatures").ShowAsync();
            }
            
            ProgressRing2.IsActive = false;
            return;
        }


        private void MinTempPlus_Click(object sender, RoutedEventArgs e)
        {
            if(Device.MinTemp < Device.MaxTemp)
                Device.MinTemp++;
        }

        private void MinTempMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Device.MinTemp > 0)
                Device.MinTemp--;
        }

        private void MaxTempPlus_Click(object sender, RoutedEventArgs e)
        {
            if (Device.MaxTemp < 80)
                Device.MaxTemp++;
        }

        private void MaxTempMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Device.MaxTemp > Device.MinTemp)
                Device.MaxTemp--;
        }
    }
}
