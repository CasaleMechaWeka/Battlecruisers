namespace BattleCruisers.Targets.Factories
{
    public interface ITargetFactoriesProvider
    {
        ITargetProcessorFactory ProcessorFactory { get; }
        ITargetFinderFactory FinderFactory { get; }
        ITargetTrackerFactory TrackerFactory { get; }
        ITargetFilterFactory FilterFactory { get; }
        ITargetRankerFactory RankerFactory { get; }
        ITargetProviderFactory ProviderFactory { get; }
        ITargetHelperFactory HelperFactory { get; }
    }
}