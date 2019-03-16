using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI
{
    public class LevelInfo : ILevelInfo
	{
		private readonly IStaticData _staticData;
		private readonly IPrefabFactory _prefabFactory;
        
        public int LevelNum { get; }
		public ICruiserController AICruiser { get; }
		public ICruiserController PlayerCruiser { get; }

        public LevelInfo(
            ICruiserController aiCruiser,
            ICruiserController playerCruiser,
            IStaticData staticData,
            IPrefabFactory prefabFactory,
            int levelNum)
        {
            Helper.AssertIsNotNull(aiCruiser, playerCruiser, staticData, prefabFactory);

            AICruiser = aiCruiser;
            PlayerCruiser = playerCruiser;
            _staticData = staticData;
            _prefabFactory = prefabFactory;
            LevelNum = levelNum;
        }

        public bool CanConstructBuilding(BuildingKey buildingKey)
		{
			IBuilding building = _prefabFactory.GetBuildingWrapperPrefab(buildingKey).Buildable;

			return
                _staticData.IsBuildingAvailable(buildingKey, LevelNum)
                && building.NumOfDronesRequired <= AICruiser.DroneManager.NumOfDrones;
		}

        public IList<BuildingKey> GetAvailableBuildings(BuildingCategory category)
		{
			return _staticData.GetAvailableBuildings(category, LevelNum);
		}
	}
}
