using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    public abstract class TutorialStep : ITutorialStep
    {
        private readonly IHighlighter _highlighter;
        private readonly string _textToDisplay;
        private readonly ITextDisplayer _displayer;
        private readonly IItemProvider<IHighlightable> _highlightableProvider;
        private Action _completionCallback;

        protected TutorialStep(ITutorialStepArgs args)
        {
            Assert.IsNotNull(args);

            _highlighter = args.Highlighter;
            _textToDisplay = args.TextToDisplay;
            _displayer = args.Displayer;
            _highlightableProvider = args.HighlightableProvider;
        }

        public virtual void Start(Action completionCallback)
        {
            Assert.IsNull(_completionCallback, "Start(...) should only be called once.");
            Assert.IsNotNull(completionCallback);
            _completionCallback = completionCallback;

            IHighlightable maskHighlightable = _highlightableProvider.FindItem();
            if (maskHighlightable != null)
            {
                _highlighter.Highlight(maskHighlightable);
            }

            if (_textToDisplay != null)
            {
                _displayer.DisplayText(_textToDisplay);
            }
        }

        protected virtual void OnCompleted()
        {
            Assert.IsNotNull(_completionCallback, "OnCompleted() should not be called before Start(), or more than once.");

            _highlighter.Unhighlight();
            _completionCallback.Invoke();
            _completionCallback = null;
        }
    }
}
