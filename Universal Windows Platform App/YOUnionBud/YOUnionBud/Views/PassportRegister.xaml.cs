using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YOUnionBud.Models;
using YOUnionBud.Utils;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;

namespace YOUnionBud.Views
{
    public sealed partial class PassportRegister : Page
    {
        private Account _account;

        public PassportRegister()
        {
            this.InitializeComponent();
        }

        private async void RegisterButton_Click_Async(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = "";

            //In the real world you would normally validate the entered credentials and information before 
            //allowing a user to register a new account. 
            //For this sample though we will skip that step and just register an account if username is not null.

            if (!string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                if (PasswordTextBox.ToString().Equals(PasswordConfirmTextBox.ToString()))
                {
                    //Register a new account
                    _account = AccountHelper.AddAccount(UsernameTextBox.Text);
                    //Register new account with Microsoft Passport
                    await MicrosoftPassportHelper.CreatePassportKeyAsync(_account.Username);
                    //Navigate to the Welcome Screen. 
                    Frame.Navigate(typeof(Welcome), _account);
                }
                else
                {
                    MessageDialog showDialog = new MessageDialog("You have mistyped your password");
                    showDialog.Commands.Add(new UICommand("Okay")
                    {
                        Id = 0
                    });
                    showDialog.DefaultCommandIndex = 0;
                    var result = await showDialog.ShowAsync();
                }
            }
            else
            {
                ErrorMessage.Text = "Please enter a username";
            }
        }
    }
}
