using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Units;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.BuildProgress
{
    public class UnitBuildProgressTests
    {
        private IUnitBuildProgress _unitBuildProgress;
        private string _unitName = "Turtle";
        private IBuildProgressFeedback _buildProgressFeedback;
        private IUnit _unit;
        private IFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _buildProgressFeedback = Substitute.For<IBuildProgressFeedback>();
            _unitBuildProgress = new UnitBuildProgress(_unitName, _buildProgressFeedback);

            _unit = Substitute.For<IUnit>();
            _factory = Substitute.For<IFactory>();
        }

        [Test]
        public void ShowBuildProgressIfNecessary_NullUnit()
        {
            _unitBuildProgress.ShowBuildProgressIfNecessary(unit: null, factory: _factory);
            _buildProgressFeedback.Received().HideBuildProgress();
        }

        [Test]
        public void ShowBuildProgressIfNecessary_NonNullUnit_DifferentName()
        {
            _unit.Name.Returns("Not a turtle!");
            _unitBuildProgress.ShowBuildProgressIfNecessary(_unit, _factory);

            _buildProgressFeedback.Received().HideBuildProgress();
        }

        [Test]
        public void ShowBuildProgressIfNecessary_NonNullUnit_SameName()
        {
            _unit.Name.Returns(_unitName);
            _unitBuildProgress.ShowBuildProgressIfNecessary(_unit, _factory);

            _buildProgressFeedback.Received().ShowBuildProgress(_unit, _factory);
        }
    }
}