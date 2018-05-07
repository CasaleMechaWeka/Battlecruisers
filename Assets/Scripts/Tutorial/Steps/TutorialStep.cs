using System;
using BattleCruisers.Tutorial.Highlighting;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    // FELIX  Test :D
    public abstract class TutorialStep : ITutorialStep
    {
        private readonly IHighlighter _highlighter;
        private readonly string _textToDisplay;
        private readonly ITextDisplayer _displayer;
        private readonly IHighlightablesProvider _highlightablesProvider;
        private Action _completionCallback;

        protected TutorialStep(ITutorialStepArgs args)
        {
            Assert.IsNotNull(args);

            _highlighter = args.Highlighter;
            _textToDisplay = args.TextToDisplay;
            _displayer = args.Displayer;
            _highlightablesProvider = args.HighlightablesProvider;
        }

        public virtual void Start(Action completionCallback)
        {
            Assert.IsNull(_completionCallback, "Start(...) should only be called once.");
            Assert.IsNotNull(completionCallback);
            _completionCallback = completionCallback;

            _highlighter.Highlight(_highlightablesProvider.FindHighlightables());

            if (_textToDisplay != null)
            {
                _displayer.DisplayText(_textToDisplay);
			}
        }

        protected virtual void OnCompleted()
        {
            Assert.IsNotNull(_completionCallback, "OnCompleted() should not be called before Start()");

            _highlighter.UnhighlightAll();
            _completionCallback.Invoke();
        }
    }
}
