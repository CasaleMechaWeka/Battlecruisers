using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public interface IPvPScrollRecogniser
    {
        bool IsScroll(Vector2 delta);
    }
}