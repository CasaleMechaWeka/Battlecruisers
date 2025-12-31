using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.BattleScene.HelpLabels.States
{
    public class BothShownState : HelpState
    {
        public BothShownState(Panel helpLabelCanvas, SlidingPanel informatorExtendedPanel, HelpLabelsController helpLabels)
            : base(helpLabelCanvas, informatorExtendedPanel, helpLabels)
        {
        }

        public override void ShowHelpLabels()
        {
            base.ShowHelpLabels();

            _informatorExtendedPanel.Hide();

            _helpLabels.BuildMenu.Show();
            _helpLabels.Informator.Show();
        }
    }
}