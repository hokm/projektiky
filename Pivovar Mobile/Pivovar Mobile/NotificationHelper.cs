using Pivovar_Mobile.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace Pivovar_Mobile
{
    class NotificationHelper
    {
        /// <summary>
        /// Sends simple push notification.
        /// </summary>
        /// <param name="pns">Defines the type of notification (e.g. wns, gcm, apns).</param>
        /// <param name="tag">Receiver tag name (e.g. bob).</param>
        /// <param name="message">Message text (e.g. Hello bob!). </param>
        static public async Task SendPush(string pns, string tag, string message)
        {
            var POST_URL = Config.EndpointUrl + "/notifications?pns=" +  pns + "&to_tag=" + tag;

            using (var httpClient = new HttpClient())
            {
                var settings = ApplicationData.Current.LocalSettings.Values;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)settings["AuthenticationToken"]);

                try
                {
                    await httpClient.PostAsync(POST_URL, new StringContent("\"" + message + "\"",
                        System.Text.Encoding.UTF8, "application/json"));
                }
                catch (Exception ex)
                {
                    MessageDialog alert = new MessageDialog(ex.Message, "Failed to send " + pns + " message");
                    alert.ShowAsync();
                }
            }
        }
    }
}
