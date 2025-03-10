using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.BattleScene.HelpLabels.States
{
    public class InformatorShownState : HelpState
    {
        public InformatorShownState(IPanel helpLabelCanvas, ISlidingPanel informatorExtendedPanel, IHelpLabels helpLabels) 
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