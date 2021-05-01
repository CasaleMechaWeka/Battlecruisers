using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    public interface IHelpLabelManager
    {
        IBroadcastingProperty<bool> IsShown { get; }

        void HideHelpLabels();
        void ShowHelpLables();
    }
}