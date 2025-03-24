using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPCruiserTargetFactoriesProvider
    {
        ITargetProcessorFactory ProcessorFactory { get; }
        TargetDetectorFactory DetectorFactory { get; }
        IPvPTargetProviderFactory ProviderFactory { get; }
        TargetTrackerFactory TrackerFactory { get; }
    }
}