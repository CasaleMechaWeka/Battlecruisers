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

namespace BattleCruisers.Utils.Network
{
    public class AppleAuthentication : IAppleAuthentication
    {
        public IAppleAuthManager m_AppleAuthManager { get; private set; }
        public string Token { get; private set; }
        public string Error { get; private set; }

        public void Initialize()
        {
            var deserializer = new PayloadDeserializer();
            Debug.LogError("####### deserializer assignment failed.");
            m_AppleAuthManager = new AppleAuthManager(deserializer);
            Debug.LogError("####### m_AppleAuthManager assignment failed.");
        }

        public void Update()
        {
            if (m_AppleAuthManager != null)
            {
                m_AppleAuthManager.Update();
            }
        }

        public async Task LoginApple()
        {
            // Initialize the Apple Auth Manager
            if (m_AppleAuthManager == null)
            {
                Debug.LogError("####### m_AppleAuthManager == null");
                Initialize();
                Debug.LogError("####### m_AppleAuthManager failed to Initialize");
            }
            Debug.LogError("####### m_AppleAuthManager not null");

            // Set the login arguments
            var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);
            Debug.LogError("####### loginArgs did not assign correctly.");

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

            if(Token != null)
            {
                Debug.LogError("####### Attempting SignInWithAppleAsync(Token)");
                await SignInWithAppleAsync(Token);
            }
            Debug.LogError("####### LoginWithAppleId failed for reasons unknown.");
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
