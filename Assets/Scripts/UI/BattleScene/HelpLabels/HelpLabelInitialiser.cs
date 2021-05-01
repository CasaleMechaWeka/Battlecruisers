using BattleCruisers.UI.BattleScene.HelpLabels.States;
using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    public class HelpLabelInitialiser : MonoBehaviour
    {
        public Panel helpLabelCanvas;
        public HelpLabelsController helpLabels;

        public IHelpLabelManager Initialise(
            LeftPanelComponents leftPanelComponents,
            RightPanelComponents rightPanelComponents,
            IPauseGameManager pauseGameManager)
        {
            Helper.AssertIsNotNull(helpLabelCanvas, helpLabels);
            Helper.AssertIsNotNull(leftPanelComponents, rightPanelComponents, pauseGameManager);

            helpLabels.Initialise();

            ISlidingPanel extendedInformatorPanel = rightPanelComponents.InformatorPanel.ExtendedPanel;

            IHelpStateFinder helpStateFinder
                = new HelpStateFinder(
                    rightPanelComponents.InformatorPanel,
                    leftPanelComponents.BuildMenu.SelectorPanel,
                    new BothCollapsedState(helpLabelCanvas, extendedInformatorPanel, helpLabels),
                    new SelectorShownState(helpLabelCanvas, extendedInformatorPanel, helpLabels),
                    new InformatorShownState(helpLabelCanvas, extendedInformatorPanel, helpLabels),
                    new BothShownState(helpLabelCanvas, extendedInformatorPanel, helpLabels));

            return
                new HelpLabelManager(
                    helpStateFinder,
                    pauseGameManager);
        }
    }
}