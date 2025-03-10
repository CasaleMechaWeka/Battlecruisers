namespace BattleCruisers.Targets.Factories
{
    public class TargetFactoriesProvider : ITargetFactoriesProvider
    {
        public ITargetFinderFactory FinderFactory { get; }
        public ITargetFilterFactory FilterFactory { get; }
        public ITargetRankerFactory RankerFactory { get; }
        public ITargetHelperFactory HelperFactory { get; }
        public IRangeCalculatorProvider RangeCalculatorProvider { get; }

        public TargetFactoriesProvider()
        {
            FinderFactory = new TargetFinderFactory();
            FilterFactory = new TargetFilterFactory();
            RankerFactory = new TargetRankerFactory();
            HelperFactory = new TargetHelperFactory();
            RangeCalculatorProvider = new RangeCalculatorProvider();
        }
    }
}