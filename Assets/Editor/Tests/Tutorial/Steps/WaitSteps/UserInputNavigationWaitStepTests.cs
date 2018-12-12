// FELIX  Convert or delete :)
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.WaitSteps;
//using BattleCruisers.UI.BattleScene.Navigation;
//using BattleCruisers.UI.Cameras;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps.WaitSteps
//{
//	public class DummyStep : UserInputNavigationWaitStep
//	{
//		public DummyStep(ITutorialStepArgs args, IUserInputCameraMover cameraMover, INavigationSettings navigationSettings) 
//			: base(args, cameraMover, navigationSettings)
//		{
//		}

//        public void Complete()
//		{
//			OnCompleted();
//		}
//	}

//	public class UserInputNavigationWaitStepTests : TutorialStepTestsBase
//    {
//		private DummyStep _tutorialStep;

//		private IUserInputCameraMover _cameraMover;
//        private INavigationSettings _navigationSettings;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//			_cameraMover = Substitute.For<IUserInputCameraMover>();
//            _navigationSettings = Substitute.For<INavigationSettings>();

//			_tutorialStep = new DummyStep(_args, _cameraMover, _navigationSettings);
//        }

//        [Test]
//        public void Start_EnablesUserInput()
//        {
//			_tutorialStep.Start(_completionCallback);
//			_navigationSettings.Received().Permission = NavigationPermission.UserInputOnly;
//        }

//        [Test]
//        public void Completed_DisablesUserInput()
//        {
//			_tutorialStep.Start(_completionCallback);
//			_tutorialStep.Complete();

//			_navigationSettings.Received().Permission = NavigationPermission.None;
//        }
//    }
//}
