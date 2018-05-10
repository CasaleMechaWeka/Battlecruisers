using BattleCruisers.Buildables;
using BattleCruisers.Buildables.BuildProgress;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.BuildProgress
{
    public class AsymptoticCalculatorTests
    {
        private IBuildProgressCalculator _calculator;
        private IBuildable _buildable;
        private float _totalBuildTimeInDroneS;
        private float _proportionOfRemainingHealth;
        private float _deltaTime;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _calculator = new AsymptoticCalculator();

            _buildable = Substitute.For<IBuildable>();
			_buildable.BuildableState.Returns(BuildableState.InProgress);
            _buildable.BuildTimeInS.Returns(100);
            _buildable.CostInDroneS.Returns(2);
            _totalBuildTimeInDroneS = _buildable.BuildTimeInS * _buildable.CostInDroneS;

            _proportionOfRemainingHealth = 0.2f;
            _deltaTime = 0.1f;
        }

        [Test]
        public void CalculateBuildProgressInDroneS_BuildableNotUnderConstruction_Throws()
        {
            _buildable.BuildableState.Returns(BuildableState.Completed);
            Assert.Throws<UnityAsserts.AssertionException>(() => _calculator.CalculateBuildProgressInDroneS(_buildable, deltaTime: 0.1f));
        }

        [Test]
        public void CalculateBuildProgressInDroneS_BuildProgress_0()
        {
            _buildable.BuildProgress.Returns(0);

            float expectedBuildProgress = _totalBuildTimeInDroneS * _proportionOfRemainingHealth * _deltaTime;

            Assert.IsTrue(Mathf.Approximately(expectedBuildProgress, _calculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime)));
        }

        [Test]
        public void CalculateBuildProgressInDroneS_BuildProgress_50()
        {
            _buildable.BuildProgress.Returns(0.5f);

            float expectedBuildProgress = _totalBuildTimeInDroneS * (1 - _buildable.BuildProgress) * _proportionOfRemainingHealth * _deltaTime;

            Assert.IsTrue(Mathf.Approximately(expectedBuildProgress, _calculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime)));
        }

        [Test]
        public void CalculateBuildProgressInDroneS_BuildProgress_90()
        {
            _buildable.BuildProgress.Returns(0.9f);

            float expectedBuildProgress = _totalBuildTimeInDroneS * (1 - _buildable.BuildProgress) * _proportionOfRemainingHealth * _deltaTime;

            Assert.IsTrue(Mathf.Approximately(expectedBuildProgress, _calculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime)));
        }
    }
}
