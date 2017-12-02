using YOUnionBud.Models;
using YOUnionBud.Utils;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System;

namespace YOUnionBud.Views
{
    public sealed partial class Welcome : Page
    {
        private Account _activeAccount;
        Random rnd = new Random();
        int accountBal;

        public Welcome()
        {
            InitializeComponent();
            accountBal = rnd.Next(18649, 236156);
            acctBal.Text = "₱ " + string.Format("{0:n0}", accountBal);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _activeAccount = (Account)e.Parameter;
            if (_activeAccount != null)
            {
                UserNameText.Text = _activeAccount.Username;
            }
        }

        private void Button_Restart_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserSelection));
        }

        private void Button_Forget_User_Click(object sender, RoutedEventArgs e)
        {
            // Remove it from Microsoft Passport
            MicrosoftPassportHelper.RemovePassportAccountAsync(_activeAccount);

            // Remove it from the local accounts list and resave the updated list
            AccountHelper.RemoveAccount(_activeAccount);

            Debug.WriteLine("User " + _activeAccount.Username + " deleted.");

            // Navigate back to UserSelection page.
            Frame.Navigate(typeof(UserSelection));
        }
    }
}