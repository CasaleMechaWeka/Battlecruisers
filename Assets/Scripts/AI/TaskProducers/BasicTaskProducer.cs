using System.Collections.Generic;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;

namespace BattleCruisers.AI.TaskProducers
{
    /// <summary>
    /// 1. Uses preset build order
    /// 2. Replaces destroyed buildings
    /// </summary>
    public class BasicTaskProducer : BaseTaskProducer
    {
        private readonly IPrefabFactory _prefabFactory;

        public BasicTaskProducer(ITaskList tasks, ICruiserController cruiser, IPrefabFactory prefabFactory, 
            ITaskFactory taskFactory, IList<IPrefabKey> buildOrder, ITaskProducerFactory taskProducerFactory)
            : base(tasks, cruiser, taskFactory)
        {
            _prefabFactory = prefabFactory;

			IDictionary<IBuilding, IPrefabKey> buildingToKey = new Dictionary<IBuilding, IPrefabKey>();
            CreateTasks(buildOrder, buildingToKey);

            taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(_tasks, _cruiser, _taskFactory, buildingToKey);
        }

        private void CreateTasks(IList<IPrefabKey> buildOrder, IDictionary<IBuilding, IPrefabKey> buildingToKey)
        {
            IDictionary<SlotType, int> slotTypeToBuildingCount = new Dictionary<SlotType, int>();

            foreach (IPrefabKey key in buildOrder)
            {
                IBuildableWrapper<IBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(key);

                if (ShouldCreateTask(slotTypeToBuildingCount, buildingWrapper.Buildable.SlotType))
                {
                    _tasks.Add(_taskFactory.CreateConstructBuildingTask(TaskPriority.Normal, key));

                    if (!buildingToKey.ContainsKey(buildingWrapper.Buildable))
                    {
                        buildingToKey.Add(buildingWrapper.Buildable, key);
                    }
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
