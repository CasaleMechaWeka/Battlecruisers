using System.Collections.Generic;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.TaskProducers
{
    public class ReplaceDestroyedBuildingsTaskProducer : TaskProducerBase
    {
        private readonly IDictionary<string, BuildingKey> _buildingNamesToKeys;

        public ReplaceDestroyedBuildingsTaskProducer(
            ITaskList tasks, 
            ICruiserController cruiser, 
            IPrefabFactory prefabFactory, 
            ITaskFactory taskFactory, 
            IList<BuildingKey> buildingKeys)
            : base(tasks, cruiser, taskFactory, prefabFactory)
        {
            _buildingNamesToKeys = CreateMap(buildingKeys);

            _cruiser.BuildingDestroyed += _cruiser_BuildingDestroyed;
        }

        private IDictionary<string, BuildingKey> CreateMap(IList<BuildingKey> buildingKeys)
        {
            IDictionary<string, BuildingKey> buildingNamesToKeys = new Dictionary<string, BuildingKey>();

            foreach (BuildingKey key in buildingKeys)
            {
                IBuildableWrapper<IBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(key);
                Assert.IsFalse(buildingNamesToKeys.ContainsKey(buildingWrapper.Buildable.Name));
                buildingNamesToKeys.Add(buildingWrapper.Buildable.Name, key);
            }

            return buildingNamesToKeys;
        }

        private void _cruiser_BuildingDestroyed(object sender, BuildingDestroyedEventArgs e)
        {
            Assert.IsTrue(_buildingNamesToKeys.ContainsKey(e.DestroyedBuilding.Name));

            IPrefabKey key = _buildingNamesToKeys[e.DestroyedBuilding.Name];
            _tasks.Add(_taskFactory.CreateConstructBuildingTask(TaskPriority.High, key));
		}

        public override void Dispose()
        {
            _cruiser.BuildingDestroyed -= _cruiser_BuildingDestroyed;
        }
    }
}
