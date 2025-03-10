using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.AI.TaskProducers
{
    public abstract class TaskProducer : ITaskProducer
    {
		protected readonly ITaskList _tasks;
		protected readonly ICruiserController _cruiser;
		protected readonly ITaskFactory _taskFactory;
        protected readonly IPrefabFactory _prefabFactory;

		public TaskProducer(
            ITaskList tasks, 
            ICruiserController cruiser, 
            ITaskFactory taskFactory, 
            IPrefabFactory prefabFactory)
		{
            Helper.AssertIsNotNull(tasks, cruiser, taskFactory, prefabFactory);

			_tasks = tasks;
			_cruiser = cruiser;
			_taskFactory = taskFactory;
            _prefabFactory = prefabFactory;
		}

        public virtual void DisposeManagedState()
        {
        }
    }
}
