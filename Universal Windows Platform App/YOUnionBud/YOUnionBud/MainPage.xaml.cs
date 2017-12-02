using System;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YOUnionBud.Views;
using YOUnionBud.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace YOUnionBud
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;

            // Change status bar color
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {

                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    //statusBar.BackgroundColor = Color.FromArgb(230, 152, 0, 1);
                    statusBar.BackgroundColor = Colors.Black;
                    statusBar.ForegroundColor = Colors.White;
                }
            }
        }
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Load the local Accounts List before navigating to the UserSelection page
            await AccountHelper.LoadAccountListAsync();
            Frame.Navigate(typeof(UserSelection));
        }
    }
}
