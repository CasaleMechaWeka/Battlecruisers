using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    public class BuildProgressCalculatorFactory : IBuildProgressCalculatorFactory
    {
        private readonly ISettingsManager _settingsManager;

        // For cheating :)
        public static IBuildSpeedController playerBuildSpeed, aiBuildSpeed;

        public BuildProgressCalculatorFactory(ISettingsManager settingsManager)
        {
            Assert.IsNotNull(settingsManager);
            _settingsManager = settingsManager;
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

        public IBuildProgressCalculator CreateAICruiserCalculator()
        {
#if ENABLE_CHEATS
            CompositeCalculator calculator = CreateCompositeCalculator(FindBuildSpeedMultiplier(_settingsManager));
            aiBuildSpeed = calculator;
            return calculator;
#endif
            return new LinearCalculator(FindBuildSpeedMultiplier(_settingsManager));
        }

        private float FindBuildSpeedMultiplier(ISettingsManager settingsManager)
        {
            // FELIX  Diffirent for skirmish :P
            switch (settingsManager.AIDifficulty)
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
                    throw new ArgumentException($"Unkown difficulty: {settingsManager.AIDifficulty}");
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