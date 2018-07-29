using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.Targets.Ranking
{
    /// <summary>
    /// Note:  Targets are ranked in ascending priority.
    /// </summary>
    public class BaseTargetRankerTests : TargetRankerTestsBase
	{
		private ITargetRanker _targetRanker;
        protected override ITargetRanker TargetRanker { get { return _targetRanker; } }

        [SetUp]
		public override void SetuUp()
		{
            base.SetuUp();
			_targetRanker = new BaseTargetRanker(_userChosenTargetProvider);
		}

		[Test]
		public void TargetsRankedByValue()
		{
			ITarget lowValue = CreateMockTarget(TargetValue.Low);
			ITarget mediumValue = CreateMockTarget(TargetValue.Medium);
			ITarget highValue = CreateMockTarget(TargetValue.High);

			_rankedTargets = new List<ITarget>() { mediumValue, highValue, lowValue };
			RankTargets();

			_expectedOrder = new List<ITarget>() { lowValue, mediumValue, highValue };

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}

        protected override ITarget CreateHighestValueTarget()
        {
            return CreateMockTarget(TargetValue.High);
        }
    }
}
