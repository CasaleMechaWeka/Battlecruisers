using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class DummyState : DurationState
    {
        public override bool ShouldFire => true;
    }

    public class DurationStateTests : StatesTestBase
    {
        private DurationState _durationState;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _durationState = new DummyState();
            _durationState.Initialise(_otherState, _durationProvider);
        }

        [Test]
        public void OnFired()
        {
            IState nextState = _durationState.OnFired();

            Assert.AreSame(_durationState, nextState);
            _durationProvider.Received().MoveToNextDuration();
        }

        [Test]
        public void ProcessTimeInterval_DuringInterval()
        {
            _durationProvider.DurationInS.Returns(2);
            _timePassedInS = 1;

            // 1s < 2s
            IState nextState = _durationState.ProcessTimeInterval(_timePassedInS);
            Assert.AreSame(_durationState, nextState);

            // 2s == 2s
            nextState = _durationState.ProcessTimeInterval(_timePassedInS);
            Assert.AreSame(_otherState, nextState);
        }
    }
}
