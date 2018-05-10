using BattleCruisers.Buildables;
using BattleCruisers.Buildables.BuildProgress;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.BuildProgress
{
    public class CompositeCalculatorTests
    {
        private CompositeCalculator _compositeCalculator;
        private IBuildProgressCalculator _slowCalculator, _normalCalculator, _fastCalculator;
        private IBuildable _buildable;
        private float _deltaTime;

        [SetUp]
        public void SetuUp()
        {
            _slowCalculator = Substitute.For<IBuildProgressCalculator>();
            _normalCalculator = Substitute.For<IBuildProgressCalculator>();
            _fastCalculator = Substitute.For<IBuildProgressCalculator>();

            _compositeCalculator = new CompositeCalculator(_slowCalculator, _normalCalculator, _fastCalculator);

            _buildable = Substitute.For<IBuildable>();
            _deltaTime = 17.29f;
        }

        [Test]
        public void CalculateBuildProgressInDroneS_DefaultIsNormal()
        {
            _compositeCalculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime);
            _normalCalculator.Received().CalculateBuildProgressInDroneS(_buildable, _deltaTime);
        }
		
		[Test]
		public void CalculateBuildProgressInDroneS_Slow()
		{
            _compositeCalculator.BuildSpeed = BuildSpeed.InfinitelySlow;
			_compositeCalculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime);
			_slowCalculator.Received().CalculateBuildProgressInDroneS(_buildable, _deltaTime);
		}

        [Test]
        public void CalculateBuildProgressInDroneS_Normal()
        {
            _compositeCalculator.BuildSpeed = BuildSpeed.Normal;
            _compositeCalculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime);
            _normalCalculator.Received().CalculateBuildProgressInDroneS(_buildable, _deltaTime);
        }

        [Test]
        public void CalculateBuildProgressInDroneS_Fast()
        {
            _compositeCalculator.BuildSpeed = BuildSpeed.VeryFast;
            _compositeCalculator.CalculateBuildProgressInDroneS(_buildable, _deltaTime);
            _fastCalculator.Received().CalculateBuildProgressInDroneS(_buildable, _deltaTime);
        }
    }
}
