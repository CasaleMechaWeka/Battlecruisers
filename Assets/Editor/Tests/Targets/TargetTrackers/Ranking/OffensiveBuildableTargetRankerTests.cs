using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.Targets.TargetTrackers.Ranking
{
    /// <summary>
    /// Note:  Targets are ranked in ascending priority.
    /// </summary>
    public class OffensiveBuildableTargetRankerTests : TargetRankerTestsBase
	{
        private ITargetRanker _targetRanker;
        protected override ITargetRanker TargetRanker => _targetRanker;

        [SetUp]
		public void SetuUp()
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
