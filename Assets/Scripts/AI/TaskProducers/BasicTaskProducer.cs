using System;
using BattleCruisers.AI.Providers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.TaskProducers
{
    /// <summary>
    /// Lazily creates tasks when the task list becomes empty.
    /// </summary>
    public class BasicTaskProducer : BaseTaskProducer
    {
        private readonly IDynamicBuildOrder _buildOrder;

        public BasicTaskProducer(
            ITaskList tasks, 
            ICruiserController cruiser, 
            IPrefabFactory prefabFactory, 
            ITaskFactory taskFactory, 
            IDynamicBuildOrder buildOrder)
            : base(tasks, cruiser, taskFactory, prefabFactory)
        {
            Assert.IsNotNull(buildOrder);

            _buildOrder = buildOrder;

            _tasks.IsEmptyChanged += _tasks_IsEmptyChanged;

            // Trigger once to create first task from first prefab key
            _tasks_IsEmptyChanged(sender: null, e: EventArgs.Empty);
        }

        private void _tasks_IsEmptyChanged(object sender, EventArgs e)
        {
            if (_tasks.IsEmpty)
            {
                ITask newTask = CreateTask();

                if (newTask != null)
                {
                    _tasks.Add(newTask);
                }
                else
                {
                    // Have run out of building keys to create tasks for
                    _tasks.IsEmptyChanged -= _tasks_IsEmptyChanged;
                }
            }
        }

        /// <returns>
        /// The next task, or null, if we have run out of building keys to create tasks from.
        /// </returns>
        private ITask CreateTask()
        {
            while (_buildOrder.MoveNext())
            {
                IPrefabKey buildingKey = _buildOrder.Current;
				IBuildableWrapper<IBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(buildingKey);

                if (CanConstructBuilding(buildingWrapper.Buildable))
                {
                    return _taskFactory.CreateConstructBuildingTask(TaskPriority.Normal, buildingKey);
                }
			}

            return null;
        }

        private bool CanConstructBuilding(IBuilding building)
        {
            return _cruiser.SlotWrapper.IsSlotAvailable(building.SlotType);
        }

        public override void Dispose()
        {
            _tasks.IsEmptyChanged -= _tasks_IsEmptyChanged;
        }
    }
}
