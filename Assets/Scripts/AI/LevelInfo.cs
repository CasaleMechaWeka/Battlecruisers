using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;

namespace BattleCruisers.AI
{
    public class LevelInfo : ILevelInfo
	{
		private readonly IGameModel _gameModel;
		private readonly IPrefabFactory _prefabFactory;
        
		public ICruiserController AICruiser { get; }
		public ICruiserController PlayerCruiser { get; }

        public LevelInfo(
            ICruiserController aiCruiser,
            ICruiserController playerCruiser,
            IGameModel gameModel,
            IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(aiCruiser, playerCruiser, gameModel, prefabFactory);

            AICruiser = aiCruiser;
            PlayerCruiser = playerCruiser;
            _gameModel = gameModel;
            _prefabFactory = prefabFactory;
        }

        public bool CanConstructBuilding(BuildingKey buildingKey)
		{
			IBuilding building = _prefabFactory.GetBuildingWrapperPrefab(buildingKey).Buildable;

			return
                _gameModel.IsBuildingUnlocked(buildingKey)
                && building.NumOfDronesRequired <= AICruiser.DroneManager.NumOfDrones;
		}

        public IList<BuildingKey> GetAvailableBuildings(BuildingCategory category)
		{
			return _gameModel.GetUnlockedBuildings(category);
		}
	}
}
