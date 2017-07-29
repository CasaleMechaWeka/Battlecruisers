using System.Collections.Generic;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.TaskProducers
{
    public class ReplaceDestroyedBuildingsTaskProducer : BaseTaskProducer
    {
        private readonly IDictionary<IBuilding, IPrefabKey> _buildingsToKeys;

        public ReplaceDestroyedBuildingsTaskProducer(ITaskList tasks, ICruiserController cruiser, 
            IPrefabFactory prefabFactory, ITaskFactory taskFactory, IList<IPrefabKey> unlockedBuildingKeys)
            : base(tasks, cruiser, taskFactory, prefabFactory)
        {
            _buildingsToKeys = CreateBuildingToPrefabKeyMap(unlockedBuildingKeys);

            _cruiser.BuildingDestroyed += _cruiser_BuildingDestroyed;
        }

        private IDictionary<IBuilding, IPrefabKey> CreateBuildingToPrefabKeyMap(IList<IPrefabKey> buildingKeys)
        {
            IDictionary<IBuilding, IPrefabKey> buildingsToKeys = new Dictionary<IBuilding, IPrefabKey>();

            foreach (IPrefabKey key in buildingKeys)
            {
                IBuildableWrapper<IBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(key);
                Assert.IsFalse(buildingsToKeys.ContainsKey(buildingWrapper.Buildable));
                buildingsToKeys.Add(buildingWrapper.Buildable, key);
            }

            return buildingsToKeys;
        }

        private void _cruiser_BuildingDestroyed(object sender, BuildingDestroyedEventArgs e)
        {
            Assert.IsTrue(_buildingsToKeys.ContainsKey(e.DestroyedBuilding));

            IPrefabKey key = _buildingsToKeys[e.DestroyedBuilding];
            _tasks.Add(_taskFactory.CreateConstructBuildingTask(TaskPriority.High, key));
		}
    }
}
