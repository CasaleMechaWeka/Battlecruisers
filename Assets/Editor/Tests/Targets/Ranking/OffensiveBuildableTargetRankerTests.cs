using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using NUnit.Framework;

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
            // FELIX  Fix :P
			_targetRanker = new OffensiveBuildableTargetRanker(null);
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
