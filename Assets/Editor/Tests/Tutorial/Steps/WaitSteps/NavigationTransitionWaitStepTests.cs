// FELIX  Convert or delete :)
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.WaitSteps;
//using BattleCruisers.UI.BattleScene.Navigation;
//using BattleCruisers.UI.Cameras;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps.WaitSteps
//{
//	public class NavigationTransitionWaitStepTests : TutorialStepTestsBase
//    {
//		private ITutorialStep _tutorialStep;

//		private ICameraMover _cameraMover;
//        private CameraState _targetState;
//        private INavigationSettings _navigationSettings;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//			_cameraMover = Substitute.For<ICameraMover>();
//			_targetState = CameraState.Overview;
//			_navigationSettings = Substitute.For<INavigationSettings>();

//			_tutorialStep = new NavigationTransitionWaitStep(_args, _cameraMover, _targetState, _navigationSettings);
//        }

//        #region Start
//		[Test]
//		public void Start()
//		{
//			_cameraMover.State.Returns(CameraState.PlayerCruiser);

//			_tutorialStep.Start(_completionCallback);

//			_navigationSettings.Received().Permission = NavigationPermission.TransitionsOnly;
//		}

//        [Test]
//        public void Start_AlreadyInTargetState_InstaCompletes()
//        {
//			_cameraMover.State.Returns(_targetState);
//            _tutorialStep.Start(_completionCallback);

//			Assert.AreEqual(1, _callbackCounter);
//        }
//        #endregion Start

//		[Test]
//		public void CameraStateChanged_NotTargetState_DoesNothing()
//		{
//			Start();
//			_cameraMover.StateChanged += Raise.EventWith(new CameraStateChangedArgs(CameraState.AiCruiser, CameraState.InTransition));
//			Assert.AreEqual(0, _callbackCounter);
//		}

//        [Test]
//        public void CameraStateChanged_TargetReached_Completes()
//        {
//            Start();
//			_cameraMover.StateChanged += Raise.EventWith(new CameraStateChangedArgs(CameraState.InTransition, CameraState.Overview));
//            Assert.AreEqual(1, _callbackCounter);
//        }

//        [Test]
//        public void Completion_UnsubsribesCameraStateChangedHandler()
//        {
//			Start();
//            _cameraMover.StateChanged += Raise.EventWith(new CameraStateChangedArgs(CameraState.InTransition, CameraState.Overview));
//            Assert.AreEqual(1, _callbackCounter);

//            // New event does not comlpete again
//			_cameraMover.StateChanged += Raise.EventWith(new CameraStateChangedArgs(CameraState.InTransition, CameraState.Overview));
//            Assert.AreEqual(1, _callbackCounter);
//        }
//    }
//}
