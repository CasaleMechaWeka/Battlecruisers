using System;
using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial
{
    public class TutorialManager : ITutorialManager
    {
        private readonly ITutorialStepConsumer _consumer;

        public event EventHandler TutorialCompleted;

        public TutorialManager(
            ITextDisplayer textDisplayer,
            ICruiser playerCruiser)
        {
            Helper.AssertIsNotNull(textDisplayer, playerCruiser);

            IHighlighter highlighter = new Highlighter(new HighlightFactory());

            ITutorialStepsFactory stepsFactory = new TutorialStepsFactory(highlighter, textDisplayer, playerCruiser);
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
