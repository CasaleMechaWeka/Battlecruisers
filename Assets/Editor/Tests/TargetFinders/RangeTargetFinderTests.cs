using BattleCruisers.Buildables.Units.Detectors;
using BattleCruisers.TargetFinders;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using BattleCruisers.Buildables;

namespace BattleCruisers.Tests.TargetFinders
{
	public class RangeTargetFinderTests 
	{
		private RangedTargetFinder _targetFinder;
		private IFactionObjectDetector _enemyDetector;

		[SetUp]
		public void TestSetup()
		{
			GameObject gameObject = new GameObject();
			_targetFinder = gameObject.AddComponent<RangedTargetFinder>();

			_enemyDetector = Substitute.For<IFactionObjectDetector>();
			_targetFinder.Initialise(_enemyDetector);
		}

		[Test]
		public void Initialisation()
		{
			Assert.IsNull(_targetFinder.FindTarget());
		}

//		[Test]
//		public void TargetFoundEvent()
//		{
//			bool wasCalled = false;
//			IFactionable target = Substitute.For<IFactionable>();
//
//			_targetFinder.TargetFound += (object sender, TargetEventArgs e) => 
//			{
//				wasCalled = true;
//				Assert.AreEqual(_targetFinder, sender);
//				Assert.AreEqual(target, e.Target);
//			};
//
//			_enemyDetector.OnEntered.Invoke(target);
//
//			Assert.IsTrue(wasCalled);
//		}
	}
}
