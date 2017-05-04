using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetFinders;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets
{
	public class GlobalTargetFinderTests 
	{
		private ITargetFinder _targetFinder;
		private ICruiser _enemyCruiser;
		private IBuildable _target;
		private ITarget _expectedTargetFound;
		private int _targetFoundEmittedCount;

		[SetUp]
		public void TestSetup()
		{
			_enemyCruiser = Substitute.For<ICruiser>();
			_target = Substitute.For<IBuildable>();

			_targetFinder = new GlobalTargetFinder(_enemyCruiser);
			_targetFinder.TargetFound += OnTargetFound;

			_targetFoundEmittedCount = 0;
		}

		[Test]
		public void Cruiser_EmitsTargetFound()
		{
			_expectedTargetFound = _enemyCruiser;
			_targetFinder.StartFindingTargets();
			Assert.AreEqual(1, _targetFoundEmittedCount);
		}

		[Test]
		public void BuildableReachesHalfway_EmitsTargetFound()
		{
			Cruiser_EmitsTargetFound();

			_expectedTargetFound = _target;

			_enemyCruiser.StartedConstruction += Raise.EventWith(_enemyCruiser, new StartedConstructionEventArgs(_target));
			_target.BuildProgress.Returns(0.5f);
			_target.BuildableProgress += Raise.EventWith(_target, new BuildProgressEventArgs(_target));

			Assert.AreEqual(2, _targetFoundEmittedCount);
		}

		[Test]
		public void BuildableThatWasFound_Destroyed_EmitsTargetLost()
		{
			BuildableReachesHalfway_EmitsTargetFound();

			bool wasTargetLostEmitted = false;

			_targetFinder.TargetLost += (object sender, TargetEventArgs e) => 
			{
				wasTargetLostEmitted = true;

				Assert.AreEqual(_targetFinder, sender);
				Assert.AreEqual(_target, e.Target);
			};

			_target.Destroyed += Raise.EventWith(_target, new DestroyedEventArgs(_target));

			Assert.IsTrue(wasTargetLostEmitted);
		}

		[Test]
		public void BuildableThatWasNotFound_Destroyed_DoesNotEmitsTargetLost()
		{
			_enemyCruiser.StartedConstruction += Raise.EventWith(_enemyCruiser, new StartedConstructionEventArgs(_target));
			_target.BuildProgress.Returns(0.1f);

			bool wasTargetLostEmitted = false;

			_targetFinder.TargetLost += (object sender, TargetEventArgs e) => 
			{
				wasTargetLostEmitted = true;
			};

			_target.Destroyed += Raise.EventWith(_target, new DestroyedEventArgs(_target));

			Assert.IsFalse(wasTargetLostEmitted);
		}

		private void OnTargetFound(object sender, TargetEventArgs e)
		{
			_targetFoundEmittedCount++;
			Assert.AreEqual(_targetFinder, sender);
			Assert.AreEqual(_expectedTargetFound, e.Target);
		}
	}
}
