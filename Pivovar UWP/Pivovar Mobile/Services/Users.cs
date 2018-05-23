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
    static class Users
    {
        static private string ControllerUrl = Config.EndpointUrl + "/Users";

        static async public Task<HttpResponseMessage> Post(string email, string password, string firstName, string lastName)
        {
            using(var httpClient = new HttpClient())
            {
                var formContent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("firstName", "firstName"),
                    new KeyValuePair<string, string>("lastName", "lastName")
                });
                var response = await httpClient.PostAsync(ControllerUrl, formContent);
                return response;
            }         
        }
    }
}
