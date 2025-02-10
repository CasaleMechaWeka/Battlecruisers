using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public interface IPvPModalMenu
    {
        IBroadcastingProperty<bool> IsVisible { get; }

        void ShowMenu();
        void HideMenu();
        void ShowSettings();
    }
}