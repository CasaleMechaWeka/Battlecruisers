using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPCruiserTargetFactoriesProvider
    {
        ITargetProcessorFactory ProcessorFactory { get; }
        IPvPTargetDetectorFactory DetectorFactory { get; }
        IPvPTargetProviderFactory ProviderFactory { get; }
        IPvPTargetTrackerFactory TrackerFactory { get; }
    }
}