namespace BattleCruisers.Targets.Factories
{
    public class TargetFactoriesProvider
    {
        public TargetFilterFactory FilterFactory { get; }
        public ITargetRankerFactory RankerFactory { get; }
        public RangeCalculatorProvider RangeCalculatorProvider { get; }

        public TargetFactoriesProvider()
        {
            FilterFactory = new TargetFilterFactory();
            RankerFactory = new TargetRankerFactory();
            RangeCalculatorProvider = new RangeCalculatorProvider();
        }
    }
}