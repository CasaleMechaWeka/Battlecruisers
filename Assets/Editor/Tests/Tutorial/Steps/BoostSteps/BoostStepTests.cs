using BattleCruisers.Buildables.Boost;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.BoostSteps;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.BoostSteps
{
    public class DummyBoostStep : BoostStep
    {
        public int BoostActionCount { get; private set; }

        public DummyBoostStep(ITutorialStepArgs args, IGlobalBoostProviders globalBoostProviders, IBoostProvider boostProvider)
            : base(args, globalBoostProviders, boostProvider)
        {
            BoostActionCount = 0;
        }

        protected override void BoostProviderAction()
        {
            BoostActionCount++;
        }
    }

    public class BoostStepTests : BoostStepTestsBase
    {
        private DummyBoostStep _boostStep;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _boostStep = new DummyBoostStep(_args, _globalBoostProviders, _boostProvider);
        }

        [Test]
        public void Start_RunsBoostAction_AndCompletes()
        {
            _boostStep.Start(_completionCallback);

            Assert.AreEqual(1, _boostStep.BoostActionCount);
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}
