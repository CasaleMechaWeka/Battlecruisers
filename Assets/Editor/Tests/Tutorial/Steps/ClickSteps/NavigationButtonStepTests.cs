// FELIX  Convert or delete :)
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.ClickSteps;
//using BattleCruisers.UI.BattleScene.Navigation;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
//{
//	public class NavigationButtonStepTests : TutorialStepTestsBase
//    {
//        private ITutorialStep _clickStep;
//		private INavigationSettings _navigationSettings;
//        private IClickableEmitter _clickable;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//			_navigationSettings = Substitute.For<INavigationSettings>();
//            _clickable = Substitute.For<IClickableEmitter>();

//            _clickStep = new NavigationButtonStep(_args, _navigationSettings, _clickable);
//        }

//        [Test]
//        public void Start_PermitsNavigation()
//        {
//            _clickStep.Start(_completionCallback);
//			_navigationSettings.Received().Permission = NavigationPermission.TransitionsOnly;
//        }

//        [Test]
//        public void Click_DisablesNavigation()
//        {
//            Start_PermitsNavigation();

//            _clickable.Clicked += Raise.Event();
//			_navigationSettings.Received().Permission = NavigationPermission.None;
//        }
//    }
//}
