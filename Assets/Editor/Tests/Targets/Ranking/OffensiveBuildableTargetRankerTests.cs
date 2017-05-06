using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets.Ranking
{
	/// <summary>
	/// Note:  Targets are ranked in ascending priority.
	/// </summary>
	public class OffensiveBuildableTargetRankerTests : BaseTargetRankerTests
	{
		[SetUp]
		public override void SetuUp()
		{
			_targetRanker = new OffensiveBuildableTargetRanker();
		}

		[Test]
		public void LowValueAntiCruiser_TrumpsHighValue()
		{
			ITarget lowValueAnitCruiser = CreateMockTarget(TargetValue.Low, TargetType.Cruiser);
			ITarget highValue = CreateMockTarget(TargetValue.High);

			_rankedTargets = new List<ITarget>(new ITarget[] { lowValueAnitCruiser, highValue });
			RankTargets();

			_expectedOrder = new List<ITarget>(new ITarget[] { highValue, lowValueAnitCruiser });

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}
	}
}
