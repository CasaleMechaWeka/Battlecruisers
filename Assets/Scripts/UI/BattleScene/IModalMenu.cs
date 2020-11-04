using UnityCommon.Properties;

namespace BattleCruisers.UI.BattleScene
{
    public interface IModalMenu
    {
        IBroadcastingProperty<bool> IsVisible { get; }

        void ShowMenu();
        void HideMenu();
    }
}