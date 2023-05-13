using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.HelpLabels
{
    public interface IPvPHelpLabelManager
    {
        IPvPBroadcastingProperty<bool> IsShown { get; }

        void HideHelpLabels();
        void ShowHelpLabels();
    }
}