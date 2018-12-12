//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.WaitSteps;
//using BattleCruisers.UI.BattleScene.Navigation;
//using BattleCruisers.UI.Cameras;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps.WaitSteps
//{
//    // FELIX  Convert or delete :)
//    public class ScrollWaitStepTests : TutorialStepTestsBase
//    {
//        private ITutorialStep _tutorialStep;

//        private IUserInputCameraMover _cameraMover;
//        private INavigationSettings _navigationSettings;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//            _cameraMover = Substitute.For<IUserInputCameraMover>();
//            _navigationSettings = Substitute.For<INavigationSettings>();

//			_tutorialStep = new ScrollWaitStep(_args, _cameraMover, _navigationSettings);
//        }

//        [Test]
//        public void Scroll_Completes()
//        {
//            _tutorialStep.Start(_completionCallback);
//            _cameraMover.Scrolled += Raise.Event();
//            Assert.AreEqual(1, _callbackCounter);
//        }

//        [Test]
//        public void DoubleScroll_OnlyCompletesOnce()
//        {
//            _tutorialStep.Start(_completionCallback);

//            // First scroll completes
//            _cameraMover.Scrolled += Raise.Event();
//            Assert.AreEqual(1, _callbackCounter);

//            // Second scroll does nothing
//            _cameraMover.Scrolled += Raise.Event();
//            Assert.AreEqual(1, _callbackCounter);
//        }
//    }
//}
