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

        public void LoginToApple()
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
        public void QuickLoginWithApple()
        {
            var quickLoginArgs = new AppleAuthQuickLoginArgs();
            m_AppleAuthManager.QuickLogin(
            quickLoginArgs,
            credential =>
            {
                // Received a valid credential!
                // Try casting to IAppleIDCredential or IPasswordCredential
                // Previous Apple sign in credential
                var appleIdCredential = credential as IAppleIDCredential;
                // Saved Keychain credential (read about Keychain Items)
                var passwordCredential = credential as IPasswordCredential;
            },
            error =>
            {
                // Quick login failed. The user has never used Sign in With Apple on your app.
                Debug.Log("Sign-in with Apple error. Message: " + error);
            });
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
