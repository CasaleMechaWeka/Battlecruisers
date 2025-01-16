using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.ShopScreen;

namespace BattleCruisers.Data.Models
{
    public interface IGameModel
    {
        long Coins { get; set; }
        long Credits { get; set; }
        int CoinsChange { get; set; }
        int CreditsChange { get; set; }
        string PlayerName { get; set; }
        List<int> PurchasedExos { get; }
        List<int> PurchasedHeckles { get; }
        List<int> PurchasedBodykits { get; }
        List<int> PurchasedVariants { get; }
        int GameMap { get; set; }
        float BattleWinScore { get; set; }
        string QueueName { get; set; }
        List<Arena> Arenas { get; set; }
        Dictionary<string, int> GameConfigs { get; set; }
        List<HeckleData> OutstandingHeckleTransactions { get; set; }
        List<CaptainData> OutstandingCaptainTransactions { get; set; }
        List<BodykitData> OutstandingBodykitTransactions { get; set; }
        List<VariantData> OutstandingVariantTransactions { get; set; }
        bool HasSyncdShop { get; set; }
        int NumOfLevelsCompleted { get; }
        int NumOfSideQuestsCompleted { get; }
        int ID_Bodykit_AIbot { get; set; }
        long LifetimeDestructionScore { get; set; }
        long BestDestructionScore { get; set; }
        bool HasAttemptedTutorial { get; set; }
        bool FirstNonTutorialBattle { get; }
        Loadout PlayerLoadout { get; set; }
        BattleResult LastBattleResult { get; set; }
        /*        int RankData { get; set; }*/
        public bool PremiumEdition { get; set; }
        SettingsModel Settings { get; set; }
        int SelectedLevel { get; set; }
        int SelectedPvPLevel { get; set; }
        int SelectedSideQuestID { get; set; }
        HotkeysModel Hotkeys { get; }
        SkirmishModel Skirmish { get; set; }
        CoinBattleModel CoinBattle { get; set; }

        ReadOnlyCollection<HullKey> UnlockedHulls { get; }
        ReadOnlyCollection<BuildingKey> UnlockedBuildings { get; }
        ReadOnlyCollection<UnitKey> UnlockedUnits { get; }
        ReadOnlyCollection<CompletedLevel> CompletedLevels { get; }
        ReadOnlyCollection<CompletedLevel> CompletedSideQuests { get; }
        //ReadOnlyCollection<CaptainExoKey> UnlockedCaptainExos { get; }

        NewItems<HullKey> NewHulls { get; }
        NewItems<BuildingKey> NewBuildings { get; }
        NewItems<UnitKey> NewUnits { get; }
        //NewItems<CaptainExoKey> NewCaptainExos { get; }

        Dictionary<string, object> Analytics(string gameModeString, string type, bool lastSkirmishResult);

        void AddUnlockedHull(HullKey hull);
        void AddUnlockedBuilding(BuildingKey building);
        void AddUnlockedUnit(UnitKey unit);
        void AddCompletedLevel(CompletedLevel completedLevel);
        void AddCompletedSideQuest(CompletedLevel completedSideQuest);
        bool IsSideQuestCompleted(int sideQuestID);

        IList<BuildingKey> GetUnlockedBuildings(BuildingCategory buildingCategory);
        IList<UnitKey> GetUnlockedUnits(UnitCategory unitCategory);

        bool IsUnitUnlocked(UnitKey unitKey);
        bool IsBuildingUnlocked(BuildingKey buildingKey);

        void AddExo(int index);
        void RemoveExo(int id);

        void AddHeckle(int index);
        void RemoveHeckle(int id);

        void AddBodykit(int index);
        void RemoveBodykit(int id);

        void AddVariant(int index);
        void RemoveVariant(int id);
    }
}
