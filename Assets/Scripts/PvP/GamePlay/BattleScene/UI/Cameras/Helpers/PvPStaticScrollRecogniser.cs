using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPStaticScrollRecogniser : IPvPScrollRecogniser
    {
        private readonly bool _isScroll;

        public PvPStaticScrollRecogniser(bool isScroll)
        {
            _isScroll = isScroll;
        }

        public bool IsScroll(Vector2 delta)
        {
            return _isScroll;
        }
    }
}