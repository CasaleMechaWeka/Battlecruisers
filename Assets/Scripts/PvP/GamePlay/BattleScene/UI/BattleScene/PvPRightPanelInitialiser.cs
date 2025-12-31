using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Update;
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
        public PvPHecklePanelController heckleController;

        public PvPRightPanelComponents Initialise(
            PvPUIManager uiManager,
            IPvPCruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            PvPButtonVisibilityFilters buttonVisibilityFilters,
            IUpdater perFrameUpdater,
            PauseGameManager pauseGameManager,
            PvPBattleCompletionHandler battleCompletionHandler,
            SingleSoundPlayer soundPlayer,
            NavigationPermitterManager navigationPermitterManager
            )
        {
            PvPHelper.AssertIsNotNull(modalMenu, modalMainMenuButton, helpButton);
            PvPHelper.AssertIsNotNull(
                uiManager,
                playerCruiser,
                userChosenTargetHelper,
                buttonVisibilityFilters,
                perFrameUpdater,
                battleCompletionHandler,
                soundPlayer,
                navigationPermitterManager
                );

            PvPInformatorPanelController informator = SetupInformator(uiManager, playerCruiser, perFrameUpdater, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer);
            heckleController.Initialise(soundPlayer, uiManager);
            uiManager.SetHecklePanel(heckleController);
            //    PvPSpeedComponents speedComponents = SetupSpeedPanel(soundPlayer, buttonVisibilityFilters);
            IMainMenuManager mainMenuManager = new PvPMainMenuManager(navigationPermitterManager, pauseGameManager, modalMenu, battleCompletionHandler);
            modalMenu.Initialise(soundPlayer, ApplicationModel.IsTutorial, mainMenuManager, DataProvider.SettingsManager);
            SetupMainMenuButtons(soundPlayer, mainMenuManager);
            MakeRightBackgroundPanelFit();
            return
                new PvPRightPanelComponents(
                    informator,
                    mainMenuManager,
                    modalMenu,
                    heckleController,
                    //  speedComponents,
                    helpButton);
        }

        private PvPInformatorPanelController SetupInformator(
            PvPUIManager uiManager,
            IPvPCruiser playerCruiser,
            IUpdater perFrameUpdater,
            IUserChosenTargetHelper userChosenTargetHelper,
            PvPButtonVisibilityFilters buttonVisibilityFilters,
            SingleSoundPlayer soundPlayer)
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

        private PvPSpeedComponents SetupSpeedPanel(SingleSoundPlayer soundPlayer, PvPButtonVisibilityFilters buttonVisibilityFilters)
        {
            PvPSpeedPanelController speedPanelInitialiser = GetComponentInChildren<PvPSpeedPanelController>();
            Assert.IsNotNull(speedPanelInitialiser);
            return speedPanelInitialiser.Initialise(soundPlayer, buttonVisibilityFilters.SpeedButtonsEnabledFilter);
        }

        private void SetupMainMenuButtons(SingleSoundPlayer soundPlayer, IMainMenuManager mainMenuManager)
        {
            PvPMainMenuButtonController mainMenuButton = GetComponentInChildren<PvPMainMenuButtonController>();
            Assert.IsNotNull(mainMenuButton);
            mainMenuButton.Initialise(soundPlayer, mainMenuManager);

            modalMainMenuButton.Initialise(soundPlayer, mainMenuManager);
        }

        public void MakeRightBackgroundPanelFit()
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