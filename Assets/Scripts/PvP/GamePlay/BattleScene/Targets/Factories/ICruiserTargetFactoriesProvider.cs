namespace BattleCruisers.Targets.Factories
{
    public interface ICruiserTargetFactoriesProvider
    {
        public ITargetProcessorFactory ProcessorFactory { get; }
        public TargetTrackerFactory TrackerFactory { get; }
        public TargetDetectorFactory DetectorFactory { get; }
    }
}