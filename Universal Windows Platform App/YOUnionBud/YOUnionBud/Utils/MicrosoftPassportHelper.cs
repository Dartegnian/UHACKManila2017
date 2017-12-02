using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using YOUnionBud.Models;

namespace YOUnionBud.Utils
{
    public static class MicrosoftPassportHelper
    {
        /// <summary>
        /// Checks to see if Passport is ready to be used.
        /// 
        /// Passport has dependencies on:
        ///     1. Having a connected Microsoft Account
        ///     2. Having a Windows PIN set up for that _account on the local machine
        /// </summary>
        public static async Task<bool> MicrosoftPassportAvailableCheckAsync()
        {
            bool keyCredentialAvailable = await KeyCredentialManager.IsSupportedAsync();
            if (keyCredentialAvailable == false)
            {
                // Key credential is not enabled yet as user 
                // needs to connect to a Microsoft Account and select a PIN in the connecting flow.
                Debug.WriteLine("Microsoft Passport is not setup!\nPlease go to Windows Settings and set up a PIN to use it.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a Passport key on the machine using the _account id passed.
        /// </summary>
        /// <param name="accountId">The _account id associated with the _account that we are enrolling into Passport</param>
        /// <returns>Boolean representing if creating the Passport key succeeded</returns>
        public static async Task<bool> CreatePassportKeyAsync(string accountId)
        {
            KeyCredentialRetrievalResult keyCreationResult = await KeyCredentialManager.RequestCreateAsync(accountId, KeyCredentialCreationOption.ReplaceExisting);

            switch (keyCreationResult.Status)
            {
                case KeyCredentialStatus.Success:
                    Debug.WriteLine("Successfully made key");

                    // In the real world authentication would take place on a server.
                    // So every time a user migrates or creates a new Microsoft Passport account Passport details should be pushed to the server.
                    // The details that would be pushed to the server include:
                    // The public key, keyAttesation if available, 
                    // certificate chain for attestation endorsement key if available,  
                    // status code of key attestation result: keyAttestationIncluded or 
                    // keyAttestationCanBeRetrievedLater and keyAttestationRetryType
                    // As this sample has no concept of a server it will be skipped for now
                    // for information on how to do this refer to the second Passport sample

                    //For this sample just return true
                    return true;
                case KeyCredentialStatus.UserCanceled:
                    Debug.WriteLine("User cancelled sign-in process.");
                    break;
                case KeyCredentialStatus.NotFound:
                    // User needs to setup Microsoft Passport
                    Debug.WriteLine("Microsoft Passport is not setup!\nPlease go to Windows Settings and set up a PIN to use it.");
                    break;
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Function to be called when user requests deleting their account.
        /// Checks the KeyCredentialManager to see if there is a Passport for the current user
        /// Then deletes the local key associated with the Passport.
        /// </summary>
        public static async void RemovePassportAccountAsync(Account account)
        {
            // Open the account with Passport
            KeyCredentialRetrievalResult keyOpenResult = await KeyCredentialManager.OpenAsync(account.Username);

            if (keyOpenResult.Status == KeyCredentialStatus.Success)
            {
                // In the real world you would send key information to server to unregister
                //e.g. RemovePassportAccountOnServer(account);
            }

            // Then delete the account from the machines list of Passport Accounts
            await KeyCredentialManager.DeleteAsync(account.Username);
        }
    }
}
