using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.Factories;
using BattleCruisers.Utils.BattleScene;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialManager : MonoBehaviour, ITutorialManager
    {
        private ITutorialStepConsumer _consumer;
        private IExplanationPanel _explanationPanel;
        private ExplanationPanelHeightManager _explanationPanelHeightManager;
        private IGameEndMonitor _gameEndMonitor;

        public HighlighterInitialiser highlighterInitialiser;

        public void Initialise(ITutorialArgs tutorialArgs)
        {
            Assert.IsNotNull(highlighterInitialiser);
            Assert.IsNotNull(tutorialArgs);

            IMaskHighlighter maskHighlighter = highlighterInitialiser.CreateHighlighter(tutorialArgs.CameraComponents.MainCamera);
            IHighlighter highlighter
                = new Highlighter(
                    maskHighlighter,
                    new HighlightArgsFactory(tutorialArgs.CameraComponents.MainCamera));

            ExplanationPanel explanationPanel = GetComponentInChildren<ExplanationPanel>(includeInactive: true);
            Assert.IsNotNull(explanationPanel);
            explanationPanel.Initialise(tutorialArgs.PlayerCruiser.FactoryProvider.Sound.SoundPlayer);
            _explanationPanel = explanationPanel;
            _explanationPanelHeightManager 
                = new ExplanationPanelHeightManager(
                    _explanationPanel,
                    new HeightDecider());

            ITutorialStepsFactory stepsFactory 
                = new MasterTutorialStepsFactory(
                    highlighter, 
                    explanationPanel, 
                    tutorialArgs.Components.Deferrer, 
                    tutorialArgs);

            Queue<ITutorialStep> steps = new Queue<ITutorialStep>(stepsFactory.CreateSteps());
            _consumer = new TutorialStepConsumer(steps);
            _consumer.Completed += _consumer_Completed;

            _gameEndMonitor = tutorialArgs.GameEndMonitor;
            _gameEndMonitor.GameEnded += GameEndMonitor_GameEnded;
        }

        private void _consumer_Completed(object sender, EventArgs e)
        {
            _consumer.Completed -= _consumer_Completed;
            _explanationPanel.IsVisible = false;
        }

        private void GameEndMonitor_GameEnded(object sender, EventArgs e)
        {
            _gameEndMonitor.GameEnded -= GameEndMonitor_GameEnded;
            _explanationPanel.IsVisible = false;
        }

        public void StartTutorial()
        {
            _consumer.StartConsuming();
        }
    }
}
