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
        Task<GameModel> CloudLoad();

        Task<bool> SyncCoinsToCloud(IDataProvider dataProvider);
        Task<bool> SyncCoinsFromCloud(IDataProvider dataProvider);

        Task<bool> SyncCreditsToCloud(IDataProvider dataProvider);
        Task<bool> SyncCreditsFromCloud(IDataProvider dataProvider);
    }
}
