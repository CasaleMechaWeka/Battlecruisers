using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.Targets.TargetTrackers.Ranking
{
    /// <summary>
    /// Note:  Targets are ranked in ascending priority.
    /// </summary>
    public class ShipTargetRankerTests : TargetRankerTestsBase
	{
        private ITargetRanker _targetRanker;
        protected override ITargetRanker TargetRanker => _targetRanker;

        [SetUp]
		public void SetuUp()
		{
            _targetRanker = new ShipTargetRanker();
		}

		[Test]
		public void LowValueAntiShip_TrumpsHighValue()
		{
            ITarget lowValueAntiShip = CreateMockTarget(TargetValue.Low, TargetType.Ships);
			ITarget highValue = CreateMockTarget(TargetValue.High);

			_rankedTargets = new List<ITarget>(new ITarget[] { lowValueAntiShip, highValue });
			RankTargets();

			_expectedOrder = new List<ITarget>(new ITarget[] { highValue, lowValueAntiShip });

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}
    }
}
