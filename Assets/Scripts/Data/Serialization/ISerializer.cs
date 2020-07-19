using BattleCruisers.Data.Models;

namespace BattleCruisers.Data.Serialization
{
    public interface ISerializer
    {
        bool DoesSavedGameExist();
        void SaveGame(GameModel game);
        GameModel LoadGame();
        void DeleteSavedGame();
    }
}
