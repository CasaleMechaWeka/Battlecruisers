using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.AI.Tasks
{
	public class TaskFactory : ITaskFactory
	{
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _cruiser;
        private readonly ICoroutinesHelper _coroutinesHelper;

        public TaskFactory(IPrefabFactory prefabFactory, ICruiserController cruiser, ICoroutinesHelper coroutinesHelper)
        {
            _prefabFactory = prefabFactory;
            _cruiser = cruiser;
            _coroutinesHelper = coroutinesHelper;
        }

		public ITask CreateConstructBuildingTask(TaskPriority taskPriority, IPrefabKey buildingKey)
        {
            IInternalTask constructBuildingTask = new ConstructBuildingTask(buildingKey, _prefabFactory, _cruiser, _coroutinesHelper);
            return new TaskController(taskPriority, constructBuildingTask);
        }
	}
}