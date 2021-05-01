using BattleCruisers.UI.Panels;

namespace BattleCruisers.UI.BattleScene.HelpLabels.States
{
    public class SelectorShownState : HelpState
    {
        public SelectorShownState(IPanel helpLabelCanvas, ISlidingPanel informatorExtendedPanel, IHelpLabels helpLabels) 
            : base(helpLabelCanvas, informatorExtendedPanel, helpLabels)
        {
        }

        public override void ShowHelpLabels()
        {
            base.ShowHelpLabels();

            _helpLabels.LeftBottom.Show();
            _helpLabels.RightBottom.Show();
            _helpLabels.BuildMenu.Show();
        }
    }
}