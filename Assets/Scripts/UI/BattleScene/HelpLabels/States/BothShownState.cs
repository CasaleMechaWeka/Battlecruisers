using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.BattleScene.HelpLabels.States
{
    public class BothShownState : HelpState
    {
        public BothShownState(IPanel helpLabelCanvas, ISlidingPanel informatorExtendedPanel, IHelpLabels helpLabels) 
            : base(helpLabelCanvas, informatorExtendedPanel, helpLabels)
        {
        }

        public override void ShowHelpLabels()
        {
            base.ShowHelpLabels();

            _helpLabels.BuildMenu.Show();
            _helpLabels.Informator.Show();
        }
    }
}