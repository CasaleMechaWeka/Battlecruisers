using UnityEngine;

namespace BattleCruisers.Utils.Network
{
    public interface IAppleAuthentication
    {
        void Initialize();
        void Update();
        void LoginToApple();
        void QuickLoginWithApple();
    }
}
