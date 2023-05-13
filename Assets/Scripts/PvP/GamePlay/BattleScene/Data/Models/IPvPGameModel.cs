using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
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
        PvPBattleResult LastBattleResult { get; set; }

        public bool PremiumEdition { get; set; }
        SettingsModel Settings { get; set; }
        int SelectedLevel { get; set; }
        PvPHotkeysModel Hotkeys { get; }
        PvPSkirmishModel Skirmish { get; set; }

        ReadOnlyCollection<PvPHullKey> UnlockedHulls { get; }
        ReadOnlyCollection<PvPBuildingKey> UnlockedBuildings { get; }
        ReadOnlyCollection<PvPUnitKey> UnlockedUnits { get; }
        ReadOnlyCollection<PvPCompletedLevel> CompletedLevels { get; }

        PvPNewItems<PvPHullKey> NewHulls { get; }
        PvPNewItems<PvPBuildingKey> NewBuildings { get; }
        PvPNewItems<PvPUnitKey> NewUnits { get; }

        Dictionary<string, object> Analytics(string gameModeString, string type, bool lastSkirmishResult);

        void AddUnlockedHull(PvPHullKey hull);
        void AddUnlockedBuilding(PvPBuildingKey building);
        void AddUnlockedUnit(PvPUnitKey unit);
        void AddCompletedLevel(PvPCompletedLevel completedLevel);

        IList<PvPBuildingKey> GetUnlockedBuildings(PvPBuildingCategory buildingCategory);
        IList<PvPUnitKey> GetUnlockedUnits(PvPUnitCategory unitCategory);

        bool IsUnitUnlocked(PvPUnitKey unitKey);
        bool IsBuildingUnlocked(PvPBuildingKey buildingKey);
    }
}
