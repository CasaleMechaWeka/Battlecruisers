using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.BattleScene.MainMenu;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.WaitSteps
{
    public class MenuDismissedWaitStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _tutorialStep;
        private IModalMenu _menu;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _menu = Substitute.For<IModalMenu>();
            _tutorialStep = new MenuDismissedWaitStep(_args, _menu);

            _menu.IsVisible.Value.Returns(true);
        }

        [Test]
        public void Start_IsHidden()
        {
            _menu.IsVisible.Value.Returns(false);
            _tutorialStep.Start(_completionCallback);
            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void Start_IsNotHidden()
        {
            _tutorialStep.Start(_completionCallback);
            Assert.AreEqual(0, _callbackCounter);
        }

        [Test]
        public void IsVisibleChanged_ToFalse()
        {
            _tutorialStep.Start(_completionCallback);
            Assert.AreEqual(0, _callbackCounter);
            _menu.IsVisible.Value.Returns(false);

            _menu.IsVisible.ValueChanged += Raise.Event();

            Assert.AreEqual(1, _callbackCounter);
        }

        [Test]
        public void IsVisibleChanged_ToTrue()
        {
            _tutorialStep.Start(_completionCallback);
            Assert.AreEqual(0, _callbackCounter);

            _menu.IsVisible.ValueChanged += Raise.Event();

            Assert.AreEqual(0, _callbackCounter);
        }
    }
}
