using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
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
    public sealed partial class LoginPage : Page
    {
        private static string BACKEND_ENDPOINT = "http://pivovar3.azurewebsites.net";
        //private static string BACKEND_ENDPOINT = "http://localhost:31894";

        public LoginPage()
        {
            this.InitializeComponent();
            if(ApplicationData.Current.LocalSettings.Values["username"] != null)
            {
                Username.Text = ApplicationData.Current.LocalSettings.Values["Username"].ToString();
            }
            if (ApplicationData.Current.LocalSettings.Values["password"] != null)
            {
                Password.Password = ApplicationData.Current.LocalSettings.Values["Password"].ToString();
            }
            ApplicationData.Current.LocalSettings.Values.Remove("AuthenticationToken");
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing1.IsActive = true;
            string username = Username.Text;
            string password = Password.Password;
            PushNotificationChannel channel;

            try
            {
                await SetAuthenticationTokenInLocalStorage(username, password);
            }
            catch (HttpRequestException ex)
            {
                MessageDialog alert = new MessageDialog(ex.Message, "Failed to Sign In");
                alert.ShowAsync();
                ProgressRing1.IsActive = false;
                return;
            }

            try
            { 
                channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            }
            catch(HttpRequestException ex)
            {
                MessageDialog alert = new MessageDialog(ex.Message , "Failed to create push notification channel.");
                alert.ShowAsync();
                ProgressRing1.IsActive = false;
                return;
            }
            catch(Exception ex)
            {
                MessageDialog alert = new MessageDialog(ex.Message, "Failed to create push notification channel.");
                alert.ShowAsync();
                ProgressRing1.IsActive = false;
                Frame.Navigate(typeof(MainPage));
                return;
            }
            

            

            // The "username:<user name>" tag gets automatically added by the message handler in the backend.
            // The tag passed here can be whatever other tags you may want to use.
            try
            {
                // The device handle used will be different depending on the device and PNS. 
                // Windows devices use the channel uri as the PNS handle.
                await new RegisterNotificationClient(BACKEND_ENDPOINT).RegisterAsync(channel.Uri, new string[] { "clientApp", "app:"+username });

                var dialog = new MessageDialog("Registered as: " + username);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
                //SendPushButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                if(ex.Message == "OK")
                {
                    ProgressRing1.IsActive = false;
                    Frame.Navigate(typeof(MainPage));
                    return;
                }
                else
                {
                    MessageDialog alert = new MessageDialog(ex.Message, "Failed to register in Notification Hub.");
                    alert.ShowAsync();
                    ProgressRing1.IsActive = false;
                    return;
                }
            }
        }

        private async Task SetAuthenticationTokenInLocalStorage(string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("grant_type", "password")
                });
                try
                {
                    var response = await httpClient.PostAsync(BACKEND_ENDPOINT + "/token", formContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string regId = await response.Content.ReadAsStringAsync();
                        JObject o = (JObject)JsonConvert.DeserializeObject(regId);
                        var token = o["access_token"].ToString();
                        ApplicationData.Current.LocalSettings.Values["AuthenticationToken"] = token;
                        ApplicationData.Current.LocalSettings.Values["Username"] = username;
                        ApplicationData.Current.LocalSettings.Values["Password"] = password;
                    }
                    else
                    {
                        /*MessageDialog alert = new MessageDialog("Failed to LogIn. StatusCode: " + response.StatusCode.ToString());
                        alert.ShowAsync();*/
                        throw new HttpRequestException(response.StatusCode.ToString() + ": " + await response.Content.ReadAsStringAsync());
                    }
                }
                catch(HttpRequestException ex)
                {
                    throw ex;
                }
                
                

            }

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }
    }
}
