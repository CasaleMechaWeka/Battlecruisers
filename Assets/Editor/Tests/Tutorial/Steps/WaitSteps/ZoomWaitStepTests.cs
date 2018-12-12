// FELIX  Convert or delete :)
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.WaitSteps;
//using BattleCruisers.UI.BattleScene.Navigation;
//using BattleCruisers.UI.Cameras;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps.WaitSteps
//{
//	public class ZoomWaitStepTests : TutorialStepTestsBase
//    {
//		private ITutorialStep _tutorialStep;

//        private IUserInputCameraMover _cameraMover;
//        private INavigationSettings _navigationSettings;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//            _cameraMover = Substitute.For<IUserInputCameraMover>();
//            _navigationSettings = Substitute.For<INavigationSettings>();

//			_tutorialStep = new ZoomWaitStep(_args, _cameraMover, _navigationSettings);
//        }

//        [Test]
//        public void Zoom_Completes()
//        {
//            _tutorialStep.Start(_completionCallback);
//			_cameraMover.Zoomed += Raise.Event();
//			Assert.AreEqual(1, _callbackCounter);
//        }

//        [Test]
//        public void DoubleZoom_OnlyCompletesOnce()
//        {
//			_tutorialStep.Start(_completionCallback);

//            // First zoom completes
//            _cameraMover.Zoomed += Raise.Event();
//            Assert.AreEqual(1, _callbackCounter);

//            // Second zoom does nothing
//			_cameraMover.Zoomed += Raise.Event();
//            Assert.AreEqual(1, _callbackCounter);
//        }
//    }
//}
