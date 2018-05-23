using Pivovar_Mobile.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Pivovar_Mobile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing1.IsActive = true;
            string email = Username.Text;
            string password = Password.Password;
            string firstName = FirstName.Text;
            string lastName = LastName.Text;
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await Users.Post(email: email, password: password, firstName: firstName, lastName: lastName);

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        ProgressRing1.IsActive = false;
                        Frame.Navigate(typeof(LoginPage));
                        return;
                    }
                    else
                    {
                        throw new HttpRequestException(response.StatusCode.ToString() + ": " + await response.Content.ReadAsStringAsync());
                    }
                }
                catch(HttpRequestException ex)
                {
                    MessageDialog alert = new MessageDialog(ex.Message, "Failed to Sign Up");
                    alert.ShowAsync();
                    ProgressRing1.IsActive = false;
                    return;
                }
                
            }
        }
    }
}
