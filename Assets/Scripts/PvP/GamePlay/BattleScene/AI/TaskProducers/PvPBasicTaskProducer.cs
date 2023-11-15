using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers
{
    /// <summary>
    /// Lazily creates tasks when the task list becomes empty.
    /// </summary>
    public class PvPBasicTaskProducer : PvPTaskProducer
    {
        private readonly IPvPDynamicBuildOrder _buildOrder;

        public PvPBasicTaskProducer(
            IPvPTaskList tasks,
            IPvPCruiserController cruiser,
            IPvPPrefabFactory prefabFactory,
            IPvPTaskFactory taskFactory,
            IPvPDynamicBuildOrder buildOrder)
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
                IPvPPrioritisedTask newTask = CreateTask();

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
        private IPvPPrioritisedTask CreateTask()
        {
            while (_buildOrder.MoveNext())
            {
                IPvPPrefabKey buildingKey = _buildOrder.Current;
                IPvPBuildableWrapper<IPvPBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(buildingKey);

                if (CanConstructBuilding(buildingWrapper.Buildable))
                {
                    return _taskFactory.CreateConstructBuildingTask(PvPTaskPriority.Low, buildingKey);
                }
            }

            return null;
        }

        private bool CanConstructBuilding(IPvPBuilding building)
        {
            return _cruiser.SlotAccessor.IsSlotAvailable(building.SlotSpecification);
        }

        public override void DisposeManagedState()
        {
            _tasks.IsEmptyChanged -= _tasks_IsEmptyChanged;
            base.DisposeManagedState();
        }
    }
}
