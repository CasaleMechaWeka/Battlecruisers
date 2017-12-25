using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.Ranking
{
    /// <summary>
    /// Note:  Targets are ranked in ascending priority.
    /// </summary>
    public class BomberTargetRankerTests : BaseTargetRankerTests
	{
		[SetUp]
		public override void SetuUp()
		{
			_targetRanker = new BomberTargetRanker();
		}

		[Test]
		public void LowValueAntiAir_TrumpsHighValue()
		{
			ITarget lowValueAnitAir = CreateMockTarget(TargetValue.Low, TargetType.Aircraft);
			ITarget highValue = CreateMockTarget(TargetValue.High);

			_rankedTargets = new List<ITarget>(new ITarget[] { lowValueAnitAir, highValue });
			RankTargets();

			_expectedOrder = new List<ITarget>(new ITarget[] { highValue, lowValueAnitAir });

			Assert.AreEqual(_expectedOrder, _rankedTargets);
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

		[Test]
		public void LowValueAntiAir_TrumpsHighValueAntiCruiser()
		{
			ITarget lowValueAnitAir = CreateMockTarget(TargetValue.Low, TargetType.Aircraft);
			ITarget highValueAntiCruiser = CreateMockTarget(TargetValue.High, TargetType.Cruiser);

			_rankedTargets = new List<ITarget>(new ITarget[] { lowValueAnitAir, highValueAntiCruiser });
			RankTargets();

			_expectedOrder = new List<ITarget>(new ITarget[] { highValueAntiCruiser, lowValueAnitAir });

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}
	}
}
