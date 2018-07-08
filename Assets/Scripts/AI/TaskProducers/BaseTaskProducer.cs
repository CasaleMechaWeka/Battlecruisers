using System;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.TaskProducers
{
    // FELIX  Rename to append Base :)
    public abstract class BaseTaskProducer : IDisposable
	{
		protected readonly ITaskList _tasks;
		protected readonly ICruiserController _cruiser;
		protected readonly ITaskFactory _taskFactory;
        protected readonly IPrefabFactory _prefabFactory;

		public BaseTaskProducer(
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

        public virtual void Dispose()
        {
        }
    }
}
