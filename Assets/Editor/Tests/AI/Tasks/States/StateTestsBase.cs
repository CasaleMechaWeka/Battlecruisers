using BattleCruisers.AI.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Tasks.States
{
    public abstract class StateTestsBase
    {
        protected ITask _task;
        protected ICompletedEventEmitter _eventEmitter;

        [SetUp]
        public virtual void TestSetup()
        {
            _task = Substitute.For<ITask>();
            _eventEmitter = Substitute.For<ICompletedEventEmitter>();
        }
    }
}
