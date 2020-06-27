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

        public void Initialise(ITutorialArgsBase baseArgs)
        {
            Helper.AssertIsNotNull(tutorialManager, explanationPanel, highlighterInitialiser, modalMainMenuButton);
            Assert.IsNotNull(baseArgs);

            // FELIX  Only initialise if:  < level 5 && setting enabled
            if (baseArgs.AppModel.SelectedLevel > 5)
            {
                Destroy(gameObject);
                return;
            }

            explanationPanel.Initialise(baseArgs.PlayerCruiser.FactoryProvider.Sound.UISoundPlayer);
            _explanationPanelHeightManager
                = new ExplanationPanelHeightManager(
                    explanationPanel,
                    new HeightDecider());

            if (!baseArgs.AppModel.IsTutorial)
            {
                _hintManager
                    = new HintManager(
                        new BuildingMonitor(baseArgs.AICruiser),
                        new NonRepeatingHintDisplayer(
                            new HintDisplayer(explanationPanel)));

                // Destroy tutorial specific game objects
                Destroy(highlighterInitialiser.gameObject);
                Destroy(modalMainMenuButton.gameObject);
                return;
            }

            baseArgs.AppModel.DataProvider.GameModel.HasAttemptedTutorial = true;
            baseArgs.AppModel.DataProvider.SaveGame();

            ITutorialArgs tutorialArgs = new TutorialArgs(baseArgs, explanationPanel);
            tutorialManager.Initialise(tutorialArgs, highlighterInitialiser);
            tutorialManager.StartTutorial();
        }
    }
}