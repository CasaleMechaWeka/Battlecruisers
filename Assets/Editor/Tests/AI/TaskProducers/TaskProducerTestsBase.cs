using BattleCruisers.AI;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.TaskProducers
{
    public abstract class TaskProducerTestsBase
    {
        protected ITaskList _tasks;
        protected ICruiserController _cruiser;
        protected IPrefabFactory _prefabFactory;
        protected ITaskFactory _taskFactory;

        [SetUp]
        public virtual void SetuUp()
        {
            _tasks = Substitute.For<ITaskList>();
            _cruiser = Substitute.For<ICruiserController>();
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _taskFactory = Substitute.For<ITaskFactory>();
        }
    }
}
