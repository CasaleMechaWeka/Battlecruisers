using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Buildables.BuildProgress
{
    public class BuildProgressCalculatorFactory : IBuildProgressCalculatorFactory
    {
        // For cheating :)
        public static IBuildSpeedController playerBuildSpeed, aiBuildSpeed;

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
            CompositeCalculator calculator = CreateCompositeCalculator(FindBuildSpeedMultiplier(difficulty));
            aiBuildSpeed = calculator;
            return calculator;
#endif
            return new LinearCalculator(FindBuildSpeedMultiplier(difficulty));
        }

        private float FindBuildSpeedMultiplier(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return BuildSpeedMultipliers.HALF_DEFAULT;

                case Difficulty.Normal:
                    return BuildSpeedMultipliers.POINT_7_DEFAULT;

                case Difficulty.Hard:
                    return BuildSpeedMultipliers.DEFAULT;

                case Difficulty.Harder:
                    return BuildSpeedMultipliers.ONE_AND_A_QUARTER_DEFAULT;

                default:
                    throw new ArgumentException($"Unkown difficulty: {difficulty}");
            }
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