using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.InGameHints;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Threading.Tasks;
using UnityEngine;

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

        public async Task InitialiseAsync(
            ITutorialArgsBase baseArgs, 
            bool showInGameHints, 
            ICruiserDamageMonitor playerCruiserDamageMonitor,
            ILocTable commonStrings)
        {
            Helper.AssertIsNotNull(tutorialManager, explanationPanel, highlighterInitialiser, modalMainMenuButton);
            Helper.AssertIsNotNull(baseArgs, playerCruiserDamageMonitor, commonStrings);

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

                ILocTable tutorialStrings = await LocTableFactory.Instance.LoadTutorialTable();

                ITutorialArgs tutorialArgs = new TutorialArgs(baseArgs, explanationPanel, modalMainMenuButton, tutorialStrings, commonStrings);
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