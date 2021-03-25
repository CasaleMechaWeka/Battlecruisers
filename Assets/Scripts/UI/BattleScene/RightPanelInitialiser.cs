using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Localisation;
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
        public HelpLabel helpLabels;

        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private FilterToggler _helpLabelsVisibilityToggler;
#pragma warning restore CS0414  // Variable is assigned but never used

        public RightPanelComponents Initialise(
            IApplicationModel applicationModel,
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IPauseGameManager pauseGameManager,
            IBattleCompletionHandler battleCompletionHandler,
            ISingleSoundPlayer soundPlayer,
            INavigationPermitterManager navigationPermitterManager,
            ILocTable commonStrings)
        {
            Helper.AssertIsNotNull(modalMenu, modalMainMenuButton, helpLabels);
            Helper.AssertIsNotNull(
                applicationModel, 
                uiManager, 
                playerCruiser, 
                userChosenTargetHelper, 
                buttonVisibilityFilters, 
                pauseGameManager,
                battleCompletionHandler,
                soundPlayer,
                navigationPermitterManager,
                commonStrings);

            IInformatorPanel informator = SetupInformator(uiManager, playerCruiser, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer, commonStrings);
            SpeedComponents speedComponents = SetupSpeedPanel(soundPlayer, buttonVisibilityFilters);
            IMainMenuManager mainMenuManager = new MainMenuManager(pauseGameManager, modalMenu, battleCompletionHandler, navigationPermitterManager);
            modalMenu.Initialise(soundPlayer, applicationModel.IsTutorial, mainMenuManager, applicationModel.DataProvider.SettingsManager);
            SetupMainMenuButtons(soundPlayer, mainMenuManager);
            SetupHelpButton(soundPlayer, buttonVisibilityFilters.HelpLabelsVisibilityFilter);
            SetupHelpLabels(buttonVisibilityFilters.HelpLabelsVisibilityFilter);

            return 
                new RightPanelComponents(
                    informator, 
                    mainMenuManager,
                    modalMenu,
                    speedComponents);
        }

        private IInformatorPanel SetupInformator(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            ISingleSoundPlayer soundPlayer,
            ILocTable commonStrings)
        {
            InformatorPanelController informator = GetComponentInChildren<InformatorPanelController>();
            Assert.IsNotNull(informator);

            informator
                .Initialise(
                    uiManager,
                    playerCruiser,
                    userChosenTargetHelper,
                    buttonVisibilityFilters,
                    soundPlayer,
                    commonStrings);

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

        private void SetupHelpButton(ISingleSoundPlayer soundPlayer, BroadcastingFilter helpLabelsVisibilityFilter)
        {
            HelpButton helpButton = GetComponentInChildren<HelpButton>();
            Assert.IsNotNull(helpButton);
            helpButton.Initialise(soundPlayer, helpLabelsVisibilityFilter);
        }

        private void SetupHelpLabels(IBroadcastingFilter helpLabelsVisibilityFilter)
        {
            helpLabels.Initialise();
            _helpLabelsVisibilityToggler = new FilterToggler(helpLabelsVisibilityFilter, helpLabels);
        }
    }
}