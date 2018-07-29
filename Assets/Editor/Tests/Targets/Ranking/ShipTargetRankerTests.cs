using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.Ranking
{
    /// <summary>
    /// Note:  Targets are ranked in ascending priority.
    /// </summary>
    public class ShipTargetRankerTests : BaseTargetRankerTests
	{
		[SetUp]
		public override void SetuUp()
		{
            // FELIX  Fix :P
			_targetRanker = new ShipTargetRanker(null);
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
