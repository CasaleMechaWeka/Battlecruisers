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
#if APPLE_AUTH_MANAGER_NATIVE_IMPLEMENTATION_AVAILABLE
            var requestId = CallbackHandler.AddMessageCallback(
                true,
                payload =>
                {
                    var response = this._payloadDeserializer.DeserializeCredentialStateResponse(payload);
                    if (response.Error != null)
                        errorCallback(response.Error);
                    else
                        successCallback(response.CredentialState);
                });
            
            PInvoke.AppleAuth_GetCredentialState(requestId, userId);
#else
            throw new Exception("AppleAuthManager is not supported in this platform");
#endif
        }

        public void SetCredentialsRevokedCallback(Action<string> credentialsRevokedCallback)
        {
#if APPLE_AUTH_MANAGER_NATIVE_IMPLEMENTATION_AVAILABLE
            if (this._credentialsRevokedCallback != null)
            {
                CallbackHandler.NativeCredentialsRevoked -= this._credentialsRevokedCallback;
                this._credentialsRevokedCallback = null;
            }

            if (credentialsRevokedCallback != null)
            {
                CallbackHandler.NativeCredentialsRevoked += credentialsRevokedCallback;
                this._credentialsRevokedCallback = credentialsRevokedCallback;
            }
#endif
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
#if APPLE_AUTH_MANAGER_NATIVE_IMPLEMENTATION_AVAILABLE
            var nonce = quickLoginArgs.Nonce;
            var state = quickLoginArgs.State;
            var requestId = CallbackHandler.AddMessageCallback(
                true,
                payload =>
                {
                    var response = this._payloadDeserializer.DeserializeLoginWithAppleIdResponse(payload);
                    if (response.Error != null)
                        errorCallback(response.Error);
                    else if (response.PasswordCredential != null)
                        successCallback(response.PasswordCredential);
                    else
                        successCallback(response.AppleIDCredential);
                });

            PInvoke.AppleAuth_QuickLogin(requestId, nonce, state);
#else
            throw new Exception("AppleAuthManager is not supported in this platform");
#endif
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
