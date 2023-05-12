using BattleCruisers.Data.Settings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public interface IPvPBuildProgressCalculatorFactory
    {
        IPvPBuildProgressCalculator CreatePlayerCruiserCalculator();
        IPvPBuildProgressCalculator CreateAICruiserCalculator(Difficulty difficulty);
        IPvPBuildProgressCalculator CreateIncrementalAICruiserCalculator(Difficulty difficulty, int levelNum);
    }
}