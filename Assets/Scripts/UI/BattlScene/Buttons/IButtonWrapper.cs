using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IButtonWrapper
    {
        Button Button { get; }
        bool IsEnabled { set; }
    }
}
