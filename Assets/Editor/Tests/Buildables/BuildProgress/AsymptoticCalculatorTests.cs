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
        private float _proportionOfRemainingHealth;
        private float _maxBuildProgress;
        private float _deltaTime;

        [SetUp]
        public void SetuUp()
        {
            _calculator = new AsymptoticCalculator();

            _buildable = Substitute.For<IBuildable>();
			_buildable.BuildableState.Returns(BuildableState.InProgress);
            _buildable.BuildTimeInS.Returns(100);
            _buildable.CostInDroneS.Returns(2);

            _proportionOfRemainingHealth = 0.05f;
            _maxBuildProgress = 0.95f;
            _deltaTime = 0.1f;
        }

        [Test]
        public void CalculateBuildProgressInDroneS_BuildableNotUnderConstruction_Throws()
        {
            _buildable.BuildableState.Returns(BuildableState.Completed);
            Assert.Throws<UnityAsserts.AssertionException>(() => _calculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime));
        }

        [Test]
        public void CalculateBuildProgressInDroneS_BuildProgress_0()
        {
            _buildable.BuildProgress.Returns(0);

            float expectedBuildProgress = _buildable.CostInDroneS * _proportionOfRemainingHealth * _deltaTime;

            Assert.IsTrue(Mathf.Approximately(expectedBuildProgress, _calculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime)));
        }

        [Test]
        public void CalculateBuildProgressInDroneS_BuildProgress_50()
        {
            _buildable.BuildProgress.Returns(0.5f);

            float expectedBuildProgress = _buildable.CostInDroneS * (1 - _buildable.BuildProgress) * _proportionOfRemainingHealth * _deltaTime;

            Assert.IsTrue(Mathf.Approximately(expectedBuildProgress, _calculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime)));
        }

        [Test]
        public void CalculateBuildProgressInDroneS_BuildProgress_90()
        {
            _buildable.BuildProgress.Returns(0.9f);

            float expectedBuildProgress = _buildable.CostInDroneS * (1 - _buildable.BuildProgress) * _proportionOfRemainingHealth * _deltaTime;

            Assert.IsTrue(Mathf.Approximately(expectedBuildProgress, _calculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime)));
        }

        [Test]
        public void CalculateBuildProgressInDroneS_BuildProgress_MaxBuildProgress()
        {
            _buildable.BuildProgress.Returns(_maxBuildProgress);
            Assert.AreEqual(0, _calculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime));
        }
    }
}
