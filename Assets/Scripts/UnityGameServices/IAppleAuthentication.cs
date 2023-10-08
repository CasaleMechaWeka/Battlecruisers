using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using System;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Network
{
    public interface IAppleAuthentication
    {
        public IAppleAuthManager m_AppleAuthManager { get; }
        string Token { get; }
        string Error { get; }
        void Initialize();
        void Update();
        void QuickLoginApple(AppleAuthQuickLoginArgs quickLoginArgs, Action<ICredential> successCallback, Action<IAppleError> errorCallback);
        Task SignInWithAppleAsync(string idToken);
        Task LoginApple();
    }
}
