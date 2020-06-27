using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.Factories;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Tutorial
{
    public class TutorialManager : MonoBehaviour, ITutorialManager
    {
        private ITutorialStepConsumer _consumer;
        private IExplanationPanel _explanationPanel;
        private IGameEndMonitor _gameEndMonitor;

        public void Initialise(ITutorialArgs tutorialArgs, HighlighterInitialiser highlighterInitialiser)
        {
            Helper.AssertIsNotNull(tutorialArgs, highlighterInitialiser);

            _explanationPanel = tutorialArgs.ExplanationPanel;

            ICoreHighlighter coreHighlighter = highlighterInitialiser.CreateHighlighter(tutorialArgs.CameraComponents.MainCamera);
            IHighlighter highlighter
                = new Highlighter(
                    coreHighlighter,
                    new HighlightArgsFactory(tutorialArgs.CameraComponents.MainCamera));

            ITutorialStepsFactory stepsFactory 
                = new MasterTutorialStepsFactory(
                    highlighter, 
                    _explanationPanel, 
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
