using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using BattleCruisers.UI.BattleScene.Manager;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps
{
    public class DismissStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _clickStep;
        private IDismissableEmitter _dismissableEmitter;
        private IUIManagerSettablePermissions _uiManagerPermissions;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _dismissableEmitter = Substitute.For<IDismissableEmitter>();
            _uiManagerPermissions = Substitute.For<IUIManagerSettablePermissions>();

            _clickStep = new DismissStep(_args, _dismissableEmitter, _uiManagerPermissions);
        }

        [Test]
        public void Start_EnablesDismissal()
        {
            _clickStep.Start(_completionCallback);
            _uiManagerPermissions.Received().CanDismissItemDetails = true;
        }

        [Test]
        public void Click_DisablesDismissal()
        {
            Start_EnablesDismissal();

            _dismissableEmitter.Dismissed += Raise.Event();
            _uiManagerPermissions.Received().CanDismissItemDetails = false;
        }
    }
}
