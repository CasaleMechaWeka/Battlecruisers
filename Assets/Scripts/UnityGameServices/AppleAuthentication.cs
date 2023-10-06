#if PLATFORM_IOS
using System.Text;
using UnityEngine;
using Unity.Services.Authentication;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using System.Threading.Tasks;
using Unity.Services.Core;
using System;
using BattleCruisers.Scenes;

namespace BattleCruisers.Utils.Network
{
    public class AppleAuthentication : IAppleAuthentication
    {
        IAppleAuthManager m_AppleAuthManager;
        public string Token { get; private set; }
        public string Error { get; private set; }

        public void Initialize()
        {
            var deserializer = new PayloadDeserializer();
            m_AppleAuthManager = new AppleAuthManager(deserializer);
        }

        public void Update()
        {
            if (m_AppleAuthManager != null)
            {
                m_AppleAuthManager.Update();
            }
        }

        public void GetCredentialState(
            string userId,
            Action<CredentialState> successCallback,
            Action<IAppleError> errorCallback)
        {
            m_AppleAuthManager.GetCredentialState(userId, successCallback, errorCallback);
        }

        public void SetCredentialsRevokedCallback(Action<string> credentialsRevokedCallback)
        {
            m_AppleAuthManager.SetCredentialsRevokedCallback(credentialsRevokedCallback);
        }

        public void LoginApple()
        {
            // Initialize the Apple Auth Manager
            if (m_AppleAuthManager == null)
            {
                Initialize();
            }

            // Set the login arguments
            var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

            // Perform the login
            m_AppleAuthManager.LoginWithAppleId(
                loginArgs,
                credential =>
                {
                    var appleIDCredential = credential as IAppleIDCredential;
                    if (appleIDCredential != null)
                    {
                        var idToken = Encoding.UTF8.GetString(
                            appleIDCredential.IdentityToken,
                            0,
                            appleIDCredential.IdentityToken.Length);
                        Debug.Log("Sign-in with Apple successfully done. IDToken: " + idToken);
                        SignInWithAppleAsync(Token);
                        Token = idToken;
                    }
                    else
                    {
                        Debug.Log("Sign-in with Apple error. Message: appleIDCredential is null");
                        Error = "Retrieving Apple Id Token failed.";
                    }
                },
                error =>
                {
                    Debug.Log("Sign-in with Apple error. Message: " + error);
                    Error = "Retrieving Apple Id Token failed.";
                }
            );
        }

        //If the user has previously authorized the app to login with Apple, this will open a
        //native dialog to re-confirm the login, and obtain an Apple User ID.
        //If the credentials were never given, or they were revoked, the Quick login will fail.
        public void QuickLoginApple(AppleAuthQuickLoginArgs quickLoginArgs,Action<ICredential> successCallback,Action<IAppleError> errorCallback)
        {
            m_AppleAuthManager.QuickLogin(quickLoginArgs, successCallback, errorCallback);
        }

        // Sign in a returning player or create new player
        public async Task SignInWithAppleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithAppleAsync(idToken);
                Debug.Log("SignIn is successful.");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }
    }
}
#endif
