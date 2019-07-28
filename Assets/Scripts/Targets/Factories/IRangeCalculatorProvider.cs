using BattleCruisers.Targets.Helpers;

namespace BattleCruisers.Targets.Factories
{
    public interface IRangeCalculatorProvider
    {
        IRangeCalculator BasicCalculator { get; }
        IRangeCalculator SizeInclusiveCalculator { get; }
    }
}