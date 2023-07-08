using BattleCruisers.Data.Settings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public interface IPvPAIManager
    {
        IPvPArtificialIntelligence CreateAI(IPvPLevelInfo levelInfo, Difficulty difficulty);
    }
}