using BattleCruisers.Targets.TargetFinders;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using BattleCruisers.Buildables;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets
{
	public class RangedTargetFinderTests 
	{
		private ITargetFinder _targetFinder;
		private ITargetDetector _enemyDetector;
		private ITarget _target;

		[SetUp]
		public void TestSetup()
		{
			_enemyDetector = Substitute.For<ITargetDetector>();
			_target = Substitute.For<ITarget>();

			_targetFinder = new RangedTargetFinder(_enemyDetector);
			_targetFinder.StartFindingTargets();
		}

		[Test]
		public void EnemyEntered_EmitsTargetFound()
		{
			bool wasCalled = false;

			_targetFinder.TargetFound += (object sender, TargetEventArgs e) => 
			{
				wasCalled = true;
				Assert.AreEqual(_targetFinder, sender);
				Assert.AreEqual(_target, e.Target);
			};

			_enemyDetector.OnEntered += Raise.EventWith(_enemyDetector, new TargetEventArgs(_target));

			Assert.IsTrue(wasCalled);
		}
		
		[Test]
		public void EnemyExited_EmitsTargetLost()
		{
			bool wasCalled = false;

			_targetFinder.TargetLost += (object sender, TargetEventArgs e) => 
			{
				wasCalled = true;
				Assert.AreEqual(_targetFinder, sender);
				Assert.AreEqual(_target, e.Target);
			};

			_enemyDetector.OnExited += Raise.EventWith(_enemyDetector, new TargetEventArgs(_target));

			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void TargetDestroyed_EmitsTargetLost()
		{
			EnemyEntered_EmitsTargetFound();

			bool wasCalled = false;

			_targetFinder.TargetLost += (object sender, TargetEventArgs e) => 
			{
				wasCalled = true;
				Assert.AreEqual(_targetFinder, sender);
				Assert.AreEqual(_target, e.Target);
			};

			_target.Destroyed += Raise.EventWith(_target, new DestroyedEventArgs(_target));

			Assert.IsTrue(wasCalled);
		}
	}
}
