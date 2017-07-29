using System.Collections.Generic;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.TaskProducers
{
    public class ReplaceDestroyedBuildingsTaskProducer : BaseTaskProducer
    {
        private readonly IDictionary<string, IPrefabKey> _buildingNamesToKeys;

        public ReplaceDestroyedBuildingsTaskProducer(ITaskList tasks, ICruiserController cruiser, 
            IPrefabFactory prefabFactory, ITaskFactory taskFactory, IList<IPrefabKey> buildingKeys)
            : base(tasks, cruiser, taskFactory, prefabFactory)
        {
            _buildingNamesToKeys = CreateMap(buildingKeys);

            _cruiser.BuildingDestroyed += _cruiser_BuildingDestroyed;
        }

        private IDictionary<string, IPrefabKey> CreateMap(IList<IPrefabKey> buildingKeys)
        {
            IDictionary<string, IPrefabKey> buildingNamesToKeys = new Dictionary<string, IPrefabKey>();

            foreach (IPrefabKey key in buildingKeys)
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
    }
}
