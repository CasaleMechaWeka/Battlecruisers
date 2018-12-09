using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails;
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
        // Circular dependency between UIManager and InformatorPanelController.
        private InformatorPanelController _informator;
        public InformatorPanelController Informator
        {
            get
            {
                if (_informator == null)
                {
                    _informator = FindObjectOfType<InformatorPanelController>();
                    Assert.IsNotNull(_informator);
                    _informator.StaticInitialise();
                }

                return _informator;
            }
        }

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

            SetupInformator(uiManager, playerCruiser, userChosenTargetHelper, buttonVisibilityFilters);
            IMaskHighlightable speedButtonPanel = SetupSpeedPanel(buttonVisibilityFilters);
            SetupMainMenuButton(applicationModel, sceneNavigator, pauseGameManager);

            return new RightPanelComponents(Informator, speedButtonPanel);
        }

        private void SetupInformator(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters)
        {
            Informator
                .Initialise(
                    uiManager,
                    playerCruiser,
                    userChosenTargetHelper,
                    buttonVisibilityFilters);
        }

        private IMaskHighlightable SetupSpeedPanel(IButtonVisibilityFilters buttonVisibilityFilters)
        {
            SpeedPanelController speedPanelInitialiser = FindObjectOfType<SpeedPanelController>();
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

            MainMenuButtonController mainMenuButton = FindObjectOfType<MainMenuButtonController>();
            Assert.IsNotNull(mainMenuButton);
            mainMenuButton.Initialise(mainMenuManager);
        }
    }
}