using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.BuildProgress
{
    public class BuildSpeedCalculatorTests
    {
        private IBuildSpeedCalculator _calculator;

        [SetUp]
        public void TestSetup()
        {
            _calculator = new BuildSpeedCalculator();
        }

        [Test]
        public void FindAIBuildSpeed_Easy_Throws()
        {
            Assert.Throws<ArgumentException>(() => _calculator.FindAIBuildSpeed(Difficulty.Easy));
        }

        [Test, Sequential]
        public void FindAIBuildSpeed(
            [Values(Difficulty.Normal, Difficulty.Hard, Difficulty.Harder)] Difficulty difficulty,
            [Values(BuildSpeedMultipliers.POINT_7_DEFAULT, BuildSpeedMultipliers.DEFAULT, BuildSpeedMultipliers.ONE_AND_A_QUARTER_DEFAULT)] float expectedBuildSpeed)
        {
            Assert.AreEqual(expectedBuildSpeed, _calculator.FindAIBuildSpeed(difficulty));
        }

        [Test]
        public void FindIncrementalAICruiserBuildSpeed_Easy_Throws()
        {
            Assert.Throws<ArgumentException>(() => _calculator.FindIncrementalAICruiserBuildSpeed(Difficulty.Easy, levelNum: 2));
        }

        [Test]
        public void FindIncrementalAICruiserBuildSpeed_InvalidLevel_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _calculator.FindIncrementalAICruiserBuildSpeed(Difficulty.Normal, levelNum: 0));
            Assert.Throws<UnityAsserts.AssertionException>(() => _calculator.FindIncrementalAICruiserBuildSpeed(Difficulty.Normal, levelNum: StaticData.NUM_OF_LEVELS + 1));
        }

        [Test, Sequential]
        public void FindIncrementalAICruiserBuildSpeed_Normal(
            [Values(1, 10, 15, 25)] int levelNum,
            [Values(1.3f, 1.39f, 1.44f, 1.54f)] float expectedBuildSpeed)
        {
            Assert.IsTrue(Mathf.Approximately(expectedBuildSpeed, _calculator.FindIncrementalAICruiserBuildSpeed(Difficulty.Normal, levelNum)));
        }

        [Test, Sequential]
        public void FindIncrementalAICruiserBuildSpeed_Hard(
            [Values(1, 10, 15, 25)] int levelNum,
            [Values(1.86f, 1.95f, 2f, 2.1f)] float expectedBuildSpeed)
        {
            Assert.IsTrue(Mathf.Approximately(expectedBuildSpeed, _calculator.FindIncrementalAICruiserBuildSpeed(Difficulty.Hard, levelNum)));
        }

        [Test, Sequential]
        public void FindIncrementalAICruiserBuildSpeed_Harder([Values(1, 10, 15, 25)] int levelNum)
        {
            Assert.IsTrue(Mathf.Approximately(BuildSpeedMultipliers.ONE_AND_A_QUARTER_DEFAULT, _calculator.FindIncrementalAICruiserBuildSpeed(Difficulty.Harder, levelNum)));
        }
    }
}