using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPRangeCalculatorProvider
    {
        IPvPRangeCalculator BasicCalculator { get; }
        IPvPRangeCalculator SizeInclusiveCalculator { get; }
    }
}