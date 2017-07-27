using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;

namespace BattleCruisers.AI.Tasks
{
	public class TaskFactory : ITaskFactory
	{
        public readonly IPrefabFactory _prefabFactory;
        public readonly ICruiserController _cruiser;

        public TaskFactory(IPrefabFactory prefabFactory, ICruiserController cruiser)
        {
            _prefabFactory = prefabFactory;
            _cruiser = cruiser;
        }

		public ITask CreateConstructBuildingTask(TaskPriority taskPriority, IPrefabKey buildingKey)
        {
            IInternalTask constructBuildingTask = new ConstructBuildingTask(buildingKey, _prefabFactory, _cruiser);
            return new TaskController(taskPriority, constructBuildingTask);
        }
	}
}