using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Pivovar_Mobile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string PageNamespace = nameof(Pivovar_Mobile);

        public MainPage()
        {
            this.InitializeComponent();

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            //remove the solid-colored backgrounds behind the caption controls and system back button
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            CoreApplicationViewTitleBar titleBarr = CoreApplication.GetCurrentView().TitleBar;
            titleBarr.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;

            //MainFrame.Navigate(typeof(DevicesPage));
            

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += On_BackRequested;
        }

        private void On_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
            else if (Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= On_BackRequested;
                CoreApplicationViewTitleBar titleBarr = CoreApplication.GetCurrentView().TitleBar;
                titleBarr.LayoutMetricsChanged -= TitleBar_LayoutMetricsChanged;

                var currentView = SystemNavigationManager.GetForCurrentView();
                currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

                Frame.GoBack();
            }
                
        }

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitle.Margin = new Thickness(CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset + 12, 4, 0, 0);
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DevicesPage));
            NavView.SelectedItem = NavView.MenuItems[0];
        }

        /*private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem as NavigationViewItem;
            var tag = selectedItem.Tag;

            if(tag as string == "home")
            {
                MainFrame.Navigate(typeof(DevicesPage));
            }
            else if(args.IsSettingsSelected)
            {
                MainFrame.Navigate(typeof(SettingsPage));
            }
            else if (tag as string == "about")
            {
                MainFrame.Navigate(typeof(AboutPage));
            }
        }*/

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var pageName = MainFrame.Content.GetType().Name;
            if(pageName == "SettingsPage")
                NavView.SelectedItem = NavView.SettingsItem;
            else
            {
                var item = NavView.MenuItems.OfType<NavigationViewItem>().Where(i => i.Tag.ToString() == pageName).FirstOrDefault();
                if(item != null)
                {
                    NavView.SelectedItem = item;
                }
            }
                
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var invokedMenuItem = sender.MenuItems.OfType<NavigationViewItem>().Where(item => item.Content.ToString() == args.InvokedItem.ToString()).FirstOrDefault();
            string pageTypeName;
            if (invokedMenuItem == null)
                pageTypeName = "SettingsPage";
            else
                pageTypeName = invokedMenuItem.Tag.ToString();
            var pageType = Assembly.GetExecutingAssembly().GetType($"{PageNamespace}.{pageTypeName}");
            MainFrame.Navigate(pageType);
        }
    }
}
