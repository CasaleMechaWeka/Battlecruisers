// FELIX  Convert or delete :)
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.EnemyCruiser;
//using BattleCruisers.UI.BattleScene.Navigation;
//using BattleCruisers.UI.Cameras;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps
//{
//    public class AutomaticNavigationButtonStepTests : TutorialStepTestsBase
//    {
//        private ITutorialStep _tutorialStep;
//        private INavigationSettings _navigationSettings;
//        private CameraState _targetState;
//        private ICameraController _cameraController;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//            _navigationSettings = Substitute.For<INavigationSettings>();
//            _targetState = CameraState.LeftMid;
//            _cameraController = Substitute.For<ICameraController>();

//            _tutorialStep = new AutomaticNavigationButtonStep(_args, _navigationSettings, _targetState, _cameraController);
//        }

//        [Test]
//        public void Start_Navigates_AndCompletes()
//        {
//            _tutorialStep.Start(_completionCallback);

//            _navigationSettings.Received().Permission = NavigationPermission.TransitionsOnly;
//            _navigationSettings.Received().Permission = NavigationPermission.None;
//            _cameraController.Received().ShowMidLeft();
//            Assert.AreEqual(1, _callbackCounter);
//        }
//    }
//}
