using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.Factories;
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

        public event EventHandler TutorialCompleted;

        public void Initialise(ITutorialArgs tutorialArgs)
        {
            Assert.IsNotNull(tutorialArgs);

            MaskHighlighter maskHighlighter = GetComponentInChildren<MaskHighlighter>(includeInactive: true);
            Assert.IsNotNull(maskHighlighter);
            maskHighlighter.Initialise();

            IHighlighter highlighter
                = new Highlighter(
                    maskHighlighter,
                    new HighlightArgsFactory(tutorialArgs.Components.Camera));

            ExplanationPanel explanationPanel = GetComponentInChildren<ExplanationPanel>(includeInactive: true);
            Assert.IsNotNull(explanationPanel);
            explanationPanel.Initialise();
            _explanationPanel = explanationPanel;

            ITutorialStepsFactory stepsFactory 
                = new MasterTutorialStepsFactory(
                    highlighter, 
                    explanationPanel, 
                    tutorialArgs.Components.Deferrer, 
                    tutorialArgs);

            Queue<ITutorialStep> steps = new Queue<ITutorialStep>(stepsFactory.CreateSteps());
            _consumer = new TutorialStepConsumer(steps);

            _consumer.Completed += _consumer_Completed;

            tutorialArgs.BattleCompletionHandler.BattleCompleted += BattleCompletionHandler_BattleCompleted;
        }

        private void _consumer_Completed(object sender, EventArgs e)
        {
            _consumer.Completed -= _consumer_Completed;

            TutorialCompleted?.Invoke(this, EventArgs.Empty);
        }

        private void BattleCompletionHandler_BattleCompleted(object sender, EventArgs e)
        {
            _explanationPanel.IsVisible = false;
        }

        public void StartTutorial()
        {
            _consumer.StartConsuming();
        }
    }
}
