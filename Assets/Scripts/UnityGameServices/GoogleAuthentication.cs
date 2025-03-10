#if PLATFORM_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace BattleCruisers.Utils.Network
{
    // Google authentication currently works under these specific conditions:
    // - Unity's Authentication package 2.4.0
    // - Google Play Games plugin version 0.10.14
    // - OAuth clients in GPC/Google Cloud set up as they are right now, on 14/08/2023
    //
    // Nothing sums it up better than this quote from a Unity Developer in a thread on the Unity forums:
    // "Just to clarify, Authentication works fine, it's google's API we are trying to clarify as it's ever changing."

    public class GoogleAuthentication : IGoogleAuthentication
    {
        public void InitializePlayGamesLogin()
        {
          
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }

        //Fetch the Token / Auth code
        public async Task<bool> Authenticate(SignInInteractivity interactivity)
        {

            bool state = false;
            //The compiler doesn't like it if "interactivity" isn't passed into Authenticate().
            //This diverges from tutorials and documentation!
            PlayGamesPlatform.Instance.Authenticate( (success) =>
            {
                if (SignInStatus.Success == success)
                {
                    Debug.Log("PlayGamesPlatform: Authenticate ====  success");
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, (token) => 
                    {
                        Debug.Log("PlayGamesPlatform: RequestServerSideAccess: " + token);
                        SignInWithGooglePlayGamesAsync(token);
                        state = true;
                    });
                   
                    state = true;
                }
                else
                {
                    Debug.Log("Failed to retrieve Google play games authorization code");
                    state = false;
                }
            });
            return state;
        }

        public void LoginGoogle()
        {
            Social.localUser.Authenticate(OnGoogleLogin);
        }

        void OnGoogleLogin(bool success)
        {
            if (success)
            {
                // Call Unity Authentication SDK to sign in or link with Google.
                Debug.Log("Login with Google done. IdToken: " + ((PlayGamesLocalUser)Social.localUser).id);
            }
            else
            {
                Debug.Log("Unsuccessful login");
            }
        }

        // Sign in a returning player or create new player
        public async Task SignInWithGoogleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithGoogleAsync(idToken);
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


        // This one is Google Play as opposed to Google
        // But both AuthenticationServices run through the GooglePlayGames api/plugin/sdk!
        public async Task SignInWithGooglePlayGamesAsync(string authCode)
        {
            try
            {
                Debug.Log("PlayGamesPlatform: SignInWithGooglePlayGamesAsync: " + authCode);
                await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); //Display the Unity Authentication PlayerID
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

        // Update a player from anonymous to a Google account
        public async Task LinkWithGoogleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithGoogleAsync(idToken);
                Debug.Log("Link is successful.");
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                // Prompt the player with an error message.
                Debug.LogError("This user is already linked with another account. Log in instead.");
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
