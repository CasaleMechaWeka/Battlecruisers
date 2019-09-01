using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
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
            ISoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(
                modalMenu, 
                modalMainMenuButton,
                applicationModel, 
                uiManager, 
                playerCruiser, 
                userChosenTargetHelper, 
                buttonVisibilityFilters, 
                pauseGameManager,
                battleCompletionHandler,
                soundPlayer);

            modalMenu.Initialise(soundPlayer, applicationModel.IsTutorial);

            IInformatorPanel informator = SetupInformator(uiManager, playerCruiser, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer);
            IMaskHighlightable speedButtonPanel = SetupSpeedPanel(soundPlayer, buttonVisibilityFilters);
            SetupMainMenuButtons(soundPlayer, pauseGameManager, battleCompletionHandler);
            SetupHelpButton(soundPlayer, buttonVisibilityFilters.HelpLabelsVisibilityFilter);
            SetupHelpLabels(buttonVisibilityFilters.HelpLabelsVisibilityFilter);

            return new RightPanelComponents(informator, speedButtonPanel);
        }

        private IInformatorPanel SetupInformator(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            ISoundPlayer soundPlayer)
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

        private IMaskHighlightable SetupSpeedPanel(ISoundPlayer soundPlayer, IButtonVisibilityFilters buttonVisibilityFilters)
        {
            SpeedPanelController speedPanelInitialiser = GetComponentInChildren<SpeedPanelController>();
            Assert.IsNotNull(speedPanelInitialiser);
            return speedPanelInitialiser.Initialise(soundPlayer, buttonVisibilityFilters.SpeedButtonsEnabledFilter);
        }

        private void SetupMainMenuButtons(ISoundPlayer soundPlayer, IPauseGameManager pauseGameManager, IBattleCompletionHandler battleCompletionHandler)
        {
            IMainMenuManager mainMenuManager = new MainMenuManager(pauseGameManager, modalMenu, battleCompletionHandler);

            MainMenuButtonController mainMenuButton = GetComponentInChildren<MainMenuButtonController>();
            Assert.IsNotNull(mainMenuButton);
            mainMenuButton.Initialise(soundPlayer, mainMenuManager);

            modalMainMenuButton.Initialise(soundPlayer, mainMenuManager);
        }

        private void SetupHelpButton(ISoundPlayer soundPlayer, BroadcastingFilter helpLabelsVisibilityFilter)
        {
            HelpButton helpButton = GetComponentInChildren<HelpButton>();
            Assert.IsNotNull(helpButton);
            helpButton.Initialise(soundPlayer, helpLabelsVisibilityFilter);
        }

        private void SetupHelpLabels(IBroadcastingFilter helpLabelsVisibilityFilter)
        {
            HelpLabel helpLabels = GetComponentInChildren<HelpLabel>();
            Assert.IsNotNull(helpLabels);
            helpLabels.Initialise();
            _helpLabelsVisibilityToggler = new FilterToggler(helpLabels, helpLabelsVisibilityFilter);
        }
    }
}