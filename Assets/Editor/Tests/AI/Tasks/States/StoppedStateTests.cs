using BattleCruisers.AI.Tasks.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Tasks.States
{
    public class StoppedStateTests : StateTestsBase
    {
        private BaseState _stoppedState, _inProgressState;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _inProgressState = Substitute.For<BaseState>();
            _stoppedState = new StoppedState(_task, _eventEmitter, _inProgressState);
        }

        [Test]
        public void Start_ResumesTask_GoesToInProgressState()
        {
            BaseState nextState = _stoppedState.Start();

            _task.Received().Resume();
            Assert.AreSame(_inProgressState, nextState);
        }

        [Test]
        public void Stop_StaysInStoppedState()
        {
            BaseState nextState = _stoppedState.Stop();
            Assert.AreSame(_stoppedState, nextState);
        }

        [Test]
        public void OnCompleted_GoesToCompletedState()
        {
            BaseState nextState = _stoppedState.OnCompleted();
            Assert.IsInstanceOf<CompletedState>(nextState);
        }
    }
}
