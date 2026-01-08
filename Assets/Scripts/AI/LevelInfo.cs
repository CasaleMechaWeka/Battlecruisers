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
        public Cruiser AICruiser { get; }
        public Cruiser PlayerCruiser { get; }
        public int LevelNum { get; }
        public bool IsSequencerBattle { get; }

        public LevelInfo(
            Cruiser aiCruiser,
            Cruiser playerCruiser,
            int levelNum,
            bool isSequencerBattle)
        {
            Helper.AssertIsNotNull(aiCruiser, playerCruiser);

            AICruiser = aiCruiser;
            PlayerCruiser = playerCruiser;
            LevelNum = levelNum;
            IsSequencerBattle = isSequencerBattle;
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
            IList<BuildingKey> offensives = DataProvider.GameModel.GetUnlockedBuildings(BuildingCategory.Offence);

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
    }
}
