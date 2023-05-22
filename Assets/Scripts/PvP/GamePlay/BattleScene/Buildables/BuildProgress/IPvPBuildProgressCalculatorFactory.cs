using BattleCruisers.Data.Settings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public interface IPvPBuildProgressCalculatorFactory
    {
        IPvPBuildProgressCalculator CreatePlayerACruiserCalculator();
        IPvPBuildProgressCalculator CreatePlayerBCruiserCalculator();
        IPvPBuildProgressCalculator CreateIncrementalAICruiserCalculator(Difficulty difficulty, int levelNum);
    }
}