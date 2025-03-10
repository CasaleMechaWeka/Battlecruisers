using UnityEngine;

namespace BattleCruisers.UI.BattleScene
{
    public interface ITargetIndicator
    {
        void Show(Vector2 position);
        void Hide();
    }
}