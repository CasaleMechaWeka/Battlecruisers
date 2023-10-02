#if PLATFORM_IOS
using System.Text;
using UnityEngine;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;

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
    }
}
#endif
