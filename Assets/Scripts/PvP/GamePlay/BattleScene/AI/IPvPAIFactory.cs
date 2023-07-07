using BattleCruisers.AI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public interface IPvPAIFactory
    {
        IPvPArtificialIntelligence CreateBasicAI(ILevelInfo levelInfo);
        IPvPArtificialIntelligence CreateAdaptiveAI(ILevelInfo levelInfo);
    }
}
