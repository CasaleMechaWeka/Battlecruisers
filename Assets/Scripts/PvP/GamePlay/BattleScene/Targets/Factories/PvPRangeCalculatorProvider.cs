using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.Helpers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPRangeCalculatorProvider : IRangeCalculatorProvider
    {
        public PvPRangeCalculatorProvider()
        {
            BasicCalculator = new BasicCalculator();
            SizeInclusiveCalculator = new SizeInclusiveCalculator();
        }

        public IRangeCalculator BasicCalculator { get; }
        public IRangeCalculator SizeInclusiveCalculator { get; }
    }
}