using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.TaskProducers
{
    public abstract class TaskProducer : ITaskProducer
    {
        protected readonly TaskList _tasks;
        protected readonly ICruiserController _cruiser;
        protected readonly ITaskFactory _taskFactory;

        public TaskProducer(
            TaskList tasks,
            ICruiserController cruiser,
            ITaskFactory taskFactory)
        {
            Helper.AssertIsNotNull(tasks, cruiser, taskFactory);

            _tasks = tasks;
            _cruiser = cruiser;
            _taskFactory = taskFactory;
        }

        public virtual void DisposeManagedState()
        {
        }
    }
}
