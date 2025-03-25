using BattleCruisers.Data.Models;
using System.Threading.Tasks;

namespace BattleCruisers.Data.Serialization
{
    public interface ISerializer
    {
        bool DoesSavedGameExist();
        void SaveGame(GameModel game);
        GameModel LoadGame();
        void DeleteSavedGame();
        object DeserializeGameModel(string gameModelJSON);
        string SerializeGameModel(object gameModel);
        Task CloudSave(GameModel game);
        Task<SaveGameModel> CloudLoad(GameModel game);
        void DeleteCloudSave();
        Task<bool> SyncCoinsToCloud(DataProvider dataProvider);
        Task<bool> SyncCurrencyFromCloud(DataProvider dataProvider);
        Task<bool> SyncInventoryFromCloud(DataProvider dataProvider);

        Task<bool> SyncCreditsToCloud(DataProvider dataProvider);
    }
}
