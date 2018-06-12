using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.BoostSteps
{
    public class AddAircraftBoostStepTests : AircraftBoostStepTestsBase
    {
        private ITutorialStep _boostStep;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _boostStep = new AddAircraftBoostStep(_args, _boostProvidersManager, _boostProvider);
        }

        [Test]
        public void Start_AddsBoost_AndCompletes()
        {
            _boostStep.Start(_completionCallback);
            _aircraftBoostProviders.Received().Add(_boostProvider);
        }
    }
}
