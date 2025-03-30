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

        public ICruiserController AICruiser { get; }
        public ICruiserController PlayerCruiser { get; }

        public LevelInfo(
            ICruiserController aiCruiser,
            ICruiserController playerCruiser)
        {
            Helper.AssertIsNotNull(aiCruiser, playerCruiser);

            AICruiser = aiCruiser;
            PlayerCruiser = playerCruiser;
        }

        public bool CanConstructBuilding(BuildingKey buildingKey)
        {
            IBuilding building = PrefabFactory.GetBuildingWrapperPrefab(buildingKey).Buildable;

            return
                DataProvider.GameModel.IsBuildingUnlocked(buildingKey)
                && building.NumOfDronesRequired <= AICruiser.DroneManager.NumOfDrones;
        }

        public bool HasSlotType(SlotType slotType)
        {
            IList<BuildingKey> offensives = GetAvailableBuildings(BuildingCategory.Offence);

            foreach (BuildingKey offensive in offensives)
            {
                IBuilding building = PrefabFactory.GetBuildingWrapperPrefab(offensive).Buildable;
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
