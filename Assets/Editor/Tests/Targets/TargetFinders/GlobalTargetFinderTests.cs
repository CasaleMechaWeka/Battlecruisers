using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Tests.Utils.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetFinders
{
    public class GlobalTargetFinderTests 
	{
		private GlobalTargetFinder _targetFinder;
		private ICruiser _enemyCruiser;
		private IBuilding _target;
		private ITarget _expectedTargetFound, _expectedTargetLost;
		private int _targetFoundEmittedCount, _targetLostEmittedCount;

		[SetUp]
		public void TestSetup()
		{
			_enemyCruiser = Substitute.For<ICruiser>();
			_target = Substitute.For<IBuilding>();

			_targetFinder = new GlobalTargetFinder(_enemyCruiser);

            _targetFoundEmittedCount = 0;
			_targetFinder.TargetFound += OnTargetFound;

            _targetLostEmittedCount = 0;
            _targetFinder.TargetLost += OnTargetLost;
		}

		[Test]
		public void EmitCruiserAsGlobalTarget()
		{
			_expectedTargetFound = _enemyCruiser;
            _targetFinder.EmitCruiserAsGlobalTarget();
			Assert.AreEqual(1, _targetFoundEmittedCount);
		}

		[Test]
		public void BuildableReachesHalfway_EmitsTargetFound()
		{
			EmitCruiserAsGlobalTarget();

			_expectedTargetFound = _target;

            _enemyCruiser.StartConstructingBuilding(_target);
			_target.BuildProgress.Returns(0.5f);
			_target.BuildableProgress += Raise.EventWith(_target, new BuildProgressEventArgs(_target));

			Assert.AreEqual(2, _targetFoundEmittedCount);
		}

		[Test]
		public void BuildableThatWasFound_Destroyed_EmitsTargetLost()
		{
			BuildableReachesHalfway_EmitsTargetFound();

            _expectedTargetLost = _target;

			_target.Destroyed += Raise.EventWith(_target, new DestroyedEventArgs(_target));

            Assert.AreEqual(1, _targetLostEmittedCount);
		}

		[Test]
		public void BuildableThatWasNotFound_Destroyed_DoesNotEmitsTargetLost()
		{
            EmitCruiserAsGlobalTarget();

            _enemyCruiser.StartConstructingBuilding(_target);
			_target.BuildProgress.Returns(0.1f);

			_target.Destroyed += Raise.EventWith(_target, new DestroyedEventArgs(_target));

            Assert.AreEqual(0, _targetLostEmittedCount);
		}

        [Test]
        public void CruiserDestroyed_EmitsTargetLost()
        {
            EmitCruiserAsGlobalTarget();

            _expectedTargetLost = _enemyCruiser;

            _enemyCruiser.Destroyed += Raise.EventWith(new DestroyedEventArgs(_enemyCruiser));

            Assert.AreEqual(1, _targetLostEmittedCount);
        }

        private void OnTargetLost(object sender, TargetEventArgs e)
        {
            _targetLostEmittedCount++;
            Assert.AreSame(_targetFinder, sender);
            Assert.AreSame(_expectedTargetLost, e.Target);
        }

        private void OnTargetFound(object sender, TargetEventArgs e)
		{
			_targetFoundEmittedCount++;
			Assert.AreSame(_targetFinder, sender);
			Assert.AreSame(_expectedTargetFound, e.Target);
		}
	}
}
