using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.AI.Tasks
{
    public class TaskFactory : ITaskFactory
	{
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _cruiser;

        public TaskFactory(IPrefabFactory prefabFactory, ICruiserController cruiser)
        {
            Helper.AssertIsNotNull(prefabFactory, cruiser);

            _prefabFactory = prefabFactory;
            _cruiser = cruiser;
        }

		public IPrioritisedTask CreateConstructBuildingTask(TaskPriority taskPriority, IPrefabKey buildingKey)
        {
            ITask constructBuildingTask = new ConstructBuildingTask(buildingKey, _prefabFactory, _cruiser);
            return new PrioritisedTask(taskPriority, constructBuildingTask);
        }

        public IPrioritisedTask CreateWaitForUnitConstructionTask(TaskPriority priority, IFactory factory)
        {
            ITask waitForUnitConstructionTask = new WaitForUnitConstructionTask(factory);
            return new PrioritisedTask(priority, waitForUnitConstructionTask);
        }
    }
}
