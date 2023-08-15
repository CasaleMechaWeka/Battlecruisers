using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace BattleCruisers.Utils.Network
{
    public interface IGoogleAuthentication
    {
        #if PLATFORM_ANDROID
        void InitializePlayGamesLogin();
        Task Authenticate(SignInInteractivity interactivity);
        void LoginGoogle();
        Task SignInWithGoogleAsync(string idToken);
        Task SignInWithGooglePlayGamesAsync(string authCode);
        Task LinkWithGoogleAsync(string idToken);
        #endif
    }
}
