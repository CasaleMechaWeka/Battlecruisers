using BattleCruisers.AI.Tasks.States;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.AI.Tasks.States
{
    public class InitialStateTests : StateTestsBase
    {
        private BaseState _state;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _state = new InitialState(_task, _eventEmitter);
        }

        [Test]
        public void Start_TaskSuccessfullyStarts_GoesToInProgress()
        {
            _task.Start().Returns(true);

            BaseState nextState = _state.Start();

            Assert.IsInstanceOf<InProgressState>(nextState);
        }

        [Test]
        public void Start_TaskFailsToStart_GoesToCompleted()
        {
            _task.Start().Returns(false);

            BaseState nextState = _state.Start();

            Assert.IsInstanceOf<CompletedState>(nextState);
        }

        [Test]
        public void Stop_StaysInInitialState()
        {
            BaseState nextState = _state.Stop();
            Assert.AreSame(_state, nextState);
        }

        [Test]
        public void OnCompleted_Throws()
        {
            Assert.Throws<Exception>(() => _state.OnCompleted());
        }
    }
}
