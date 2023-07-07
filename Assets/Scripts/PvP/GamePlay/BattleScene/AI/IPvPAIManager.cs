using BattleCruisers.AI;
using BattleCruisers.Data.Settings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public interface IPvPAIManager
    {
        IPvPArtificialIntelligence CreateAI(ILevelInfo levelInfo, Difficulty difficulty);
    }
}