// FELIX  Convert or delete :)
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.EnemyCruiser;
//using BattleCruisers.UI.BattleScene.Navigation;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps
//{
//	public class NavigationPermitterStepTests : TutorialStepTestsBase
//    {
//        private ITutorialStep _tutorialStep;
//		private INavigationSettings _navigationSettings;
//		private NavigationPermission _permission;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//			_permission = NavigationPermission.None;
//			_navigationSettings = Substitute.For<INavigationSettings>();
//            _tutorialStep = new NavigationPermitterStep(_args, _navigationSettings, _permission);
//        }

//        [Test]
//        public void Start_SetNavigationPermission_AndCompletes()
//        {
//            _tutorialStep.Start(_completionCallback);
//			_navigationSettings.Received().Permission = _permission;
//            Assert.AreEqual(1, _callbackCounter);
//        }
//    }
//}
