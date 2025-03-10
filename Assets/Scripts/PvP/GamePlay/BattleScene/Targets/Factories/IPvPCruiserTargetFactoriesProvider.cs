using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPCruiserTargetFactoriesProvider
    {
        ITargetProcessorFactory ProcessorFactory { get; }
        ITargetDetectorFactory DetectorFactory { get; }
        IPvPTargetProviderFactory ProviderFactory { get; }
        ITargetTrackerFactory TrackerFactory { get; }
    }
}