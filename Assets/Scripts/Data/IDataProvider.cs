using System.Collections.Generic;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Threading.Tasks;
using Unity.Services.Economy.Model;

namespace BattleCruisers.Data
{
    public interface IDataProvider
    {
        // Local Saving:
        IList<ILevel> Levels { get; }
        IGameModel GameModel { get; }
        ILockedInformation LockedInfo { get; }
        IStaticData StaticData { get; }
        ISettingsManager SettingsManager { get; }

        List<VirtualPurchaseDefinition> m_VirtualPurchaseDefinitions { get; set; }
        public VirtualShopConfig virtualShopConfig { get; set; }

        ILevel GetLevel(int levelNum);
        IPvPLevel GetPvPLevel(Map map);
        void SaveGame();

        /// <summary>
        /// Deletes the saved game and resets all settings to their default values.
        /// Designed for user playtests, so users can start with a clean slate.
        /// </summary>
        void Reset();
/*        Task<int> GetCaptainCost(int index);
        Task<int> GetHeckleCost(int index);*/
        // Cloud Saving:
        Task CloudSave();
        Task CloudLoad();

        Task<bool> SyncCoinsFromCloud();
        Task<bool> SyncCoinsToCloud();

        Task<bool> SyncCreditsFromCloud();
        Task<bool> SyncCreditsToCloud();
        Task SyncCaptainsCost();

        Task<bool> PurchaseCaptain(int index);
        /*        Task RefreshEconomyConfiguration();
                Task FetchConfigs();*/
    }
}
