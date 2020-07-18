using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.InGameHints;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialInitialiser : MonoBehaviour
    {
        private ExplanationPanelHeightManager _explanationPanelHeightManager;
        private HintManager _hintManager;

        public TutorialManager tutorialManager;
        public ExplanationPanel explanationPanel;
        public HighlighterInitialiser highlighterInitialiser;
        public MainMenuButtonController modalMainMenuButton;

        public void Initialise(ITutorialArgsBase baseArgs, bool showInGameHints, ICruiserDamageMonitor playerCruiserDamageMonitor)
        {
            Helper.AssertIsNotNull(tutorialManager, explanationPanel, highlighterInitialiser, modalMainMenuButton);
            Helper.AssertIsNotNull(baseArgs, playerCruiserDamageMonitor);

            if (!baseArgs.AppModel.IsTutorial
                && !showInGameHints)
            {
                Destroy(gameObject);
                return;
            }

            explanationPanel.Initialise(baseArgs.PlayerCruiser.FactoryProvider.Sound.UISoundPlayer);
            _explanationPanelHeightManager
                = new ExplanationPanelHeightManager(
                    explanationPanel,
                    new HeightDecider());

            if (baseArgs.AppModel.IsTutorial)
            {
                baseArgs.AppModel.DataProvider.GameModel.HasAttemptedTutorial = true;
                baseArgs.AppModel.DataProvider.SaveGame();

                ITutorialArgs tutorialArgs = new TutorialArgs(baseArgs, explanationPanel);
                tutorialManager.Initialise(tutorialArgs, highlighterInitialiser);
                tutorialManager.StartTutorial();
            }
            else
            {
                _hintManager
                    = new HintManager(
                        new BuildingMonitor(baseArgs.AICruiser),
                        new BuildingMonitor(baseArgs.PlayerCruiser),
                        new FactoryMonitor(baseArgs.PlayerCruiser.BuildingMonitor),
                        playerCruiserDamageMonitor,
                        baseArgs.PlayerCruiser.DroneFocuser,
                        baseArgs.GameEndMonitor,
                        new NonRepeatingHintDisplayer(
                            new HintDisplayer(explanationPanel)));

                // Destroy tutorial specific game objects
                Destroy(highlighterInitialiser.gameObject);
                Destroy(modalMainMenuButton.gameObject);
            }
        }
    }
}