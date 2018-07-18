using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.AI.Tasks
{
    public class TaskFactory : ITaskFactory
	{
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _cruiser;
        private readonly IDeferrer _deferrer;
        private readonly IRandomGenerator _randomGenerator;

        public TaskFactory(IPrefabFactory prefabFactory, ICruiserController cruiser, IDeferrer deferrer, IRandomGenerator randomGenerator)
        {
            Helper.AssertIsNotNull(prefabFactory, cruiser, deferrer, randomGenerator);

            _prefabFactory = prefabFactory;
            _cruiser = cruiser;
            _deferrer = deferrer;
            _randomGenerator = randomGenerator;
        }

		public IPrioritisedTask CreateConstructBuildingTask(TaskPriority priority, IPrefabKey buildingKey)
        {
            ITask constructBuildingTask = new ConstructBuildingTask(buildingKey, _prefabFactory, _cruiser);
            return CreatePrioritisedTask(constructBuildingTask, priority);
        }

        public IPrioritisedTask CreateWaitForUnitConstructionTask(TaskPriority priority, IFactory factory)
        {
            int numOfUnitsToBuild = _randomGenerator.Range(minInclusive: 2, maxInclusive: 4);
            ITask waitForUnitConstructionTask = new WaitForUnitConstructionTask(factory, numOfUnitsToBuild);
            return CreatePrioritisedTask(waitForUnitConstructionTask, priority);
        }

        private IPrioritisedTask CreatePrioritisedTask(ITask task, TaskPriority priority)
        {
            IPrioritisedTask prioritisedTask = new PrioritisedTask(priority, task);
            return new DeferredPrioritisedTask(prioritisedTask, _deferrer);
        }
    }
}
