using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class FiringDurationStateTests : StatesTestBase
    {
        private FiringDurationState _firingState;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _firingState = new FiringDurationState();
            _firingState.Initialise(_otherState, _durationProvider);
        }

        [Test]
        public void ShouldFire()
        {
            Assert.IsTrue(_firingState.ShouldFire);
        }
    }
}
