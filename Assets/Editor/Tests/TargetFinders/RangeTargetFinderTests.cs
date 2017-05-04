using BattleCruisers.Targets.TargetFinders;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using BattleCruisers.Buildables;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.TargetFinders
{
	public class RangeTargetFinderTests 
	{
		private ITargetFinder _targetFinder;
		private IFactionObjectDetector _enemyDetector;
		private IFactionable _target;

		[SetUp]
		public void TestSetup()
		{
			_enemyDetector = Substitute.For<IFactionObjectDetector>();
			_target = Substitute.For<IFactionable>();

			_targetFinder = new RangedTargetFinder(_enemyDetector);
			_targetFinder.StartFindingTargets();
		}

		[Test]
		public void TargetFoundEvent()
		{
			bool wasCalled = false;

			_targetFinder.TargetFound += (object sender, TargetEventArgs e) => 
			{
				wasCalled = true;
				Assert.AreEqual(_targetFinder, sender);
				Assert.AreEqual(_target, e.Target);
			};

			_enemyDetector.OnEntered += Raise.EventWith(_enemyDetector, new FactionObjectEventArgs(_target));

			Assert.IsTrue(wasCalled);
		}
		
		[Test]
		public void TargetLostEvent()
		{
			bool wasCalled = false;

			_targetFinder.TargetLost += (object sender, TargetEventArgs e) => 
			{
				wasCalled = true;
				Assert.AreEqual(_targetFinder, sender);
				Assert.AreEqual(_target, e.Target);
			};

			_enemyDetector.OnExited += Raise.EventWith(_enemyDetector, new FactionObjectEventArgs(_target));

			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void TargetDestroyedEvent()
		{
			TargetFoundEvent();

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
