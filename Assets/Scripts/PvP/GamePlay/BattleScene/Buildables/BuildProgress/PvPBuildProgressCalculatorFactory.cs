using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public class PvPBuildProgressCalculatorFactory
    {
        private readonly PvPBuildSpeedCalculator _buildSpeedCalculator;

        // For cheating :)
        public static IPvPBuildSpeedController playerABuildSpeed, playerBBuildSpeed, aiBuildSpeed;

        public const float BOOST_PER_LEVEL = 0.01f;

        public PvPBuildProgressCalculatorFactory(PvPBuildSpeedCalculator buildSpeedCalculator)
        {
            Assert.IsNotNull(buildSpeedCalculator);
            _buildSpeedCalculator = buildSpeedCalculator;
        }

        public IPvPBuildProgressCalculator CreatePlayerACruiserCalculator()
        {
#if ENABLE_CHEATS
            PvPCompositeCalculator calculator = CreateCompositeCalculator(BuildSpeedMultipliers.DEFAULT);
            playerABuildSpeed = calculator;
            return calculator;
#else
            return new PvPLinearCalculator(BuildSpeedMultipliers.DEFAULT);
#endif
        }

        public IPvPBuildProgressCalculator CreatePlayerBCruiserCalculator()
        {
#if ENABLE_CHEATS
            PvPCompositeCalculator calculator = CreateCompositeCalculator(/*_buildSpeedCalculator.FindAIBuildSpeed(difficulty)*/BuildSpeedMultipliers.DEFAULT);
            playerBBuildSpeed = calculator;
            return calculator;
#else
            return new PvPLinearCalculator(/*_buildSpeedCalculator.FindAIBuildSpeed(difficulty)*/BuildSpeedMultipliers.DEFAULT);
#endif
        }

        public IPvPBuildProgressCalculator CreateIncrementalAICruiserCalculator(Difficulty difficulty, int levelNum)
        {
#if ENABLE_CHEATS
            PvPCompositeCalculator calculator = CreateCompositeCalculator(_buildSpeedCalculator.FindIncrementalAICruiserBuildSpeed(difficulty, levelNum));
            aiBuildSpeed = calculator;
            return calculator;
#else
            return new PvPLinearCalculator(_buildSpeedCalculator.FindIncrementalAICruiserBuildSpeed(difficulty, levelNum));
#endif
        }

        private PvPCompositeCalculator CreateCompositeCalculator(float defaultBuildSpeedMultiplier)
        {
            PvPCompositeCalculator calculator
                = new PvPCompositeCalculator(
                    new PvPLinearCalculator(defaultBuildSpeedMultiplier),
                    new PvPLinearCalculator(BuildSpeedMultipliers.FAST),
                    new PvPLinearCalculator(BuildSpeedMultipliers.VERY_FAST))
                {
                    BuildSpeed = PvPBuildSpeed.InfinitelySlow
                };
            return calculator;
        }
    }
}