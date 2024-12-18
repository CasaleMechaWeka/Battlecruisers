
#if PLATFORM_ANDROID
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Network
{
    public interface IGoogleAuthentication
    {
        void InitializePlayGamesLogin();
        Task<bool> Authenticate(SignInInteractivity interactivity);
        void LoginGoogle();
        Task SignInWithGoogleAsync(string idToken);
        Task SignInWithGooglePlayGamesAsync(string authCode);
        Task LinkWithGoogleAsync(string idToken);
    }
}
#endif
