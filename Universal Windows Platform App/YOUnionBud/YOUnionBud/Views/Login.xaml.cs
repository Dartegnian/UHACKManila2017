using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YOUnionBud.Utils;
using YOUnionBud.Models;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YOUnionBud.Views
{
    public sealed partial class Login : Page
    {
        private Account _account;

        public Login()
        {
            this.InitializeComponent();
        }
        private void PassportSignInButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = "";
            SignInPassport();
        }
        private void RegisterButtonTextBlock_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ErrorMessage.Text = "";
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Check Microsoft Passport is setup and available on this machine
            if (await MicrosoftPassportHelper.MicrosoftPassportAvailableCheckAsync())
            {
            }
            else
            {
                // Microsoft Passport is not setup so inform the user
                PassportStatus.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 50, 170, 207));
                PassportStatusText.Text = "Microsoft Passport is not setup!\nPlease go to Windows Settings and set up a PIN to use it.";
                PassportSignInButton.IsEnabled = false;
            }
        }
        private async void SignInPassport()
        {
            if (AccountHelper.ValidateAccountCredentials(UsernameTextBox.Text))
            {
                // Create and add a new local account
                _account = AccountHelper.AddAccount(UsernameTextBox.Text);
                Debug.WriteLine("Successfully signed in with traditional credentials and created local account instance!");

                if (await MicrosoftPassportHelper.CreatePassportKeyAsync(UsernameTextBox.Text))
                {
                    Debug.WriteLine("Successfully signed in with Microsoft Passport!");
                    Frame.Navigate(typeof(Welcome), _account);
                }
            }
            else
            {
                ErrorMessage.Text = "Invalid Credentials";
            }
        }
    }
}