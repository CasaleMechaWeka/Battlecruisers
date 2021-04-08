using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    // FELIX  Test :)
    public class BuildProgressCalculatorFactory : IBuildProgressCalculatorFactory
    {
        // For cheating :)
        public static IBuildSpeedController playerBuildSpeed, aiBuildSpeed;

        public const float BOOST_PER_LEVEL = 0.01f;

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

        public IBuildProgressCalculator CreateIncrementalAICruiserCalculator(Difficulty difficulty, int levelNum)
        {
#if ENABLE_CHEATS
            CompositeCalculator calculator = CreateCompositeCalculator(FindBuildSpeedMultiplier(difficulty));
            aiBuildSpeed = calculator;
            return calculator;
#endif
            float baseBuildSpeed = FindBaseBuildSpeedMultiplier(difficulty);
            float levelBoost = FindLevelBoost(difficulty, levelNum);
            float buildSpeedMultiplier = baseBuildSpeed + levelBoost;
            return new LinearCalculator(buildSpeedMultiplier);
        }

        private float FindBaseBuildSpeedMultiplier(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return BuildSpeedMultipliers.POINT_65_DEFAULT;

                case Difficulty.Hard:
                    return BuildSpeedMultipliers.POINT_925_DEFAULT;

                case Difficulty.Harder:
                    return BuildSpeedMultipliers.ONE_AND_A_QUARTER_DEFAULT;

                default:
                    throw new ArgumentException($"Unkown difficulty: {difficulty}");
            }
        }

        private float FindLevelBoost(Difficulty difficulty, int levelNum)
        {
            Assert.IsTrue(levelNum > 0);

            switch (difficulty)
            {
                case Difficulty.Normal:
                case Difficulty.Hard:
                    return levelNum * BOOST_PER_LEVEL;

                case Difficulty.Harder:
                    return 0;

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