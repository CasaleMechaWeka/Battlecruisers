using System;
using BattleCruisers.Tutorial.Highlighting;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    // FELIX  Test :D
    public abstract class TutorialStep : ITutorialStep
    {
        private readonly IHighlightable[] _elementsToHighlight;
        private readonly IHighlighter _highlighter;
        private readonly string _textToDisplay;
        private readonly ITextDisplayer _displayer;
        private Action _completionCallback;

        protected TutorialStep(ITutorialStepArgs args)
        {
            Assert.IsNotNull(args);

            _highlighter = args.Highlighter;
            _textToDisplay = args.TextToDisplay;
            _displayer = args.Displayer;
            _elementsToHighlight = args.ElementsToHighlight;
        }

        public virtual void Start(Action completionCallback)
        {
            Assert.IsNotNull(completionCallback);
            _completionCallback = completionCallback;

            _highlighter.Highlight(_elementsToHighlight);

            if (_textToDisplay != null)
            {
                _displayer.DisplayText(_textToDisplay);
			}
        }

        protected virtual void OnCompleted()
        {
            _highlighter.UnhighlightAll();
            _completionCallback.Invoke();
        }
    }
}
