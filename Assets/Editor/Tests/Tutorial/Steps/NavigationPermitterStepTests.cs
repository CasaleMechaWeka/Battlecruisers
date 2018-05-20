using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using BattleCruisers.UI;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps
{
	public class NavigationPermitterStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _tutorialStep;
        private BasicFilter _navigationPermitter;
        private bool _isNavigationEnabled;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _isNavigationEnabled = true;
            _navigationPermitter = new BasicFilter(!_isNavigationEnabled);
            _tutorialStep = new NavigationPermitterStep(_args, _navigationPermitter, _isNavigationEnabled);
        }

        [Test]
        public void Start_SetNavigationPermission_AndCompletes()
        {
            _tutorialStep.Start(_completionCallback);
			Assert.AreEqual(_isNavigationEnabled, _navigationPermitter.IsMatch);
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}
