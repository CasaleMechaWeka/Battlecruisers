using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Models
{
    public interface IGameModel
    {
        bool HasAttemptedTutorial { get; set; }
        int NumOfLevelsCompleted { get; set; }
        Loadout PlayerLoadout { get; set; }
        BattleResult LastBattleResult { get; set; }

        ReadOnlyCollection<HullKey> UnlockedHulls { get; }
        ReadOnlyCollection<BuildingKey> UnlockedBuildings { get; }
        ReadOnlyCollection<UnitKey> UnlockedUnits { get; }

        void AddUnlockedHull(HullKey hull);
        void AddUnlockedBuilding(BuildingKey building);
        void AddUnlockedUnit(UnitKey unit);

        IList<BuildingKey> GetUnlockedBuildings(BuildingCategory buildingCategory);
        IList<UnitKey> GetUnlockedUnits(UnitCategory unitCategory);
    }
}
