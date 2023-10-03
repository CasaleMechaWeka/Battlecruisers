using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Utils.Network
{
    public interface IAppleAuthentication
    {
        string Token { get; }
        string Error { get; }
        void Initialize();
        void Update();
        void LoginToApple();
        void QuickLoginWithApple();
        Task SignInWithAppleAsync(string idToken);
    }
}
