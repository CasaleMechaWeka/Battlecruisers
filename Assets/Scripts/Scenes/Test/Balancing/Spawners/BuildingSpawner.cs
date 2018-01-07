using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Spawners
{
    public class BuildingSpawner : BuildableSpawner
    {
        public BuildingSpawner(IPrefabFactory prefabFactory, Helper helper) 
            : base(prefabFactory, helper)
        {
        }

        protected override IBuildable SpawnBuildable(IPrefabKey buildableKey, BuildableInitialisationArgs args)
        {
            IBuildableWrapper<IBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(buildableKey);
            IBuilding building = _prefabFactory.CreateBuilding(buildingWrapper);
            _helper.InitialiseBuilding(building, args);
            return building;
        }
    }
}
