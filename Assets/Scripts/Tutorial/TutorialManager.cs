using System;
using System.Collections.Generic;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialManager : MonoBehaviour, ITutorialManager
    {
		private IVariableDelayDeferrer _deferrer;
        private IHighlightFactory _highlightFactory;
        private ITutorialStepConsumer _consumer;

        public TextDisplayer textDisplayer;
        public MaskHighlighter maskHighlighter;

        public event EventHandler TutorialCompleted;

        public void Initialise(ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(tutorialArgs, textDisplayer, maskHighlighter);

            textDisplayer.Initialise();
            maskHighlighter.Initialise();

            _deferrer = GetComponent<IVariableDelayDeferrer>();
            Assert.IsNotNull(_deferrer);

            _highlightFactory = GetComponent<IHighlightFactory>();
            Assert.IsNotNull(_highlightFactory);

            IHighlighter highlighter = new Highlighter(new HighlightHelper(_highlightFactory));

            // FELIX  Delete whole class :P
            //ITutorialStepsFactory stepsFactory = new TutorialStepsFactory(highlighter, textDisplayer, _deferrer, tutorialArgs);
            //Queue<ITutorialStep> steps = stepsFactory.CreateTutorialSteps();
            //_consumer = new TutorialStepConsumer(steps);

            //_consumer.Completed += _consumer_Completed;
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
