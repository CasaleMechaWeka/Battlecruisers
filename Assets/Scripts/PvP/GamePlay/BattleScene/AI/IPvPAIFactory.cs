using BattleCruisers.AI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public interface IPvPAIFactory
    {
        IArtificialIntelligence CreateBasicAI(IPvPLevelInfo levelInfo);
        IArtificialIntelligence CreateAdaptiveAI(IPvPLevelInfo levelInfo);
    }
}
