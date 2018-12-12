using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
{
    public class ExplanationClickStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _clickStep;
        private IItemProvider<IClickableEmitter> _clickableProvider;
        private IClickableEmitter _clickable;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _clickable = Substitute.For<IClickableEmitter>();
            _clickableProvider = Substitute.For<IItemProvider<IClickableEmitter>>();
            _clickableProvider.FindItem().Returns(_clickable);

            _clickStep = new ExplanationClickStep(_args, _clickableProvider);
        }

        [Test]
        public void Start()
        {
            _clickStep.Start(_completionCallback);
            _clickableProvider.Received().FindItem();
        }

        [Test]
        public void ClickCompletes()
        {
            Start();

            _clickable.Clicked += Raise.Event();
            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void SecondClick_DoesNothing()
        {
            Start();

            // First click completes
            _clickable.Clicked += Raise.Event();
            Assert.AreEqual(1, _callbackCounter);

            // Second click does nothing
            _clickable.Clicked += Raise.Event();
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}
