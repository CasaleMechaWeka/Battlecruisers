using System.Collections.Generic;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.TaskProducers
{
    public class ReplaceDestroyedBuildingsTaskProducer : BaseTaskProducer
    {
        private readonly IDictionary<IBuilding, IPrefabKey> _buildingToKey;

        public ReplaceDestroyedBuildingsTaskProducer(ITaskList tasks, ICruiserController cruiser, ITaskFactory taskFactory, IDictionary<IBuilding, IPrefabKey> buildingToKey)
            : base(tasks, cruiser, taskFactory)
        {
            _buildingToKey = buildingToKey;

            _cruiser.BuildingDestroyed += _cruiser_BuildingDestroyed;
        }

        private void _cruiser_BuildingDestroyed(object sender, BuildingDestroyedEventArgs e)
        {
            Assert.IsTrue(_buildingToKey.ContainsKey(e.DestroyedBuilding));

            IPrefabKey key = _buildingToKey[e.DestroyedBuilding];
            _tasks.Add(_taskFactory.CreateConstructBuildingTask(TaskPriority.High, key));
		}
    }
}
