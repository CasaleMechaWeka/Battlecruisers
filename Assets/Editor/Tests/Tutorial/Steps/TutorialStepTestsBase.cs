using BattleCruisers.Tutorial;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Tutorial.Steps
{
    public abstract class TutorialStepTestsBase
    {
        protected ITutorialStepArgsNEW _args;
        protected IHighlighter _highlighter;
        protected string _textToDisplay;
        protected ITextDisplayer _displayer;
        protected IItemProvider<IMaskHighlightable> _highlightableProvider;
        protected IMaskHighlightable _highlightable;
        protected Action _completionCallback;
        protected int _callbackCounter;

        [SetUp]
        public virtual void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _highlighter = Substitute.For<IHighlighter>();
            _textToDisplay = "Staub";
            _displayer = Substitute.For<ITextDisplayer>();

            _highlightable = Substitute.For<IMaskHighlightable>();
            _highlightableProvider = Substitute.For<IItemProvider<IMaskHighlightable>>();
            _highlightableProvider.FindItem().Returns(_highlightable);

            _args = new TutorialStepArgsNEW(_highlighter, _textToDisplay, _displayer, _highlightableProvider);

            _completionCallback = () => _callbackCounter++;
            _callbackCounter = 0;
        }
    }
}
