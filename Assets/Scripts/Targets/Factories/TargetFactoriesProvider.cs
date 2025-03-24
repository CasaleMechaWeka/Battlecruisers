namespace BattleCruisers.Targets.Factories
{
    public class TargetFactoriesProvider
    {
        public TargetRankerFactory RankerFactory { get; }
        public RangeCalculatorProvider RangeCalculatorProvider { get; }

        public TargetFactoriesProvider()
        {
            RankerFactory = new TargetRankerFactory();
            RangeCalculatorProvider = new RangeCalculatorProvider();
        }
    }
}