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
#endif
            return new LinearCalculator(BuildSpeedMultipliers.DEFAULT);
        }

        public IBuildProgressCalculator CreateAICruiserCalculator(Difficulty difficulty)
        {
#if ENABLE_CHEATS
            CompositeCalculator calculator = CreateCompositeCalculator(_buildSpeedCalculator.FindAIBuildSpeed(difficulty));
            aiBuildSpeed = calculator;
            return calculator;
#endif
            return new LinearCalculator(_buildSpeedCalculator.FindAIBuildSpeed(difficulty));
        }

        public IBuildProgressCalculator CreateIncrementalAICruiserCalculator(Difficulty difficulty, int levelNum)
        {
#if ENABLE_CHEATS
            CompositeCalculator calculator = CreateCompositeCalculator(_buildSpeedCalculator.FindIncrementalAICruiserBuildSpeed(difficulty, levelNum));
            aiBuildSpeed = calculator;
            return calculator;
#endif
            return new LinearCalculator(_buildSpeedCalculator.FindIncrementalAICruiserBuildSpeed(difficulty, levelNum));
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