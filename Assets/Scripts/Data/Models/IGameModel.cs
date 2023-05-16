using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Models
{
    public interface IGameModel
    {
        int NumOfLevelsCompleted { get; }
        long LifetimeDestructionScore { get; set; }
        long BestDestructionScore { get; set; }
        bool HasAttemptedTutorial { get; set; }
        bool FirstNonTutorialBattle { get; }
        Loadout PlayerLoadout { get; set; }
        BattleResult LastBattleResult { get; set; }

        public bool PremiumEdition { get; set; }
        SettingsModel Settings { get; set; }
        int SelectedLevel { get; set; }
        int SelectedPvPLevel { get; set; }
        HotkeysModel Hotkeys { get; }
        SkirmishModel Skirmish { get; set; }

        ReadOnlyCollection<HullKey> UnlockedHulls { get; }
        ReadOnlyCollection<BuildingKey> UnlockedBuildings { get; }
        ReadOnlyCollection<UnitKey> UnlockedUnits { get; }
        ReadOnlyCollection<CompletedLevel> CompletedLevels { get; }

        NewItems<HullKey> NewHulls { get; }
        NewItems<BuildingKey> NewBuildings { get; }
        NewItems<UnitKey> NewUnits { get; }

        Dictionary<string, object> Analytics(string gameModeString, string type, bool lastSkirmishResult);

        void AddUnlockedHull(HullKey hull);
        void AddUnlockedBuilding(BuildingKey building);
        void AddUnlockedUnit(UnitKey unit);
        void AddCompletedLevel(CompletedLevel completedLevel);

        IList<BuildingKey> GetUnlockedBuildings(BuildingCategory buildingCategory);
        IList<UnitKey> GetUnlockedUnits(UnitCategory unitCategory);

        bool IsUnitUnlocked(UnitKey unitKey);
        bool IsBuildingUnlocked(BuildingKey buildingKey);
    }
}
