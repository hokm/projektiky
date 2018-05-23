using Pivovar_Mobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace Pivovar_Mobile.API
{
    static class Devices
    {
        static private string ControllerUrl = Config.EndpointUrl + "/Devices";

        static async public Task<HttpResponseMessage> Get()
        {
            using (var httpClient = new HttpClient())
            {
                var settings = ApplicationData.Current.LocalSettings.Values;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)settings["AuthenticationToken"]);
                var response = await httpClient.GetAsync(ControllerUrl);
                return response;
            }
        }

        static async public Task<HttpResponseMessage> Get(int ID)
        {
            using (var httpClient = new HttpClient())
            {
                var settings = ApplicationData.Current.LocalSettings.Values;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)settings["AuthenticationToken"]);
                var response = await httpClient.GetAsync(ControllerUrl + "/" + ID);
                return response;
            }
        }

        static async public Task<HttpResponseMessage> Put(Device device)
        {
            using(var httpClient = new HttpClient())
            {
                var settings = ApplicationData.Current.LocalSettings.Values;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)settings["AuthenticationToken"]);
                var formContent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("Name", device.Name),
                    new KeyValuePair<string, string>("MinTemp", device.MinTemp.ToString()),
                    new KeyValuePair<string, string>("MaxTemp", device.MaxTemp.ToString()),
                });
                var response = await httpClient.PutAsync(ControllerUrl, formContent);
                return response;
            } 
        }
    }
}
