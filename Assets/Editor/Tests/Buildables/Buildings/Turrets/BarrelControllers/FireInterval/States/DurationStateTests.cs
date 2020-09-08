using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class DummyState : DurationState
    {
        public override bool ShouldFire => true;

        public bool HaveFired => _haveFired;

        private bool _shouldProcessTimeInterval;
        public bool SetShouldProcessTimeInterval { set { _shouldProcessTimeInterval = value; } }
        protected override bool ShouldProcessTimeInterval()
        {
            return _shouldProcessTimeInterval;
        }
    }

    public class DurationStateTests : StatesTestBase
    {
        private DummyState _durationState;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _durationState = new DummyState();
            _durationState.Initialise(_otherState, _durationProvider);
        }

        [Test]
        public void InitialState()
        {
            Assert.IsFalse(_durationState.HaveFired);
        }

        [Test]
        public void OnFired()
        {
            IState nextState = _durationState.OnFired();

            Assert.AreSame(_durationState, nextState);
            _durationProvider.Received().MoveToNextDuration();
            Assert.IsTrue(_durationState.HaveFired);
        }

        [Test]
        public void ProcessTimeInterval_DuringInterval()
        {
            _durationProvider.DurationInS.Returns(2);
            _timePassedInS = 1;
            OnFired();
            Assert.IsTrue(_durationState.HaveFired);

            // 1s < 2s
            _durationState.SetShouldProcessTimeInterval = true;
            IState nextState = _durationState.ProcessTimeInterval(_timePassedInS);
            Assert.AreSame(_durationState, nextState);
            Assert.IsTrue(_durationState.HaveFired);

            // Ignored
            _durationState.SetShouldProcessTimeInterval = false;
            nextState = _durationState.ProcessTimeInterval(_timePassedInS);
            Assert.AreSame(_durationState, nextState);
            Assert.IsTrue(_durationState.HaveFired);

            // 2s == 2s
            _durationState.SetShouldProcessTimeInterval = true;
            nextState = _durationState.ProcessTimeInterval(_timePassedInS);
            Assert.AreSame(_otherState, nextState);
            Assert.IsFalse(_durationState.HaveFired);
        }
    }
}
