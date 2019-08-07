using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
{
    public class ExplanationDismissableStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _step;
        private IExplanationDismissButton _dismissButton;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _dismissButton = Substitute.For<IExplanationDismissButton>();
            _step = new ExplanationDismissableStep(_args, _dismissButton);
        }

        [Test]
        public void Start_ShowsDismissButton()
        {
            _step.Start(_completionCallback);
            _dismissButton.Received().Enabled = true;
        }

        [Test]
        public void Click_HidesDismissButton()
        {
            Start_ShowsDismissButton();

            _dismissButton.Clicked += Raise.Event();
            _dismissButton.Received().Enabled = false;
        }
    }
}
