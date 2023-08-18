using BattleCruisers.Data.Models;
using System.Threading.Tasks;
using UnityEngine;

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
        Task<GameModel> CloudLoad(GameModel game);

        Task<bool> SyncCoinsToCloud(IDataProvider dataProvider);
        Task<bool> SyncCurrencyFromCloud(IDataProvider dataProvider);
        Task<bool> SyncInventoryFromCloud(IDataProvider dataProvider);

        Task<bool> SyncCreditsToCloud(IDataProvider dataProvider);
    }
}
