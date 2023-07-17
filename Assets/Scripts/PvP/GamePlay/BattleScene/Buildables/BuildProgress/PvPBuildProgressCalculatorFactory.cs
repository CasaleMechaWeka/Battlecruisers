using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public class PvPBuildProgressCalculatorFactory : IPvPBuildProgressCalculatorFactory
    {
        private readonly IPvPBuildSpeedCalculator _buildSpeedCalculator;

        // For cheating :)
        public static IPvPBuildSpeedController playerABuildSpeed, playerBBuildSpeed, aiBuildSpeed;

        public const float BOOST_PER_LEVEL = 0.01f;

        public PvPBuildProgressCalculatorFactory(IPvPBuildSpeedCalculator buildSpeedCalculator)
        {
            Assert.IsNotNull(buildSpeedCalculator);
            _buildSpeedCalculator = buildSpeedCalculator;
        }

        public IPvPBuildProgressCalculator CreatePlayerACruiserCalculator()
        {
#if ENABLE_CHEATS
            PvPCompositeCalculator calculator = CreateCompositeCalculator(PvPBuildSpeedMultipliers.DEFAULT);
            playerABuildSpeed = calculator;
            return calculator;
#endif
            return new PvPLinearCalculator(PvPBuildSpeedMultipliers.DEFAULT);
        }

        public IPvPBuildProgressCalculator CreatePlayerBCruiserCalculator()
        {
#if ENABLE_CHEATS
            PvPCompositeCalculator calculator = CreateCompositeCalculator(/*_buildSpeedCalculator.FindAIBuildSpeed(difficulty)*/PvPBuildSpeedMultipliers.DEFAULT);
            playerBBuildSpeed = calculator;
            return calculator;
#endif
            return new PvPLinearCalculator(/*_buildSpeedCalculator.FindAIBuildSpeed(difficulty)*/PvPBuildSpeedMultipliers.DEFAULT);
        }

        public IPvPBuildProgressCalculator CreateIncrementalAICruiserCalculator(Difficulty difficulty, int levelNum)
        {
#if ENABLE_CHEATS
            PvPCompositeCalculator calculator = CreateCompositeCalculator(_buildSpeedCalculator.FindIncrementalAICruiserBuildSpeed(difficulty, levelNum));
            aiBuildSpeed = calculator;
            return calculator;
#endif
            return new PvPLinearCalculator(_buildSpeedCalculator.FindIncrementalAICruiserBuildSpeed(difficulty, levelNum));
        }

        private PvPCompositeCalculator CreateCompositeCalculator(float defaultBuildSpeedMultiplier)
        {
            PvPCompositeCalculator calculator
                = new PvPCompositeCalculator(
                    new PvPLinearCalculator(defaultBuildSpeedMultiplier),
                    new PvPLinearCalculator(PvPBuildSpeedMultipliers.FAST),
                    new PvPLinearCalculator(PvPBuildSpeedMultipliers.VERY_FAST))
                {
                    BuildSpeed = PvPBuildSpeed.InfinitelySlow
                };
            return calculator;
        }
    }
}