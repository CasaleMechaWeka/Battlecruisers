using BattleCruisers.AI.Tasks.States;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.AI.Tasks.States
{
    public class CompletedStateTests : StateTestsBase
    {
        private IState _state;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _state = new CompletedState(_task, _eventEmitter);
        }

        [Test]
        public void Start_EmitsEvent_StaysInCompletedState()
        {
            IState nextState = _state.Start();

            Assert.AreSame(_state, nextState);
            _eventEmitter.Received().EmitCompletedEvent();
        }

        [Test]
        public void Stop_StaysInCompletedState()
        {
            IState nextState = _state.Stop();
            Assert.AreSame(_state, nextState);
        }

        [Test]
        public void OnCompleted_Throws()
        {
            Assert.Throws<Exception>(() => _state.OnCompleted());
        }
    }
}
