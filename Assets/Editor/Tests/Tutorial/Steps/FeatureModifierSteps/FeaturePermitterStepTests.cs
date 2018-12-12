using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.UI.Filters;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.FeatureModifierSteps
{
    public class FeaturePermitterStepTests : TutorialStepTestsBase
    {
        [Test]
        public void Start()
        {
            BasicFilter permitter = new BasicFilter(isMatch: false);
            ITutorialStep step = new FeaturePermitterStep(_args, permitter, enableFeature: true);

            step.Start(_completionCallback);

            Assert.IsTrue(permitter.IsMatch);
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}