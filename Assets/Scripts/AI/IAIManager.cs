using BattleCruisers.Data.Settings;

namespace BattleCruisers.AI
{
    public interface IAIManager
    {
        IArtificialIntelligence CreateAI(ILevelInfo levelInfo, Difficulty difficulty);
	}
}
