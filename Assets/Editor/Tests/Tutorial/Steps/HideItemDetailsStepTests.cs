using BattleCruisers.Tutorial.Steps;
using BattleCruisers.UI.BattleScene.Manager;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps
{
    public class HideItemDetailsStepTests : TutorialStepTestsBase
    {
        [Test]
        public void Start()
        {
            IUIManager uiManager = Substitute.For<IUIManager>();
            ITutorialStep step = new HideItemDetailsStep(_args, uiManager);

            step.Start(_completionCallback);

            uiManager.Received().HideItemDetails();
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}