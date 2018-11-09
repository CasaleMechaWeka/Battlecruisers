using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.PlatformAbstractions;
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

        public void Initialise(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IBattleCompletionHandler battleCompletionHandler)
        {
            Helper.AssertIsNotNull(uiManager, playerCruiser, userChosenTargetHelper, buttonVisibilityFilters, battleCompletionHandler);

            SetupInformator(uiManager, playerCruiser, userChosenTargetHelper, buttonVisibilityFilters);
            SetupSpeedPanel();
            // FELIX  Setup help button
            SetupMainMenuButton(battleCompletionHandler);
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

        private void SetupSpeedPanel()
        {
            SpeedPanelController speedPanelInitialiser = FindObjectOfType<SpeedPanelController>();
            Assert.IsNotNull(speedPanelInitialiser);
            speedPanelInitialiser.Initialise();
        }

        private void SetupMainMenuButton(IBattleCompletionHandler battleCompletionHandler)
        {
            ModalMenuController modalMenu = FindObjectOfType<ModalMenuController>();
            Assert.IsNotNull(modalMenu);
            IPauseGameManager pauseGameManager = new PauseGameManager(new TimeBC());
            IMainMenuManager mainMenuManager = new MainMenuManager(pauseGameManager, modalMenu, battleCompletionHandler);

            MainMenuButtonController mainMenuButton = FindObjectOfType<MainMenuButtonController>();
            Assert.IsNotNull(mainMenuButton);
            mainMenuButton.Initialise(mainMenuManager);
        }
    }
}