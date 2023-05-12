using BattleCruisers.Data.Settings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public interface IPvPBuildSpeedCalculator
    {
        float FindAIBuildSpeed(Difficulty difficulty);
        float FindIncrementalAICruiserBuildSpeed(Difficulty difficulty, int levelNum);
    }
}