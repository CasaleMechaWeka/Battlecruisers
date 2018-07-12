using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.Tasks.States;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.AI.Tasks.States
{
    // FELIX  Base class, avoid duplicate code :D
    public class InitialStateTests
    {
        private IState _state;

        private ITask _task;
        private ICompletedEventEmitter _eventEmitter;

        [SetUp]
        public void TestSetup()
        {
            _task = Substitute.For<ITask>();
            _eventEmitter = Substitute.For<ICompletedEventEmitter>();

            _state = new InitialState(_task, _eventEmitter);
        }

        [Test]
        public void Start_TaskSuccessfullyStarts_GoesToInProgress()
        {
            _task.Start().Returns(true);

            IState nextState = _state.Start();

            Assert.IsInstanceOf(typeof(InProgressState), nextState);
        }

        [Test]
        public void Start_TaskFailsToStart_GoesToCompleted()
        {
            _task.Start().Returns(false);

            IState nextState = _state.Start();

            Assert.IsInstanceOf(typeof(CompletedState), nextState);
        }

        [Test]
        public void Stop_StaysInInitialState()
        {
            IState nextState = _state.Stop();
            Assert.AreSame(_state, nextState);
        }

        [Test]
        public void Completed_Throws()
        {
            Assert.Throws<Exception>(() => _state.OnCompleted());
        }
    }
}
