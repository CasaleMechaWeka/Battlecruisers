using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.BattleScene.HelpLabels.States
{
    public class InformatorShownState : HelpState
    {
        public InformatorShownState(Panel helpLabelCanvas, SlidingPanel informatorExtendedPanel, IHelpLabels helpLabels)
            : base(helpLabelCanvas, informatorExtendedPanel, helpLabels)
        {
        }

        public override void ShowHelpLabels()
        {
            base.ShowHelpLabels();

            _informatorExtendedPanel.Hide();
            _helpLabels.Informator.Show();
        }
    }
}