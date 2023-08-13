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

        Task SyncCoinsToCloud(IDataProvider dataProvider);
        Task SyncCoinsFromCloud(IDataProvider dataProvider);

        Task SyncCreditsToCloud(IDataProvider dataProvider);
        Task SyncCreditsFromCloud(IDataProvider dataProvider);
    }
}
