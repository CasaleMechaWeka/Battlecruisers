using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.Targets.Ranking
{
    /// <summary>
    /// Note:  Targets are ranked in ascending priority.
    /// </summary>
    public class OffensiveBuildableTargetRankerTests : TargetRankerTestsBase
	{
        private ITargetRanker _targetRanker;
        protected override ITargetRanker TargetRanker { get { return _targetRanker; } }

        [SetUp]
		public override void SetuUp()
		{
            base.SetuUp();
            _targetRanker = new OffensiveBuildableTargetRanker(_userChosenTargetProvider);
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

        protected override ITarget CreateHighestValueTarget()
        {
            return CreateMockTarget(TargetValue.High, TargetType.Cruiser);
        }
    }
}
