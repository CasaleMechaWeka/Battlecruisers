namespace BattleCruisers.Targets.Factories
{
    public interface ICruiserTargetFactoriesProvider
    {
        ITargetProcessorFactory ProcessorFactory { get; }
        ITargetDetectorFactory DetectorFactory { get; }
        ITargetProviderFactory ProviderFactory { get; }
        ITargetTrackerFactory TrackerFactory { get; }
    }
}