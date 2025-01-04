using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;
using BattleCruisers.Targets.Helpers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPRangeCalculatorProvider : IPvPRangeCalculatorProvider
    {
        public PvPRangeCalculatorProvider()
        {
            BasicCalculator = new PvPBasicCalculator();
            SizeInclusiveCalculator = new PvPSizeInclusiveCalculator();
        }

        public IRangeCalculator BasicCalculator { get; }
        public IRangeCalculator SizeInclusiveCalculator { get; }
    }
}