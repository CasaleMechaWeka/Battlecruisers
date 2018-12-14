using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.BoostSteps;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.BoostSteps
{
    public class AddTurretAccuracyBoostStepTests : BoostStepTestsBase
    {
        private ITutorialStep _boostStep;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _boostStep = new AddTurretAccuracyBoostStep(_args, _globalBoostProviders, _boostProvider);
        }

        [Test]
        public void Start_AddsBoost_AndCompletes()
        {
            _boostStep.Start(_completionCallback);
            _globalBoostProviders.TurretAccuracyBoostProviders.Received().Add(_boostProvider);
        }
    }
}
