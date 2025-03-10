using BattleCruisers.AI.Tasks.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Tasks.States
{
    public class StoppedStateTests : StateTestsBase
    {
        private IState _stoppedState, _inProgressState;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _inProgressState = Substitute.For<IState>();
            _stoppedState = new StoppedState(_task, _eventEmitter, _inProgressState);
        }

        [Test]
        public void Start_ResumesTask_GoesToInProgressState()
        {
            IState nextState = _stoppedState.Start();

            _task.Received().Resume();
            Assert.AreSame(_inProgressState, nextState);
        }

        [Test]
        public void Stop_StaysInStoppedState()
        {
            IState nextState = _stoppedState.Stop();
            Assert.AreSame(_stoppedState, nextState);
        }

        [Test]
        public void OnCompleted_GoesToCompletedState()
        {
            IState nextState = _stoppedState.OnCompleted();
            Assert.IsInstanceOf<CompletedState>(nextState);
        }
    }
}
