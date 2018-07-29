using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetProviders;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.Targets.TargetProcessors.Ranking
{
    /// <summary>
    /// Note:  Targets are ranked in ascending priority.
    /// </summary>
    public abstract class TargetRankerTestsBase
    {
        protected ITargetProvider _userChosenTargetProvider;

        protected List<ITarget> _rankedTargets;
		protected IList<ITarget> _expectedOrder;

		protected abstract ITargetRanker TargetRanker { get; }

		[SetUp]
		public virtual void SetuUp()
		{
            _userChosenTargetProvider = Substitute.For<ITargetProvider>();
		}

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

        [Test]
        public void UserChosenTarget_TrumpsHighestValueTarget()
        {
            ITarget highValueTarget = CreateHighestValueTarget();
            ITarget lowValueUserChosenTarget = CreateMockTarget(TargetValue.Low);
            _userChosenTargetProvider.Target.Returns(lowValueUserChosenTarget);

            _rankedTargets = new List<ITarget>() { lowValueUserChosenTarget, highValueTarget };
            RankTargets();

            _expectedOrder = new List<ITarget>() { highValueTarget, lowValueUserChosenTarget };

            Assert.AreEqual(_expectedOrder, _rankedTargets);
        }

        protected abstract ITarget CreateHighestValueTarget();
    }
}
