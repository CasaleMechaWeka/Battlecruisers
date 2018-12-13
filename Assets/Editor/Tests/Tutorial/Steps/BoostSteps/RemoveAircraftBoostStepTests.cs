using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.BoostSteps
{
    public class RemoveAircraftBoostStepTests : BoostStepTestsBase
    {
        private ITutorialStep _boostStep;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _boostStep = new RemoveAircraftBoostStep(_args, _globalBoostProviders, _boostProvider);
        }

        [Test]
        public void Start_RemovesBoost_AndCompletes()
        {
            _boostStep.Start(_completionCallback);
            _globalBoostProviders.AircraftBoostProviders.Received().Remove(_boostProvider);
        }
    }
}
