namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPCruiserTargetFactoriesProvider
    {
        IPvPTargetProcessorFactory ProcessorFactory { get; }
        IPvPTargetDetectorFactory DetectorFactory { get; }
        IPvPTargetProviderFactory ProviderFactory { get; }
        IPvPTargetTrackerFactory TrackerFactory { get; }
    }
}