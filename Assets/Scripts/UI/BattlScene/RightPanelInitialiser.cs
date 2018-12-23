using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Filters;
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
        // Not using FindObjectOfType() because that ignores inactive objects
        public ModalMenuController modalMenu;

        public RightPanelComponents Initialise(
            IApplicationModel applicationModel,
            ISceneNavigator sceneNavigator,
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IPauseGameManager pauseGameManager)
        {
            Helper.AssertIsNotNull(modalMenu, applicationModel, sceneNavigator, uiManager, playerCruiser, userChosenTargetHelper, buttonVisibilityFilters, pauseGameManager);

            modalMenu.Initialise(applicationModel.IsTutorial);

            IInformatorPanel informator = SetupInformator(uiManager, playerCruiser, userChosenTargetHelper, buttonVisibilityFilters);
            IMaskHighlightable speedButtonPanel = SetupSpeedPanel(buttonVisibilityFilters);
            SetupMainMenuButton(applicationModel, sceneNavigator, pauseGameManager);
            SetupHelpButton(buttonVisibilityFilters.HelpLabelsVisibilityFilter);

            return new RightPanelComponents(informator, speedButtonPanel);
        }

        private IInformatorPanel SetupInformator(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters)
        {
            InformatorPanelController informator = GetComponentInChildren<InformatorPanelController>();
            Assert.IsNotNull(informator);

            informator
                .Initialise(
                    uiManager,
                    playerCruiser,
                    userChosenTargetHelper,
                    buttonVisibilityFilters);

            return informator;
        }

        private IMaskHighlightable SetupSpeedPanel(IButtonVisibilityFilters buttonVisibilityFilters)
        {
            SpeedPanelController speedPanelInitialiser = GetComponentInChildren<SpeedPanelController>();
            Assert.IsNotNull(speedPanelInitialiser);
            return speedPanelInitialiser.Initialise(buttonVisibilityFilters.SpeedButtonsEnabledFilter);
        }

        private void SetupMainMenuButton(
            IApplicationModel applicationModel, 
            ISceneNavigator sceneNavigator,
            IPauseGameManager pauseGameManager)
        {
            IBattleCompletionHandler battleCompletionHandler = new BattleCompletionHandler(applicationModel, sceneNavigator);
            IMainMenuManager mainMenuManager = new MainMenuManager(pauseGameManager, modalMenu, battleCompletionHandler);

            MainMenuButtonController mainMenuButton = GetComponentInChildren<MainMenuButtonController>();
            Assert.IsNotNull(mainMenuButton);
            mainMenuButton.Initialise(mainMenuManager);
        }

        private void SetupHelpButton(BroadcastingFilter helpLabelsVisibilityFilter)
        {
            HelpButton helpButton = GetComponentInChildren<HelpButton>();
            Assert.IsNotNull(helpButton);
            helpButton.Initialise(helpLabelsVisibilityFilter);
        }
    }
}