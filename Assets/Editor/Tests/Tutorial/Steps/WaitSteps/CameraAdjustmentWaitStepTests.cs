using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.Cameras.Adjusters;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.WaitSteps
{
    public class CameraAdjustmentWaitStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _step;
        private ICameraAdjuster _cameraAdjuster;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _cameraAdjuster = Substitute.For<ICameraAdjuster>();
            _step = new CameraAdjustmentWaitStep(_args, _cameraAdjuster);
        }

        [Test]
        public void CompletedAdjustment_TriggersCompletedCallback()
        {
            _step.Start(_completionCallback);

            _cameraAdjuster.CompletedAdjustment += Raise.Event();
            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void SecondCompletedAdjustment_IsIgnored()
        {
            _step.Start(_completionCallback);

            // First adjustment completes
            _cameraAdjuster.CompletedAdjustment += Raise.Event();
            Assert.AreEqual(1, _callbackCounter);

            // Second adjustment is ignored
            _cameraAdjuster.CompletedAdjustment += Raise.Event();
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}
