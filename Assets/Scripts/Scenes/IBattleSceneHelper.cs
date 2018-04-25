using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models;

namespace BattleCruisers.Scenes
{
    public interface IBattleSceneHelper
    {
        ILoadout GetPlayerLoadout();
        void CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum);
    }
}
