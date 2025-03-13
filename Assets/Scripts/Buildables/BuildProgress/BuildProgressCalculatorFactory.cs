using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    public class BuildProgressCalculatorFactory : IBuildProgressCalculatorFactory
    {
        private readonly IBuildSpeedCalculator _buildSpeedCalculator;

        // For cheating :)
        public static IBuildSpeedController playerBuildSpeed, aiBuildSpeed;

        public const float BOOST_PER_LEVEL = 0.01f;

        public BuildProgressCalculatorFactory(IBuildSpeedCalculator buildSpeedCalculator)
        {
            Assert.IsNotNull(buildSpeedCalculator);
            _buildSpeedCalculator = buildSpeedCalculator;
        }

        public IBuildProgressCalculator CreatePlayerCruiserCalculator()
        {
#if ENABLE_CHEATS
            CompositeCalculator calculator = CreateCompositeCalculator(BuildSpeedMultipliers.DEFAULT);
            playerBuildSpeed = calculator;
            return calculator;
#else
            return new LinearCalculator(BuildSpeedMultipliers.DEFAULT);
#endif
        }

        public IBuildProgressCalculator CreateAICruiserCalculator(Difficulty difficulty)
        {
#if ENABLE_CHEATS
            CompositeCalculator calculator = CreateCompositeCalculator(_buildSpeedCalculator.FindAIBuildSpeed(difficulty));
            aiBuildSpeed = calculator;
            return calculator;
#else
            return new LinearCalculator(_buildSpeedCalculator.FindAIBuildSpeed(difficulty));
#endif
        }

        public IBuildProgressCalculator CreateIncrementalAICruiserCalculator(Difficulty difficulty, int levelNum, bool isSideQuest)
        {
#if ENABLE_CHEATS
            CompositeCalculator calculator = CreateCompositeCalculator(_buildSpeedCalculator.FindIncrementalAICruiserBuildSpeed(difficulty, levelNum, isSideQuest));
            aiBuildSpeed = calculator;
            return calculator;
#else
            return new LinearCalculator(_buildSpeedCalculator.FindIncrementalAICruiserBuildSpeed(difficulty, levelNum, isSideQuest));
#endif
        }

        private CompositeCalculator CreateCompositeCalculator(float defaultBuildSpeedMultiplier)
        {
            CompositeCalculator calculator
                = new CompositeCalculator(
                    new LinearCalculator(defaultBuildSpeedMultiplier),
                    new LinearCalculator(BuildSpeedMultipliers.FAST),
                    new LinearCalculator(BuildSpeedMultipliers.VERY_FAST))
                {
                    BuildSpeed = BuildSpeed.InfinitelySlow
                };
            return calculator;
        }
    }
}