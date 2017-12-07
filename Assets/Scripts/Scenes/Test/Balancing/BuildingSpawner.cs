using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    // FELIX  Use in DefenceBuildingBalancingTest :)
    public class BuildingSpawner : BuildableSpawner
    {
        public BuildingSpawner(IPrefabFactory prefabFactory, TestUtils.Helper helper) 
            : base(prefabFactory, helper)
        {
        }

        protected override IBuildable SpawnBuildable(IPrefabKey buildableKey, Direction facingDirection)
        {
            IBuildableWrapper<IBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(buildableKey);
            IBuilding building = _prefabFactory.CreateBuilding(buildingWrapper);
            _helper.InitialiseBuilding(building, Faction.Reds, parentCruiserDirection: facingDirection);
            return building;
        }
    }
}
