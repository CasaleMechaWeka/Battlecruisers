namespace BattleCruisers.Targets.Factories
{
    public class TargetFactoriesProvider
    {
        public TargetFilterFactory FilterFactory { get; }
        public ITargetRankerFactory RankerFactory { get; }
        public TargetHelperFactory HelperFactory { get; }
        public RangeCalculatorProvider RangeCalculatorProvider { get; }

        public TargetFactoriesProvider()
        {
            FilterFactory = new TargetFilterFactory();
            RankerFactory = new TargetRankerFactory();
            HelperFactory = new TargetHelperFactory();
            RangeCalculatorProvider = new RangeCalculatorProvider();
        }
    }
}