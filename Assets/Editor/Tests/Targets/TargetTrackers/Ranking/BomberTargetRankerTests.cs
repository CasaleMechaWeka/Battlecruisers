using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.Targets.TargetTrackers.Ranking
{
    /// <summary>
    /// Note:  Targets are ranked in ascending priority.
    /// </summary>
    public class BomberTargetRankerTests : TargetRankerTestsBase
    {
		private ITargetRanker _targetRanker;
        protected override ITargetRanker TargetRanker => _targetRanker;

        [SetUp]
		public void SetuUp()
		{
			_targetRanker = new BomberTargetRanker();
		}

		[Test]
		public void LowValueAntiAir_TrumpsHighValue()
		{
            ITarget lowValueAntiAir = CreateMockTarget(TargetValue.Low, TargetType.Aircraft);
			ITarget highValue = CreateMockTarget(TargetValue.High);

			_rankedTargets = new List<ITarget>() { lowValueAntiAir, highValue };
			RankTargets();

			_expectedOrder = new List<ITarget>() { highValue, lowValueAntiAir };

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}

		[Test]
		public void LowValueAntiCruiser_TrumpsHighValue()
		{
            ITarget lowValueAntiCruiser = CreateMockTarget(TargetValue.Low, TargetType.Cruiser);
			ITarget highValue = CreateMockTarget(TargetValue.High);

			_rankedTargets = new List<ITarget>() { lowValueAntiCruiser, highValue };
			RankTargets();

			_expectedOrder = new List<ITarget>() { highValue, lowValueAntiCruiser };

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}

		[Test]
		public void LowValueAntiAir_TrumpsHighValueAntiCruiser()
		{
            ITarget lowValueAntiAir = CreateMockTarget(TargetValue.Low, TargetType.Aircraft);
			ITarget highValueAntiCruiser = CreateMockTarget(TargetValue.High, TargetType.Cruiser);

			_rankedTargets = new List<ITarget>() { lowValueAntiAir, highValueAntiCruiser };
			RankTargets();

			_expectedOrder = new List<ITarget>() { highValueAntiCruiser, lowValueAntiAir };

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}
    }
}
