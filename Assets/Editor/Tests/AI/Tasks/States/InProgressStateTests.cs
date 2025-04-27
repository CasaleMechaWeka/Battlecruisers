using BattleCruisers.AI.Tasks.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Tasks.States
{
    public class InProgressStateTests : StateTestsBase
    {
        private BaseState _state;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _state = new InProgressState(_task, _eventEmitter);
        }

        [Test]
        public void Start_StaysInInProgressState()
        {
            BaseState nextState = _state.Start();
            Assert.AreSame(_state, nextState);
        }

        [Test]
        public void Stop_StopsTask_GoesToStoppedState()
        {
            BaseState nextState = _state.Stop();

            _task.Received().Stop();
            Assert.IsInstanceOf<StoppedState>(nextState);
        }

        [Test]
        public void OnCompleted_GoesToCompletedState()
        {
            BaseState nextState = _state.OnCompleted();
            Assert.IsInstanceOf<CompletedState>(nextState);
        }
    }
}
