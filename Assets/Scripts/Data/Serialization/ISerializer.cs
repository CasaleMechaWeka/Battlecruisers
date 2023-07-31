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
        public Task CloudSave(GameModel game);
        public Task<GameModel> CloudLoad();
    }
}
