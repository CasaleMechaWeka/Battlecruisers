using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPRangeCalculatorProvider : IPvPRangeCalculatorProvider
    {
        public PvPRangeCalculatorProvider()
        {
            BasicCalculator = new PvPBasicCalculator();
            SizeInclusiveCalculator = new PvPSizeInclusiveCalculator();
        }

        public IPvPRangeCalculator BasicCalculator { get; }
        public IPvPRangeCalculator SizeInclusiveCalculator { get; }
    }
}