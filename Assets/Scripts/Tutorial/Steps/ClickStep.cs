using System;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    /// <summary>
    /// Completed when the user clicks on a specific element.
    /// </summary>
    // FELIX  Test :D
    public class ClickStep : ITutorialStep
    {
        private readonly IHighlightable[] _elementsToHighlight;
        private readonly IHighlighter _highlighter;
        private readonly string _textToDisplay;
        private readonly ITextDisplayer _displayer;
        private readonly IClickable _completionClickable;
        private Action _completionCallback;

        private const int MIN_NUM_OF_HIGHLIGHTABLES = 1;

        public ClickStep(
            IHighlighter highlighter,
            string textToDisplay,
            ITextDisplayer displayer,
            IClickable completionClickable,
            params IHighlightable[] elementsToHighlight)
        {
            Helper.AssertIsNotNull(highlighter, textToDisplay, displayer, completionClickable, elementsToHighlight);
            Assert.IsTrue(elementsToHighlight.Length >= MIN_NUM_OF_HIGHLIGHTABLES);

            _highlighter = highlighter;
            _textToDisplay = textToDisplay;
            _displayer = displayer;
            _completionClickable = completionClickable;
            _elementsToHighlight = elementsToHighlight;
        }

        public void Start(Action completionCallback)
        {
            Assert.IsNotNull(completionCallback);
            _completionCallback = completionCallback;

            _completionClickable.Clicked += _completionClickable_Clicked;

            _highlighter.Highlight(_elementsToHighlight);
            _displayer.DisplayText(_textToDisplay);
        }

        private void _completionClickable_Clicked(object sender, EventArgs e)
        {
            _completionClickable.Clicked -= _completionClickable_Clicked;

            _highlighter.UnhighlightAll();
            _completionCallback.Invoke();
        }
    }
}
