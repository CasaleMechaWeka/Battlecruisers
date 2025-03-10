using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.Panels;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.WaitSteps
{
    public class SlidingPanelWaitStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _tutorialStep;
        private ISlidingPanel _slidingPanel;
        private PanelState _desiredState;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _slidingPanel = Substitute.For<ISlidingPanel>();
            _desiredState = PanelState.Shown;

            _tutorialStep = new SlidingPanelWaitStep(_args, _slidingPanel, _desiredState);

            _slidingPanel.State.Value.Returns(PanelState.Hidden);
        }

        [Test]
        public void Start_IsInDesiredState()
        {
            _slidingPanel.State.Value.Returns(_desiredState);
            _tutorialStep.Start(_completionCallback);
            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void Start_IsNotInDesiredState()
        {
            _tutorialStep.Start(_completionCallback);
            Assert.AreEqual(0, _callbackCounter);
        }

        [Test]
        public void PanelStateChanged_ToDesiredState()
        {
            _tutorialStep.Start(_completionCallback);
            Assert.AreEqual(0, _callbackCounter);
            _slidingPanel.State.Value.Returns(_desiredState);

            _slidingPanel.State.ValueChanged += Raise.Event();

            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void PanelStateChanged_ToNonDesiredState()
        {
            _tutorialStep.Start(_completionCallback);
            Assert.AreEqual(0, _callbackCounter);

            _slidingPanel.State.ValueChanged += Raise.Event();

            Assert.AreEqual(0, _callbackCounter);
        }
    }
}
