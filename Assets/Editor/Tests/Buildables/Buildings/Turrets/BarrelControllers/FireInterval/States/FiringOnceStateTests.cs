using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class FiringOnceStateTests : StatesTestBase
    {
        private FiringOnceState _firingState;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _firingState = new FiringOnceState();
            _firingState.Initialise(_otherState, _durationProvider);
        }

        [Test]
        public void ShouldFire()
        {
            Assert.IsTrue(_firingState.ShouldFire);
        }

        [Test]
        public void OnFired()
        {
            IState nextState = _firingState.OnFired();

            Assert.AreSame(_otherState, nextState);
            _durationProvider.Received().MoveToNextDuration();
        }

        [Test]
        public void ProcessTimeInterval()
        {
            IState nextState = _firingState.ProcessTimeInterval(_timePassedInS);
            Assert.AreSame(_firingState, nextState);
        }
    }
}
