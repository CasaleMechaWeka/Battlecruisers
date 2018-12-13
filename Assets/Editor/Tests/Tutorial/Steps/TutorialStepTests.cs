using BattleCruisers.Tutorial.Highlighting.Masked;
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

    public class TutorialStepTests : TutorialStepTestsBase
    {
        private DummyTutorialStep _tutorialStep;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();
            _tutorialStep = new DummyTutorialStep(_args);
        }

        #region Start
        [Test]
        public void Start_NonNullText()
        {
            _tutorialStep.Start(_completionCallback);

            _highlightableProvider.Received().FindItem();
            _highlighter.Received().Highlight(_highlightable);
            _displayer.Received().DisplayText(_textToDisplay);
        }

        [Test]
        public void Start_NullText_DoesNotDisplayText()
        {
            _textToDisplay = null;
            ITutorialStepArgs args = new TutorialStepArgs(_highlighter, _textToDisplay, _displayer, _highlightableProvider);
            _tutorialStep = new DummyTutorialStep(args);

            _tutorialStep.Start(_completionCallback);

            _highlightableProvider.Received().FindItem();
            _highlighter.Received().Highlight(_highlightable);
            _displayer.DidNotReceiveWithAnyArgs().DisplayText(null);
        }

        [Test]
        public void Start_NullHighlightable_DoesNotHighlight()
        {
            _highlightableProvider.FindItem().Returns((IMaskHighlightable)null);

            _tutorialStep.Start(_completionCallback);

            _highlightableProvider.Received().FindItem();
            _highlighter.DidNotReceiveWithAnyArgs().Highlight(null);
            _displayer.Received().DisplayText(_textToDisplay);
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

            _highlighter.Received().Unhighlight();
            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void OnCompleted_NoPreviousStart_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(_tutorialStep.Complete);
        }

        [Test]
        public void DoubleOnCompleted_Throws()
        {
            _tutorialStep.Start(_completionCallback);
            _tutorialStep.Complete();

            Assert.Throws<UnityAsserts.AssertionException>(_tutorialStep.Complete);
        }
        #endregion OnCompleted
    }
}
