using BattleCruisers.Targets.Helpers;

namespace BattleCruisers.Targets.Factories
{
    public class RangeCalculatorProvider : IRangeCalculatorProvider
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