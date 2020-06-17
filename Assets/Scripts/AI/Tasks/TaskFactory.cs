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

        // For cheating :)
        public static DelayProvider delayProvider;

        public const float DEFAULT_DELAY_IN_S = 1.5f;

        public TaskFactory(IPrefabFactory prefabFactory, ICruiserController cruiser, IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(prefabFactory, cruiser, deferrer);

            _prefabFactory = prefabFactory;
            _cruiser = cruiser;
            _deferrer = deferrer;

            delayProvider = new DelayProvider(DEFAULT_DELAY_IN_S);
        }

		public IPrioritisedTask CreateConstructBuildingTask(TaskPriority priority, IPrefabKey buildingKey)
        {
            ITask constructBuildingTask = new ConstructBuildingTask(buildingKey, _prefabFactory, _cruiser);
            return CreatePrioritisedTask(constructBuildingTask, priority);
        }

        private IPrioritisedTask CreatePrioritisedTask(ITask task, TaskPriority priority)
        {
            IPrioritisedTask prioritisedTask = new PrioritisedTask(priority, task);
            return new DeferredPrioritisedTask(prioritisedTask, _deferrer, delayProvider);
        }
    }
}
