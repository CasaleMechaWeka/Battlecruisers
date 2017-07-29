using System.Collections.Generic;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;

namespace BattleCruisers.AI.TaskProducers
{
    /// <summary>
    /// 1. Creates tasks for the given buildOrder.
    /// </summary>
    public class BasicTaskProducer : BaseTaskProducer
    {
        public BasicTaskProducer(ITaskList tasks, ICruiserController cruiser, IPrefabFactory prefabFactory, 
            ITaskFactory taskFactory, IList<IPrefabKey> buildOrder)
            : base(tasks, cruiser, taskFactory, prefabFactory)
        {
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

            if (slotTypeToBuildingCount[slotType] < _cruiser.SlotWrapper.GetSlotCount(slotType))
            {
                slotTypeToBuildingCount[slotType]++;
                shouldCreateTask = true;
            }

            return shouldCreateTask;
        }
    }
}
