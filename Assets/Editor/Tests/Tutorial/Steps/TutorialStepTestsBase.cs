using BattleCruisers.Tutorial;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Tutorial.Steps
{
    public abstract class TutorialStepTestsBase
    {
        protected ITutorialStepArgs _args;
        protected IHighlighter _highlighter;
        protected string _textToDisplay;
        protected ITextDisplayer _displayer;
        protected IItemProvider<IHighlightable> _highlightableProvider;
        protected IHighlightable _highlightable;
        protected Action _completionCallback;
        protected int _callbackCounter;

        [SetUp]
        public virtual void SetuUp()
        {
            _highlighter = Substitute.For<IHighlighter>();
            _textToDisplay = "Staub";
            _displayer = Substitute.For<ITextDisplayer>();

            _highlightable = Substitute.For<IHighlightable>();
            _highlightableProvider = Substitute.For<IItemProvider<IHighlightable>>();
            _highlightableProvider.FindItem().Returns(_highlightable);

            _args = new TutorialStepArgs(_highlighter, _textToDisplay, _displayer, _highlightableProvider, shouldUnhighlight: true);

            _completionCallback = () => _callbackCounter++;
            _callbackCounter = 0;
        }
    }
}
