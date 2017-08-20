using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public class BuildingKeyHelper : IBuildingKeyHelper
	{
		private readonly IDroneManager _droneManager;
		private readonly IStaticData _staticData;
        private readonly IPrefabFactory _prefabFactory;
        private readonly int _levelNum;

        public BuildingKeyHelper(
            IDroneManager droneManager,
            IStaticData staticData,
            IPrefabFactory prefabFactory,
            int levelNum)
        {
            Helper.AssertIsNotNull(droneManager, staticData, prefabFactory);

            _droneManager = droneManager;
            _staticData = staticData;
            _prefabFactory = prefabFactory;
            _levelNum = levelNum;
        }

		public bool CanConstructBuilding(IPrefabKey buildingKey)
        {
            IBuilding building = _prefabFactory.GetBuildingWrapperPrefab(buildingKey).Buildable;

			return
				_staticData.IsBuildableAvailable(buildingKey, _levelNum)
				&& building.NumOfDronesRequired <= _droneManager.NumOfDrones;
        }
	}
}
