using System.Collections.Generic;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;

namespace BattleCruisers.AI
{
    /// <summary>
    /// 1. Uses preset build order
    /// 2. Replaces destroyed buildings
    /// </summary>
    public class BasicTaskProducer : ITaskProducer
    {
        private readonly ITaskList _tasks;
        private readonly ICruiserController _cruiser;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ITaskFactory _taskFactory;

        public BasicTaskProducer(ITaskList tasks, ICruiserController cruiser, IPrefabFactory prefabFactory, 
             ITaskFactory taskFactory, IList<IPrefabKey> buildOrder)
        {
            _tasks = tasks;
            _cruiser = cruiser;
            _prefabFactory = prefabFactory;
            _taskFactory = taskFactory;

            CreateTasks(buildOrder);
        }

        private void CreateTasks(IList<IPrefabKey> buildOrder)
        {
            IDictionary<SlotType, int> slotTypeToBuildingCount = new Dictionary<SlotType, int>();

            foreach (IPrefabKey key in buildOrder)
            {
                IBuildableWrapper<IBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(key);

                if (ShouldCreateTask(slotTypeToBuildingCount, buildingWrapper.Buildable.SlotType))
                {
                    _tasks.Add(_taskFactory.CreateConstructBuildingTask(TaskPriority.Normal, key));
                }
            }
        }

        private bool ShouldCreateTask(IDictionary<SlotType, int> slotTypeToBuildingCount, SlotType slotType)
        {
            bool shouldCreateTask = false;

			if (!slotTypeToBuildingCount.ContainsKey(slotType))
			{
                slotTypeToBuildingCount.Add(slotType, 0);
			}

            if (slotTypeToBuildingCount[slotType] < _cruiser.GetSlotCount(slotType))
            {
                slotTypeToBuildingCount[slotType]++;
                shouldCreateTask = true;
            }

            return shouldCreateTask;
        }
    }
}
