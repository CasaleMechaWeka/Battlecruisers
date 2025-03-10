using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.UI.BattleScene.Manager;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.FeatureModifierSteps
{
    public class UIManagerPermissionsStepTests : TutorialStepTestsBase
    {
        [Test]
        public void Start()
        {
            IUIManagerSettablePermissions permissions = Substitute.For<IUIManagerSettablePermissions>();
            ITutorialStep step = new UIManagerPermissionsStep(_args, permissions, canShowItemDetails: true, canDismissItemDetails: false);

            step.Start(_completionCallback);

            permissions.Received().CanShowItemDetails = true;
            permissions.Received().CanDismissItemDetails = false;
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}