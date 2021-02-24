using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing.Spawners
{
    public class BuildingSpawner : BuildableSpawner
    {
        public BuildingSpawner(Helper helper) 
            : base(helper)
        {
        }

        protected override IBuildable SpawnBuildable(IPrefabKey buildableKey, BuildableInitialisationArgs args)
        {
            IBuildableWrapper<IBuilding> buildingWrapperPrefab = _helper.PrefabFactory.GetBuildingWrapperPrefab(buildableKey);
            BuildableWrapper<IBuilding> buildingWrapper = Object.Instantiate(buildingWrapperPrefab.UnityObject);
            buildingWrapper.gameObject.SetActive(true);
            buildingWrapper.StaticInitialise(_helper.CommonStrings);
            IBuilding building = buildingWrapper.Buildable;
            _helper.InitialiseBuilding(building, args);
            return building;
        }
    }
}
