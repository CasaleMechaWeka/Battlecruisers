using BattleCruisers.AI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public interface IPvPAIFactory
    {
        IPvPArtificialIntelligence CreateBasicAI(IPvPLevelInfo levelInfo);
        IPvPArtificialIntelligence CreateAdaptiveAI(IPvPLevelInfo levelInfo);
    }
}
