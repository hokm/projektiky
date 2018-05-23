using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Pivovar_Mobile.API
{
    static class Temperatures
    {
        static private string ControllerUrl = Config.EndpointUrl + "/Temperatures";

        static async public Task<HttpResponseMessage> Get(int DeviceID, DateTime? from = null, DateTime? to = null)
        {
            if (!from.HasValue)
                from = DateTime.MinValue;
            if (!to.HasValue)
                to = DateTime.MaxValue;
            using (var httpClient = new HttpClient())
            {
                string query = "?DeviceID="+ DeviceID + "&from=" + ((DateTime)from).ToString("yyyy-MM-ddTHH:mm:ss.fff") + "&to=" + ((DateTime)to).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                var settings = ApplicationData.Current.LocalSettings.Values;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)settings["AuthenticationToken"]);
                var response = await httpClient.GetAsync(ControllerUrl + query);
                return response;
            }
        }
    }
}
