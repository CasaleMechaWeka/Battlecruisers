using System;
using System.Collections.Generic;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Tutorial.Steps
{
    public class DummyTutorialStep : TutorialStep
    {
        public DummyTutorialStep(ITutorialStepArgs args) : base(args)
        {
        }

        public void Complete()
        {
            base.OnCompleted();
        }
    }

    public class TutorialStepTests
    {
        private DummyTutorialStep _tutorialStep;

        private IHighlighter _highlighter;
        private string _textToDisplay;
        private ITextDisplayer _displayer;
        private IHighlightablesProvider _highlightablesProvider;
        private IList<IHighlightable> _highlightables;
        private Action _completionCallback;
        private int _callbackCounter;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _highlighter = Substitute.For<IHighlighter>();
            _textToDisplay = "Staub";
            _displayer = Substitute.For<ITextDisplayer>();

            _highlightablesProvider = Substitute.For<IHighlightablesProvider>();
            _highlightables = new List<IHighlightable>()
            {
                Substitute.For<IHighlightable>(),
                Substitute.For<IHighlightable>()
            };
            _highlightablesProvider.FindHighlightables().Returns(_highlightables);

            ITutorialStepArgs args = new TutorialStepArgs(_highlighter, _textToDisplay, _displayer, _highlightablesProvider);
            _tutorialStep = new DummyTutorialStep(args);

            _completionCallback = () => _callbackCounter++;
            _callbackCounter = 0;
        }

        #region Start
        [Test]
        public void Start_NonNullText()
        {
            _tutorialStep.Start(_completionCallback);

            _highlightablesProvider.Received().FindHighlightables();
            _highlighter.Received().Highlight(_highlightables);
            _displayer.Received().DisplayText(_textToDisplay);
        }

        [Test]
        public void Start_NullText_DoesNotDisplayText()
        {
            _textToDisplay = null;
            ITutorialStepArgs args = new TutorialStepArgs(_highlighter, _textToDisplay, _displayer, _highlightablesProvider);
            _tutorialStep = new DummyTutorialStep(args);

            _tutorialStep.Start(_completionCallback);

            _highlightablesProvider.Received().FindHighlightables();
            _highlighter.Received().Highlight(_highlightables);
            _displayer.DidNotReceiveWithAnyArgs().DisplayText(null);
        }

        [Test]
        public void DoubleStart_Throws()
        {
            _tutorialStep.Start(_completionCallback);
            Assert.Throws<UnityAsserts.AssertionException>(() => _tutorialStep.Start(_completionCallback));
        }
        #endregion Start

        #region OnCompleted
        [Test]
        public void OnCompleted_Unhighlights_CallsCompletionCallback()
        {
            _tutorialStep.Start(_completionCallback);
            _tutorialStep.Complete();

            _highlighter.Received().UnhighlightAll();
            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void OnCompleted_NoPreviousStart_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _tutorialStep.Complete());
        }
        #endregion OnCompleted
    }
}
