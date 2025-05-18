using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.HelpLabels.States;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.HelpLabels
{
    public class HelpLabelInitialiser : MonoBehaviour
    {
        public Panel helpLabelCanvas;
        public HelpLabelsController helpLabels;
        public HelpButton modalHelpButton;

        public HelpLabelManager Initialise(
            LeftPanelComponents leftPanelComponents,
            RightPanelComponents rightPanelComponents,
            PauseGameManager pauseGameManager,
            SingleSoundPlayer soundPlayer,
            NavigationPermitterManager navigationPermitterManager)
        {
            Helper.AssertIsNotNull(helpLabelCanvas, helpLabels, modalHelpButton);
            Helper.AssertIsNotNull(leftPanelComponents, rightPanelComponents, pauseGameManager, soundPlayer, navigationPermitterManager);

            helpLabels.Initialise();

            SlidingPanel extendedInformatorPanel = rightPanelComponents.InformatorPanel.ExtendedPanel;

            HelpStateFinder helpStateFinder
                = new HelpStateFinder(
                    rightPanelComponents.InformatorPanel,
                    leftPanelComponents.BuildMenu.SelectorPanel,
                    new BothCollapsedState(helpLabelCanvas, extendedInformatorPanel, helpLabels),
                    new SelectorShownState(helpLabelCanvas, extendedInformatorPanel, helpLabels),
                    new InformatorShownState(helpLabelCanvas, extendedInformatorPanel, helpLabels),
                    new BothShownState(helpLabelCanvas, extendedInformatorPanel, helpLabels));

            HelpLabelManager helpLabelManager
                = new HelpLabelManager(
                    navigationPermitterManager,
                    pauseGameManager,
                    helpStateFinder);

            // Initialised here because of circular dependency: HelpButton > HelpLabelManager > UI (Informator/Selector)
            rightPanelComponents.HelpButton.Initialise(soundPlayer, helpLabelManager);
            modalHelpButton.Initialise(soundPlayer, helpLabelManager);

            return helpLabelManager;
        }
    }
}