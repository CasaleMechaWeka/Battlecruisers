using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public interface IPvPTargetIndicator
    {
        void Show(Vector2 position);
        void Hide();
    }
}