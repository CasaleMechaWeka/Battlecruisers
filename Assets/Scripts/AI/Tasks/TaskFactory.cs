using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.AI.Tasks
{
	public class TaskFactory : ITaskFactory
	{
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _cruiser;
        private readonly IDeferrer _deferrer;

        public TaskFactory(IPrefabFactory prefabFactory, ICruiserController cruiser, IDeferrer deferrer)
        {
            _prefabFactory = prefabFactory;
            _cruiser = cruiser;
            _deferrer = deferrer;
        }

		public IPrioritisedTask CreateConstructBuildingTask(TaskPriority taskPriority, IPrefabKey buildingKey)
        {
            IInternalTask constructBuildingTask = new ConstructBuildingTask(buildingKey, _prefabFactory, _cruiser, _deferrer);
            return new PrioritisedTask(taskPriority, constructBuildingTask);
        }

        public IPrioritisedTask CreateWaitForUnitConstructionTask(TaskPriority priority, IFactory factory)
        {
            IInternalTask waitForUnitConstructionTask = new WaitForUnitConstructionTask(factory);
            return new PrioritisedTask(priority, waitForUnitConstructionTask);
        }
    }
}