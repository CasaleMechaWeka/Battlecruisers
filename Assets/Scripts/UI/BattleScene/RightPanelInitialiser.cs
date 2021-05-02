using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    /// <summary>
    /// Contains:
    /// + Informator (item details)
    /// + Game speed buttons
    /// + Help button
    /// + Menu button
    /// </summary>
    public class RightPanelInitialiser : MonoBehaviour
    {
        public ModalMenuController modalMenu;
        public MainMenuButtonController modalMainMenuButton;
        public HelpButton helpButton;

        public RightPanelComponents Initialise(
            IApplicationModel applicationModel,
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IPauseGameManager pauseGameManager,
            IBattleCompletionHandler battleCompletionHandler,
            ISingleSoundPlayer soundPlayer,
            INavigationPermitterManager navigationPermitterManager)
        {
            Helper.AssertIsNotNull(modalMenu, modalMainMenuButton, helpButton);
            Helper.AssertIsNotNull(
                applicationModel, 
                uiManager, 
                playerCruiser, 
                userChosenTargetHelper, 
                buttonVisibilityFilters, 
                pauseGameManager,
                battleCompletionHandler,
                soundPlayer,
                navigationPermitterManager);

            IInformatorPanel informator = SetupInformator(uiManager, playerCruiser, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer);
            SpeedComponents speedComponents = SetupSpeedPanel(soundPlayer, buttonVisibilityFilters);
            IMainMenuManager mainMenuManager = new MainMenuManager(navigationPermitterManager, pauseGameManager, modalMenu, battleCompletionHandler);
            modalMenu.Initialise(soundPlayer, applicationModel.IsTutorial, mainMenuManager, applicationModel.DataProvider.SettingsManager);
            SetupMainMenuButtons(soundPlayer, mainMenuManager);

            return 
                new RightPanelComponents(
                    informator, 
                    mainMenuManager,
                    modalMenu,
                    speedComponents,
                    helpButton);
        }

        private IInformatorPanel SetupInformator(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            ISingleSoundPlayer soundPlayer)
        {
            InformatorPanelController informator = GetComponentInChildren<InformatorPanelController>();
            Assert.IsNotNull(informator);

            informator
                .Initialise(
                    uiManager,
                    playerCruiser,
                    userChosenTargetHelper,
                    buttonVisibilityFilters,
                    soundPlayer);

            return informator;
        }

        private SpeedComponents SetupSpeedPanel(ISingleSoundPlayer soundPlayer, IButtonVisibilityFilters buttonVisibilityFilters)
        {
            SpeedPanelController speedPanelInitialiser = GetComponentInChildren<SpeedPanelController>();
            Assert.IsNotNull(speedPanelInitialiser);
            return speedPanelInitialiser.Initialise(soundPlayer, buttonVisibilityFilters.SpeedButtonsEnabledFilter);
        }

        private void SetupMainMenuButtons(ISingleSoundPlayer soundPlayer, IMainMenuManager mainMenuManager)
        {
            MainMenuButtonController mainMenuButton = GetComponentInChildren<MainMenuButtonController>();
            Assert.IsNotNull(mainMenuButton);
            mainMenuButton.Initialise(soundPlayer, mainMenuManager);

            modalMainMenuButton.Initialise(soundPlayer, mainMenuManager);
        }
    }
}