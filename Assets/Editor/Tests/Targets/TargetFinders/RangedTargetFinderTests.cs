using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetFinders
{
    public class RangedTargetFinderTests
	{
		private ITargetFinder _targetFinder;
		private ITargetDetector _enemyDetector;
		private ITarget _target;
		private ITargetFilter _targetFilter;

		[SetUp]
		public void TestSetup()
		{
			_enemyDetector = Substitute.For<ITargetDetector>();
			_target = Substitute.For<ITarget>();

			_targetFilter = Substitute.For<ITargetFilter>();

			_targetFinder = new RangedTargetFinder(_enemyDetector, _targetFilter);
		}

		[Test]
		public void EnemyEntered_IsMatch_AndNotDestroyed_EmitsTargetFound()
		{
			bool wasCalled = false;

			_targetFinder.TargetFound += (object sender, TargetEventArgs e) => 
			{
				wasCalled = true;
				Assert.AreEqual(_targetFinder, sender);
				Assert.AreEqual(_target, e.Target);
			};

			_targetFilter.IsMatch(_target).Returns(true);
			_enemyDetector.OnEntered += Raise.EventWith(_enemyDetector, new TargetEventArgs(_target));

			Assert.IsTrue(wasCalled);
		}

        [Test]
        public void EnemyEntered_IsMatch_IsDestroyed_EmitsNothing()
        {
            _targetFinder.TargetFound += (object sender, TargetEventArgs e) =>
            {
                Assert.Fail();
            };

            _target.IsDestroyed.Returns(true);
            _targetFilter.IsMatch(_target).Returns(true);
            _enemyDetector.OnEntered += Raise.EventWith(_enemyDetector, new TargetEventArgs(_target));
        }

        [Test]
		public void EnemyEntered_IsNotMatch_EmitsNothing()
		{
			_targetFinder.TargetFound += (object sender, TargetEventArgs e) => 
			{
				Assert.Fail();
			};

			_targetFilter.IsMatch(_target).Returns(false);
			_enemyDetector.OnEntered += Raise.EventWith(_enemyDetector, new TargetEventArgs(_target));
		}
		
		[Test]
		public void EnemyExited_IsMatch_EmitsTargetLost()
		{
			bool wasCalled = false;

			_targetFinder.TargetLost += (object sender, TargetEventArgs e) => 
			{
				wasCalled = true;
				Assert.AreEqual(_targetFinder, sender);
				Assert.AreEqual(_target, e.Target);
			};

			_targetFilter.IsMatch(_target).Returns(true);
			_enemyDetector.OnExited += Raise.EventWith(_enemyDetector, new TargetEventArgs(_target));

			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void EnemyExited_IsNotMatch_EmitsNothing()
		{
			_targetFinder.TargetLost += (object sender, TargetEventArgs e) => 
			{
				Assert.Fail();
			};

			_targetFilter.IsMatch(_target).Returns(false);
			_enemyDetector.OnExited += Raise.EventWith(_enemyDetector, new TargetEventArgs(_target));
		}
	}
}
