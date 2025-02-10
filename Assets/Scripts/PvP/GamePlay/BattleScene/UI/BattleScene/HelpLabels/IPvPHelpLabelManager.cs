using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.HelpLabels
{
    public interface IPvPHelpLabelManager
    {
        IBroadcastingProperty<bool> IsShown { get; }

        void HideHelpLabels();
        void ShowHelpLabels();
    }
}