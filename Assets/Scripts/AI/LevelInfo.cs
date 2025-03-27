using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;

namespace BattleCruisers.AI
{
    public class LevelInfo
    {
        private readonly PrefabFactory _prefabFactory;

        public ICruiserController AICruiser { get; }
        public ICruiserController PlayerCruiser { get; }

        public LevelInfo(
            ICruiserController aiCruiser,
            ICruiserController playerCruiser,
            PrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(aiCruiser, playerCruiser, prefabFactory);

            AICruiser = aiCruiser;
            PlayerCruiser = playerCruiser;
            _prefabFactory = prefabFactory;
        }

        public bool CanConstructBuilding(BuildingKey buildingKey)
        {
            IBuilding building = _prefabFactory.GetBuildingWrapperPrefab(buildingKey).Buildable;

            return
                DataProvider.GameModel.IsBuildingUnlocked(buildingKey)
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
            return DataProvider.GameModel.GetUnlockedBuildings(category);
        }
    }
}
