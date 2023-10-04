using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using System;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Network
{
    public interface IAppleAuthentication
    {
        string Token { get; }
        string Error { get; }
        void Initialize();
        void Update();
        void LoginApple();
        void QuickLoginApple(AppleAuthQuickLoginArgs quickLoginArgs, Action<ICredential> successCallback, Action<IAppleError> errorCallback);
        Task SignInWithAppleAsync(string idToken);
        void GetCredentialState(string userId, Action<CredentialState> successCallback, Action<IAppleError> errorCallback);
        void SetCredentialsRevokedCallback(Action<string> credentialsRevokedCallback);
    }
}
