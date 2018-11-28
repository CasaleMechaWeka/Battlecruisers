using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Steps;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialManagerNEW : MonoBehaviour, ITutorialManager
    {
        private ITutorialStepConsumer _consumer;

        public event EventHandler TutorialCompleted;

        public void Initialise(ITutorialArgsNEW tutorialArgs)
        {
            Assert.IsNotNull(tutorialArgs);

            MaskHighlighter maskHighlighter = GetComponentInChildren<MaskHighlighter>(includeInactive: true);
            Assert.IsNotNull(maskHighlighter);
            maskHighlighter.Initialise();

            IHighlighterNEW highlighter
                = new HighlighterNEW(
                    maskHighlighter,
                    new HighlightArgsFactory(tutorialArgs.Components.Camera));

            TextDisplayer textDisplayer = GetComponentInChildren<TextDisplayer>(includeInactive: true);
            Assert.IsNotNull(textDisplayer);
            textDisplayer.Initialise();

            ExplanationControl explanationControl = GetComponentInChildren<ExplanationControl>(includeInactive: true);
            Assert.IsNotNull(explanationControl);
            explanationControl.Initialise();

            ITutorialStepsFactory stepsFactory 
                = new TutorialStepsFactoryNEW(
                    highlighter, 
                    textDisplayer, 
                    tutorialArgs.Components.VariableDelayDeferrer, 
                    tutorialArgs);
            Queue<ITutorialStep> steps = stepsFactory.CreateTutorialSteps();
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
