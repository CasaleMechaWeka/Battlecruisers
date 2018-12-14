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

            ITutorialStepsFactory stepsFactory 
                = new MasterTutorialStepsFactory(
                    highlighter, 
                    explanationPanel, 
                    tutorialArgs.Components.VariableDelayDeferrer, 
                    tutorialArgs);

            Queue<ITutorialStep> steps = new Queue<ITutorialStep>(stepsFactory.CreateTutorialSteps());
            _consumer = new TutorialStepConsumer(steps);

            _consumer.Completed += _consumer_Completed;
        }

        public void StartTutorial()
        {
            _consumer.StartConsuming();
        }

        private void _consumer_Completed(object sender, EventArgs e)
        {
            _consumer.Completed -= _consumer_Completed;

            if (TutorialCompleted != null)
            {
                TutorialCompleted.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
