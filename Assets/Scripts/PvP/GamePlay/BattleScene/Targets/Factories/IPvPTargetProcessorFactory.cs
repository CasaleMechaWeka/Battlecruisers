
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetProcessorFactory
    {
        IPvPTargetProcessor BomberTargetProcessor { get; }
        IPvPTargetProcessor OffensiveBuildableTargetProcessor { get; }
        IPvPTargetProcessor StaticTargetProcessor { get; }
        IPvPTargetProcessor CreateTargetProcessor(IPvPRankedTargetTracker rankedTargetTracker);
    }
}
