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
		private RangedTargetFinder _targetFinder;
		private IFactionObjectDetector _enemyDetector;
		private IFactionable _target;

		[SetUp]
		public void TestSetup()
		{
			_enemyDetector = Substitute.For<IFactionObjectDetector>();
			_target = Substitute.For<IFactionable>();

			GameObject gameObject = new GameObject();
			_targetFinder = gameObject.AddComponent<RangedTargetFinder>();
			_targetFinder.Initialise(_enemyDetector);
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
		
		[ExpectedException(typeof(UnityAsserts.AssertionException))]
		[Test]
		public void EnemyExited_WithoutPreceedingEnemyEntered_Throws()
		{
			_enemyDetector.OnExited += Raise.EventWith(_enemyDetector, new FactionObjectEventArgs(_target));
		}

		[Test]
		public void TargetFoundEvent_TargetLostEvent()
		{
			TargetFoundEvent();

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

		#region FindTarget()
		[Test]
		public void FindTarget_NoTargets()
		{
			Assert.IsNull(_targetFinder.FindTarget());
		}

		[Test]
		public void FindTarget_After_TargetFound()
		{
			TargetFoundEvent();
			Assert.AreEqual(_target, _targetFinder.FindTarget());
		}

		[Test]
		public void FindTarget_After_TargetFound_TargetLost()
		{
			TargetFoundEvent_TargetLostEvent();
			Assert.IsNull(_targetFinder.FindTarget());
		}

		[Test]
		public void FindTarget_After_TargetFound_TargetDestroyed()
		{
			TargetFoundEvent();
			_target.Destroyed += Raise.Event();
			Assert.IsNull(_targetFinder.FindTarget());
		}
		#endregion FindTarget()
	}
}
