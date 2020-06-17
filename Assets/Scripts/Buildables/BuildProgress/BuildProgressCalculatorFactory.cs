using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    public class BuildProgressCalculatorFactory : IBuildProgressCalculatorFactory
    {
        private readonly ISettingsManager _settingsManager;

        public BuildProgressCalculatorFactory(ISettingsManager settingsManager)
        {
            Assert.IsNotNull(settingsManager);
            _settingsManager = settingsManager;
        }

        public IBuildProgressCalculator CreatePlayerCruiserCalculator()
        {
            return new LinearCalculator(BuildSpeedMultipliers.DEFAULT);
        }

        public IBuildProgressCalculator CreateAICruiserCalculator()
        {
            return new LinearCalculator(FindBuildSpeedMultiplier(_settingsManager));
        }

        private float FindBuildSpeedMultiplier(ISettingsManager settingsManager)
        {
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
    }
}