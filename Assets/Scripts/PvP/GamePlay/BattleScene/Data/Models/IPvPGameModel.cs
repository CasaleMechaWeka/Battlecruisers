using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    public interface IPvPGameModel
    {
        int NumOfLevelsCompleted { get; }
        long LifetimeDestructionScore { get; set; }
        long BestDestructionScore { get; set; }
        bool HasAttemptedTutorial { get; set; }
        bool FirstNonTutorialBattle { get; }
        PvPLoadout PlayerLoadout { get; set; }
        BattleResult LastBattleResult { get; set; }

        public bool PremiumEdition { get; set; }
        SettingsModel Settings { get; set; }
        int SelectedLevel { get; set; }
        HotkeysModel Hotkeys { get; }
        SkirmishModel Skirmish { get; set; }

        ReadOnlyCollection<PvPHullKey> UnlockedHulls { get; }
        ReadOnlyCollection<PvPBuildingKey> UnlockedBuildings { get; }
        ReadOnlyCollection<PvPUnitKey> UnlockedUnits { get; }
        ReadOnlyCollection<CompletedLevel> CompletedLevels { get; }

        PvPNewItems<PvPHullKey> NewHulls { get; }
        PvPNewItems<PvPBuildingKey> NewBuildings { get; }
        PvPNewItems<PvPUnitKey> NewUnits { get; }

        Dictionary<string, object> Analytics(string gameModeString, string type, bool lastSkirmishResult);

        void AddUnlockedHull(PvPHullKey hull);
        void AddUnlockedBuilding(PvPBuildingKey building);
        void AddUnlockedUnit(PvPUnitKey unit);
        void AddCompletedLevel(CompletedLevel completedLevel);

        IList<PvPBuildingKey> GetUnlockedBuildings(BuildingCategory buildingCategory);
        IList<PvPUnitKey> GetUnlockedUnits(UnitCategory unitCategory);

        bool IsUnitUnlocked(PvPUnitKey unitKey);
        bool IsBuildingUnlocked(PvPBuildingKey buildingKey);
    }
}
