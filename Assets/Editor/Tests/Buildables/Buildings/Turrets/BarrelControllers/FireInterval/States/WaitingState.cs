using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class WaitingStateTests : StatesTestBase
    {
        private WaitingState _waitingState;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _waitingState = new WaitingState();
            _waitingState.Initialise(_otherState, _durationProvider);
        }

        [Test]
        public void ShouldFire()
        {
            Assert.IsFalse(_waitingState.ShouldFire);
        }
    }
}
