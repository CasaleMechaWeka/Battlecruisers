namespace BattleCruisers.Targets.Factories
{
    public interface ICruiserTargetFactoriesProvider
    {
        ITargetProcessorFactory ProcessorFactory { get; }
        ITargetDetectorFactory TargetDetectorFactory { get; }
        ITargetProviderFactory TargetProviderFactory { get; }
        ITargetTrackerFactory TrackerFactory { get; }
    }
}