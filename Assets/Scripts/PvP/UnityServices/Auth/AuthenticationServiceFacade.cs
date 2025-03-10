using System;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using Unity.Services.Core;
using Unity.Services.Authentication;
using BattleCruisers.Network.Multiplay.Infrastructure;
// #if UNITY_EDITOR
// using ParrelSync;
// #endif

namespace BattleCruisers.Network.Multiplay.UnityServices.Auth
{

    public class AuthenticationServiceFacade
    {
        [Inject] IPublisher<UnityServiceErrorMessage> m_UnityServiceErrorMessagePublisher;

        public async Task InitializeAndSignInAsync(InitializationOptions initializationOptions)
        {
            try
            {
                await Unity.Services.Core.UnityServices.InitializeAsync(initializationOptions);

#if UNITY_EDITOR
                if (ParrelSync.ClonesManager.IsClone())
                {
                    // When using a ParrelSync clone, switch to a different authentication profile to force the clone
                    // to sign in as a different anonymous user account.
                    string customArgument = ParrelSync.ClonesManager.GetArgument();
                    AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
                }
#endif
                
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
            }
            catch (Exception e)
            {
                var reason = $"{e.Message} ({e.InnerException?.Message})";
                m_UnityServiceErrorMessagePublisher.Publish(new UnityServiceErrorMessage("Authentication Error", reason, UnityServiceErrorMessage.Service.Authentication, e));
                throw;
            }
        }
   

        public async Task SwitchProfileAndReSignInAsync(string profile)
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                AuthenticationService.Instance.SignOut();
            }
            AuthenticationService.Instance.SwitchProfile(profile);

            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (Exception e)
            {
                var reason = $"{e.Message} ({e.InnerException?.Message})";
                m_UnityServiceErrorMessagePublisher.Publish(new UnityServiceErrorMessage("Authentication Error", reason, UnityServiceErrorMessage.Service.Authentication, e));
                throw;
            }
        }


        public void AddActionToSignedInEvent(Action action)
        {
            AuthenticationService.Instance.SignedIn += action;
        }
        public async Task<bool> EnsurePlayerIsAuthorized()
        {
            if (AuthenticationService.Instance.IsAuthorized)
            {
                return true;
            }

            try
            {           
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                return true;
            }
            catch (AuthenticationException e)
            {
                var reason = $"{e.Message} ({e.InnerException?.Message})";
                Debug.Log("Reason ---> " + reason);
                m_UnityServiceErrorMessagePublisher.Publish(new UnityServiceErrorMessage("Authentication Error", reason, UnityServiceErrorMessage.Service.Authentication, e));
                //not rethrowing for authentication exceptions - any failure to authenticate is considered "handled failure"
                return false;
            }
            catch (Exception e)
            {
                //all other exceptions should still bubble up as unhandled ones
                var reason = $"{e.Message} ({e.InnerException?.Message})";
                m_UnityServiceErrorMessagePublisher.Publish(new UnityServiceErrorMessage("Authentication Error", reason, UnityServiceErrorMessage.Service.Authentication, e));
                throw;
            }
        }

    }
}


