using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetProcessorFactory
    {
        ITargetProcessor BomberTargetProcessor { get; }
        ITargetProcessor OffensiveBuildableTargetProcessor { get; }
        ITargetProcessor StaticTargetProcessor { get; }
        ITargetProcessor CreateTargetProcessor(IPvPRankedTargetTracker rankedTargetTracker);
    }
}
