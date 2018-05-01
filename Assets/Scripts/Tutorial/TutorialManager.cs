using System;
using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialManager : MonoBehaviour, ITutorialManager
    {
		private IHighlightFactory _highlightFactory;
        private ITutorialStepConsumer _consumer;

        public TextDisplayer textDisplayer;

        public event EventHandler TutorialCompleted;

        public void Initialise(ICruiser playerCruiser)
        {
            // FELIX  Will be passing a whole lot more, so can keep multi assert :P
            Helper.AssertIsNotNull(textDisplayer, playerCruiser);

            textDisplayer.Initialise();

            _highlightFactory = GetComponent<IHighlightFactory>();
            Assert.IsNotNull(_highlightFactory);

            IHighlighter highlighter = new Highlighter(_highlightFactory);

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
