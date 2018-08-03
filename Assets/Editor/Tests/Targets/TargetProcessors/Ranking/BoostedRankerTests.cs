using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetProcessors.Ranking
{
    public class BoostedRankerTests
    {
        private ITargetRanker _boostedRanker, _baseRanker;
        private ITarget _target;
        private int _rankBoost;

        [SetUp]
        public void TestSetup()
        {
            _baseRanker = Substitute.For<ITargetRanker>();
            _rankBoost = 33;

            _boostedRanker = new BoostedRanker(_baseRanker, _rankBoost);

            _target = Substitute.For<ITarget>();
        }

        [Test]
        public void RankTarget_ForwardsToBaseRanker_AddsBoost()
        {
            _baseRanker.RankTarget(_target).Returns(7);
            Assert.AreEqual(_rankBoost + 7, _boostedRanker.RankTarget(_target));
        }
    }
}