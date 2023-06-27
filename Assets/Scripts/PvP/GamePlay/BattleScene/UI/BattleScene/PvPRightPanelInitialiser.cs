using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    /// <summary>
    /// Contains:
    /// + Informator (item details)
    /// + Game speed buttons
    /// + Help button
    /// + Menu button
    /// </summary>
    public class PvPRightPanelInitialiser : MonoBehaviour
    {
        public PvPModalMenuController modalMenu;
        public PvPMainMenuButtonController modalMainMenuButton;
        public PvPHelpButton helpButton;
        public RectTransform rectTransformThis;            

        public PvPRightPanelComponents Initialise(
            IApplicationModel applicationModel,
            IPvPUIManager uiManager,
            IPvPCruiser playerCruiser,
            IPvPUserChosenTargetHelper userChosenTargetHelper,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPUpdater perFrameUpdater,
            IPvPPauseGameManager pauseGameManager,
            IPvPBattleCompletionHandler battleCompletionHandler,
            IPvPSingleSoundPlayer soundPlayer,
            IPvPNavigationPermitterManager navigationPermitterManager
            )
        {
            PvPHelper.AssertIsNotNull(modalMenu, modalMainMenuButton, helpButton);
            PvPHelper.AssertIsNotNull(
                applicationModel,
                uiManager,
                playerCruiser, 
                userChosenTargetHelper,
                buttonVisibilityFilters,
                perFrameUpdater,
                battleCompletionHandler,
                soundPlayer,
                navigationPermitterManager
                );

            IPvPInformatorPanel informator = SetupInformator(uiManager, playerCruiser, perFrameUpdater, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer);
            PvPSpeedComponents speedComponents = SetupSpeedPanel(soundPlayer, buttonVisibilityFilters);
            IPvPMainMenuManager mainMenuManager = new PvPMainMenuManager(navigationPermitterManager, pauseGameManager, modalMenu, battleCompletionHandler);
            modalMenu.Initialise(soundPlayer, applicationModel.IsTutorial, mainMenuManager, applicationModel.DataProvider.SettingsManager);
            SetupMainMenuButtons(soundPlayer, mainMenuManager);
            makeRightBackgroundPanelFit();
            return
                new PvPRightPanelComponents(
                    informator,
                    mainMenuManager,
                    modalMenu,
                    speedComponents,
                    helpButton);
        }

        private IPvPInformatorPanel SetupInformator(
            IPvPUIManager uiManager,
            IPvPCruiser playerCruiser,
            IPvPUpdater perFrameUpdater,
            IPvPUserChosenTargetHelper userChosenTargetHelper,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPSingleSoundPlayer soundPlayer)
        {
            PvPInformatorPanelController informator = GetComponentInChildren<PvPInformatorPanelController>();
            Assert.IsNotNull(informator);

            informator
                .Initialise(
                    uiManager,
                    playerCruiser,
                    perFrameUpdater,
                    userChosenTargetHelper,
                    buttonVisibilityFilters,
                    soundPlayer);

            return informator;
        }

        private PvPSpeedComponents SetupSpeedPanel(IPvPSingleSoundPlayer soundPlayer, IPvPButtonVisibilityFilters buttonVisibilityFilters)
        {
            PvPSpeedPanelController speedPanelInitialiser = GetComponentInChildren<PvPSpeedPanelController>();
            Assert.IsNotNull(speedPanelInitialiser);
            return speedPanelInitialiser.Initialise(soundPlayer, buttonVisibilityFilters.SpeedButtonsEnabledFilter);
        }

        private void SetupMainMenuButtons(IPvPSingleSoundPlayer soundPlayer, IPvPMainMenuManager mainMenuManager)
        {
            PvPMainMenuButtonController mainMenuButton = GetComponentInChildren<PvPMainMenuButtonController>();
            Assert.IsNotNull(mainMenuButton);
            mainMenuButton.Initialise(soundPlayer, mainMenuManager);

            modalMainMenuButton.Initialise(soundPlayer, mainMenuManager);
        }

        public void makeRightBackgroundPanelFit()
        {
            if ((double)Screen.width / Screen.height <= 13.0 / 8)
            {
                float scaleAmount = (float)(((double)Screen.width / Screen.height) / 1.667);
                //Debug.Log(scaleAmount);
                rectTransformThis.localScale = new Vector2(scaleAmount, scaleAmount);
            }
        }
    }
}