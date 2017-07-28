using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.Ranking
{
    /// <summary>
    /// Note:  Targets are ranked in ascending priority.
    /// </summary>
    public class BaseTargetRankerTests 
	{
		protected ITargetRanker _targetRanker;

		protected List<ITarget> _rankedTargets;
		protected IList<ITarget> _expectedOrder;

		[SetUp]
		public virtual void SetuUp()
		{
			_targetRanker = new BaseTargetRanker();
		}

		[Test]
		public void TargetsRankedByValue()
		{
			ITarget lowValue = CreateMockTarget(TargetValue.Low);
			ITarget mediumValue = CreateMockTarget(TargetValue.Medium);
			ITarget highValue = CreateMockTarget(TargetValue.High);

			_rankedTargets = new List<ITarget>(new ITarget[] { mediumValue, highValue, lowValue });
			RankTargets();

			_expectedOrder = new List<ITarget>(new ITarget[] { lowValue, mediumValue, highValue });

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}

		protected ITarget CreateMockTarget(TargetValue targetValue, params TargetType[] attackCapabilities)
		{
			ITarget target = Substitute.For<ITarget>();
			target.TargetValue.Returns(targetValue);
			target.AttackCapabilities.Returns(new List<TargetType>(attackCapabilities));
			return target;
		}

		protected void RankTargets()
		{
			_rankedTargets.Sort((x, y) => _targetRanker.RankTarget(x) - _targetRanker.RankTarget(y));
		}
	}
}
