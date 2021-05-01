using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    public abstract class HelpState : IHelpState
    {
        private readonly IPanel _helpLabelCanvas;
        protected readonly ISlidingPanel _informatorExtendedPanel;
        protected readonly IHelpLabels _helpLabels;

        protected HelpState(IPanel helpLabelCanvas, ISlidingPanel informatorExtendedPanel, IHelpLabels helpLabels)
        {
            Helper.AssertIsNotNull(helpLabelCanvas, informatorExtendedPanel, helpLabels);

            _helpLabelCanvas = helpLabelCanvas;
            _informatorExtendedPanel = informatorExtendedPanel;
            _helpLabels = helpLabels;
        }

        public virtual void ShowHelpLabels()
        {
            _helpLabelCanvas.Show();
            _helpLabels.CruiserHealth.Show();
        }

        public void HideHelpLables()
        {
            _helpLabelCanvas.Hide();
            _helpLabels.CruiserHealth.Hide();
            _helpLabels.LeftBottom.Hide();
            _helpLabels.BuildingCategories.Hide();
            _helpLabels.RightBottom.Hide();
            _helpLabels.Informator.Hide();
            _helpLabels.BuildMenu.Hide();
        }
    }
}