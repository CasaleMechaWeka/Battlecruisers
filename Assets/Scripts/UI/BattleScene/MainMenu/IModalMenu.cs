using UnityCommon.Properties;

namespace BattleCruisers.UI.BattleScene.MainMenu
{
    public interface IModalMenu
    {
        IBroadcastingProperty<bool> IsVisible { get; }

        void ShowMenu();
        void HideMenu();
        void ShowSettings();
    }
}