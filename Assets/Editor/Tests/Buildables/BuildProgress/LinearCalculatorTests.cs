using BattleCruisers.Buildables;
using BattleCruisers.Buildables.BuildProgress;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.BuildProgress
{
    public class LinearCalculatorTests
    {
        private IBuildProgressCalculator _calculator;
        private IBuildable _buildable;
        private float _buildSpeedMultiplier = 3;

        [SetUp]
        public void SetuUp()
        {
            _calculator = new LinearCalculator(_buildSpeedMultiplier);

            _buildable = Substitute.For<IBuildable>();
        }

        [Test]
        public void CalculateBuildProgressInDroneS_BuildableNotUnderConstruction_Throws()
        {
            _buildable.BuildableState.Returns(BuildableState.Completed);
            Assert.Throws<UnityAsserts.AssertionException>(() => _calculator.CalculateBuildProgressInDroneS(_buildable, deltaTime: 0.1f));
        }

        [Test]
        public void CalculateBuildProgressInDroneS()
        {
            _buildable.BuildableState.Returns(BuildableState.InProgress);
            _buildable.DroneConsumer.NumOfDrones.Returns(4);
            _buildable.BuildProgressBoostable.BoostMultiplier.Returns(1.5f);
            float deltaTime = 0.1f;

            float expectedBuildProgress =
                _buildable.DroneConsumer.NumOfDrones
                * _buildable.BuildProgressBoostable.BoostMultiplier
                * deltaTime
                * _buildSpeedMultiplier;

            Assert.AreEqual(expectedBuildProgress, _calculator.CalculateBuildProgressInDroneS(_buildable, deltaTime));
        }
    }
}
