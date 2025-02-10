using BattleCruisers.AI;
using BattleCruisers.Data.Settings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public interface IPvPAIManager
    {
        IArtificialIntelligence CreateAI(IPvPLevelInfo levelInfo, Difficulty difficulty);
    }
}