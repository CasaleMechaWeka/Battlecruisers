using BattleCruisers.Targets.Helpers;

namespace BattleCruisers.Targets.Factories
{
    public class RangeCalculatorProvider
    {
        public RangeCalculatorProvider()
        {
            BasicCalculator = new BasicCalculator();
            SizeInclusiveCalculator = new SizeInclusiveCalculator();
        }

        public IRangeCalculator BasicCalculator { get; }
        public IRangeCalculator SizeInclusiveCalculator { get; }
    }
}