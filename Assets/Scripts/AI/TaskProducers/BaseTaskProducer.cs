using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;

namespace BattleCruisers.AI.TaskProducers
{
    public abstract class BaseTaskProducer
	{
		protected readonly ITaskList _tasks;
		protected readonly ICruiserController _cruiser;
		protected readonly ITaskFactory _taskFactory;

		public BaseTaskProducer(ITaskList tasks, ICruiserController cruiser, ITaskFactory taskFactory)
		{
			_tasks = tasks;
			_cruiser = cruiser;
			_taskFactory = taskFactory;
		}
	}
}
