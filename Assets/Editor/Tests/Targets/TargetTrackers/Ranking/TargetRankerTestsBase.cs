using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.Targets.TargetTrackers.Ranking
{
    /// <summary>
    /// Note:  Targets are ranked in ascending priority.
    /// </summary>
    public abstract class TargetRankerTestsBase
    {
        protected List<ITarget> _rankedTargets;
		protected IList<ITarget> _expectedOrder;

		protected abstract ITargetRanker TargetRanker { get; }

		protected ITarget CreateMockTarget(TargetValue targetValue, params TargetType[] attackCapabilities)
		{
			ITarget target = Substitute.For<ITarget>();
			target.TargetValue.Returns(targetValue);
			target.AttackCapabilities.Returns(new ReadOnlyCollection<TargetType>(attackCapabilities));
			return target;
		}

		protected void RankTargets()
		{
			_rankedTargets.Sort((x, y) => TargetRanker.RankTarget(x) - TargetRanker.RankTarget(y));
		}
    }
}
