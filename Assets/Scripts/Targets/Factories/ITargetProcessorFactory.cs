using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetProcessorFactory
    {
        ITargetProcessor BomberTargetProcessor { get; }
        ITargetProcessor OffensiveBuildableTargetProcessor { get; }
        ITargetProcessor IonCannonTargetProcessor { get; }
        ITargetProcessor CreateTargetProcessor(IRankedTargetTracker rankedTargetTracker);
    }
}