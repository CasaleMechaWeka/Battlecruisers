using BattleCruisers.AI;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.TaskProducers
{
    public abstract class TaskProducerTestsBase
    {
        protected ITaskList _tasks;
        protected ICruiserController _cruiser;
        protected ITaskFactory _taskFactory;

        [SetUp]
        public virtual void SetuUp()
        {
            _tasks = Substitute.For<ITaskList>();
            _cruiser = Substitute.For<ICruiserController>();
            _taskFactory = Substitute.For<ITaskFactory>();
        }
    }
}
