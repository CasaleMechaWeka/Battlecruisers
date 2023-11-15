using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public interface IPvPModalMenu
    {
        IPvPBroadcastingProperty<bool> IsVisible { get; }

        void ShowMenu();
        void HideMenu();
        void ShowSettings();
    }
}