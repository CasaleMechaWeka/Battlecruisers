using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public class PvPBuildSpeedCalculator
    {
        public const float BOOST_PER_LEVEL = 0.01f;

        public float FindAIBuildSpeed(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Normal => BuildSpeedMultipliers.POINT_7_DEFAULT,
                Difficulty.Hard => BuildSpeedMultipliers.DEFAULT,
                Difficulty.Harder => BuildSpeedMultipliers.ONE_AND_A_QUARTER_DEFAULT,
                _ => throw new ArgumentException($"Unkown difficulty: {difficulty}"),
            };

        }

        public float FindIncrementalAICruiserBuildSpeed(Difficulty difficulty, int levelNum)
        {
            float baseBuildSpeed = FindBaseBuildSpeedMultiplier(difficulty);
            float levelBoost = FindLevelBoost(difficulty, levelNum);
            return baseBuildSpeed + levelBoost;
        }

        private float FindBaseBuildSpeedMultiplier(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return BuildSpeedMultipliers.POINT_65_DEFAULT;

                case Difficulty.Hard:
                    return BuildSpeedMultipliers.POINT_93_DEFAULT;

                case Difficulty.Harder:
                    return BuildSpeedMultipliers.ONE_AND_A_QUARTER_DEFAULT;

                default:
                    throw new ArgumentException($"Unkown difficulty: {difficulty}");
            }
        }

        private float FindLevelBoost(Difficulty difficulty, int levelNum)
        {
            Assert.IsTrue(levelNum > 0);
            Assert.IsTrue(levelNum <= StaticData.NUM_OF_LEVELS);

            switch (difficulty)
            {
                case Difficulty.Normal:
                case Difficulty.Hard:
                    return (levelNum - 1) * BOOST_PER_LEVEL;

                case Difficulty.Harder:
                    return 0;

                default:
                    throw new ArgumentException($"Unkown difficulty: {difficulty}");
            }
        }
    }
}