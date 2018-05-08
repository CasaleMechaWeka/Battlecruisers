using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.UI;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
{
    public class NavigationStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _clickStep;
        private BasicFilter _shouldNavigationBeEnabledFilter;
        private IClickable _clickable;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _shouldNavigationBeEnabledFilter = new BasicFilter(isMatch: false);
            _clickable = Substitute.For<IClickable>();

            _clickStep = new NavigationStep(_args, _shouldNavigationBeEnabledFilter, _clickable);
        }

        [Test]
        public void Start_PermitsNavigation()
        {
            _clickStep.Start(_completionCallback);
            Assert.IsTrue(_shouldNavigationBeEnabledFilter.IsMatch);
        }

        [Test]
        public void Click_DisablesNavigation()
        {
            Start_PermitsNavigation();

            _clickable.Clicked += Raise.Event();
            Assert.IsFalse(_shouldNavigationBeEnabledFilter.IsMatch);
        }
    }
}
