namespace BattleCruisers.Targets.Factories
{
    public interface ITargetFactoriesProvider
    {
        // FELIX  Common
        ITargetFinderFactory FinderFactory { get; }
        ITargetFilterFactory FilterFactory { get; }
        ITargetRankerFactory RankerFactory { get; }
        ITargetHelperFactory HelperFactory { get; }
        IRangeCalculatorProvider RangeCalculatorProvider { get; }

        // FELIX  Cruiser specific
        ITargetProviderFactory ProviderFactory { get; }
        ITargetProcessorFactory ProcessorFactory { get; }
        ITargetDetectorFactory TargetDetectorFactory { get; }
    }
}