using BattleCruisers.Targets.Helpers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPRangeCalculatorProvider
    {
        IRangeCalculator BasicCalculator { get; }
        IRangeCalculator SizeInclusiveCalculator { get; }
    }
}