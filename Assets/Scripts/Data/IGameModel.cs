using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.Data
{
    public interface IGameModel
    {
        int NumOfLevelsCompleted { get; }
        Loadout PlayerLoadout { get; set; }
        BattleResult LastBattleResult { get; set; }

        ReadOnlyCollection<HullKey> UnlockedHulls { get; }
        ReadOnlyCollection<BuildingKey> UnlockedBuildings { get; }
        ReadOnlyCollection<UnitKey> UnlockedUnits { get; }

        void AddUnlockedHull(HullKey hull);
        void AddUnlockedBuilding(BuildingKey building);
        void AddUnlockedUnit(UnitKey unit);

        IList<BuildingKey> GetUnlockedBuildings(BuildingCategory buildingCategory);
    }
}
