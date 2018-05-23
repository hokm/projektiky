using Newtonsoft.Json;
using Pivovar_Mobile.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwivovalink/?LinkId=234238

namespace Pivovar_Mobile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DevicesPage : Page
    {
        private ObservableCollection<Device> _devices;
        internal ObservableCollection<Device> Devices { get => _devices; set => _devices = value; }

        public DevicesPage()
        {
            this.InitializeComponent();
            Devices = new ObservableCollection<Device>();
        }

        private async void GridView_Loaded(object sender, RoutedEventArgs e)
        {
            await DownloadDevices();
        }

        private async Task DownloadDevices()
        {
            Devices.Clear();
            ProgressRing1.IsActive = true;
            try
            {
                var response = await API.Devices.Get();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    foreach (var device in JsonConvert.DeserializeObject<List<Device>>(jsonString))
                    {
                        Devices.Add(device);
                    }
                }
                else
                {
                    new MessageDialog(response.StatusCode + ": " + await response.Content.ReadAsStringAsync(), "Failed to download Devices").ShowAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                new MessageDialog(ex.Message, "Failed to download Devices").ShowAsync();
            }
            ProgressRing1.IsActive = false;
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await DownloadDevices();
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Device device = e.AddedItems[0] as Device;
            Frame.Navigate(typeof(DevicePage), device);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainPage parentFrame = ControlHelper.FindParentControl<MainPage>(this) as MainPage;
            var MainTitle = ControlHelper.FindChildControl<TextBlock>(parentFrame, typeof(TextBlock), "MainTitle") as TextBlock;
            MainTitle.Text = "My Devices";
            int x = 5;
        }
    }
}
