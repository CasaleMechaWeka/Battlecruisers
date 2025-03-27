using BattleCruisers.Data.Settings;

namespace BattleCruisers.AI
{
    public interface IAIManager
    {
        IArtificialIntelligence CreateAI(LevelInfo levelInfo, Difficulty difficulty);
    }
}
