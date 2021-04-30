using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    public abstract class HelpState : IHelpState
    {
        protected readonly ISlidingPanel _informatorExtendedPanel;
        protected readonly IHelpLabels _helpLabels;

        protected HelpState(ISlidingPanel informatorExtendedPanel, IHelpLabels helpLabels)
        {
            Helper.AssertIsNotNull(informatorExtendedPanel, helpLabels);

            _informatorExtendedPanel = informatorExtendedPanel;
            _helpLabels = helpLabels;
        }

        public virtual void ShowHelpLabels()
        {
            // FELIX  Show help label canvas
            // FELIX  Common to all 4 stats (cruiser health bars?)
        }

        public void HideHelpLables()
        {
            // FELIX  Hide everything
        }
    }
}