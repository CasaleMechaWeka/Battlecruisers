using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class FireIntervalManagerTests
    {
        private IFireIntervalManager _manager;
        private IState _firingState, _waitingState;

        [SetUp]
        public void TestSetup()
        {
            _waitingState = Substitute.For<IState>();
            _waitingState.ShouldFire.Returns(false);

            _firingState = Substitute.For<IState>();
            _firingState.ShouldFire.Returns(true);
            _firingState.OnFired().Returns(_waitingState);
            _firingState.ProcessTimeInterval(default).ReturnsForAnyArgs(_waitingState);

            _manager = new FireIntervalManager(_firingState);
        }

        [Test]
        public void ShouldFire()
        {
            Assert.IsTrue(_manager.ShouldFire);
            bool compilerBribe = _firingState.Received().ShouldFire;
        }

        [Test]
        public void OnFired()
        {
            _manager.OnFired();

            _firingState.Received().OnFired();
            Assert_ChangedToWaitingState();
        }

        [Test]
        public void ProcessTimeInterval()
        {
            float deltaTime = 0.248f;
            _manager.ProcessTimeInterval(deltaTime);

            _firingState.Received().ProcessTimeInterval(deltaTime);
            Assert_ChangedToWaitingState();
        }

        private void Assert_ChangedToWaitingState()
        {
            Assert.IsFalse(_manager.ShouldFire);
        }
    }
}