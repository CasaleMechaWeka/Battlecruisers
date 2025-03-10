using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
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

        // TODO: Add test
        public bool HasMastOffensive()
        {
            return HasSlotType(SlotType.Mast);
        }

        public bool HasBowOffensive()
        {
            return HasSlotType(SlotType.Bow);
        }

        private bool HasSlotType(SlotType slotType)
        {
            IList<BuildingKey> offensives = GetAvailableBuildings(BuildingCategory.Offence);

            foreach (BuildingKey offensive in offensives)
            {
                IBuilding building = _prefabFactory.GetBuildingWrapperPrefab(offensive).Buildable;
                if (building.SlotSpecification.SlotType == slotType)
                {
                    return true;
                }
            }
            return false;
        }

        public IList<BuildingKey> GetAvailableBuildings(BuildingCategory category)
		{
			return _gameModel.GetUnlockedBuildings(category);
		}
    }
}
