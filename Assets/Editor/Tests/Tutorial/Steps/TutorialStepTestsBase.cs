using BattleCruisers.Tutorial;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Tutorial.Steps
{
    public abstract class TutorialStepTestsBase
    {
        protected ITutorialStepArgs _args;
        protected IHighlighter _highlighter;
        protected string _textToDisplay;
        protected ITextDisplayer _displayer;
        protected IListProvider<IHighlightable> _highlightablesProvider;
        protected IList<IHighlightable> _highlightables;
        protected Action _completionCallback;
        protected int _callbackCounter;

        [SetUp]
        public virtual void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _highlighter = Substitute.For<IHighlighter>();
            _textToDisplay = "Staub";
            _displayer = Substitute.For<ITextDisplayer>();

            _highlightablesProvider = Substitute.For<IListProvider<IHighlightable>>();
            _highlightables = new List<IHighlightable>()
            {
                Substitute.For<IHighlightable>(),
                Substitute.For<IHighlightable>()
            };
            _highlightablesProvider.FindItems().Returns(_highlightables);

            _args = new TutorialStepArgs(_highlighter, _textToDisplay, _displayer, _highlightablesProvider);

            _completionCallback = () => _callbackCounter++;
            _callbackCounter = 0;
        }
    }
}
