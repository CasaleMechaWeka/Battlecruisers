namespace BattleCruisers.Targets.Factories
{
    public interface ITargetFactoriesProvider
    {
        ITargetFinderFactory FinderFactory { get; }
        ITargetFilterFactory FilterFactory { get; }
        ITargetRankerFactory RankerFactory { get; }
        ITargetHelperFactory HelperFactory { get; }
        IRangeCalculatorProvider RangeCalculatorProvider { get; }
    }
}