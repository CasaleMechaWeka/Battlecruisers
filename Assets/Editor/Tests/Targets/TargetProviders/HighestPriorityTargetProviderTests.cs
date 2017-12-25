using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetProviders;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets.TargetProviders
{
    public class HighestPriorityTargetProviderTests
    {
        private IHighestPriorityTargetProvider _highestPriorityTargetProvider;
        private ITargetProvider _targetProvider;
        private ITargetConsumer _targetConsumer;

        private ITargetRanker _targetRanker;
        private IDamagable _parentDamagable;
        private ITarget _inRangeTarget, _inRangeTarget2, _attackingTarget, _attackingTarget2;
        private int _lowRank, _highRank;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _targetRanker = Substitute.For<ITargetRanker>();
            _parentDamagable = Substitute.For<IDamagable>();

            _inRangeTarget = Substitute.For<ITarget>();
            _inRangeTarget2 = Substitute.For<ITarget>();

            _attackingTarget = Substitute.For<ITarget>();
            _attackingTarget2 = Substitute.For<ITarget>();

            _lowRank = 1;
            _highRank = 2;

            _highestPriorityTargetProvider = new HighestPriorityTargetProvider(_targetRanker, _parentDamagable);
            _targetProvider = _highestPriorityTargetProvider;
            _targetConsumer = _highestPriorityTargetProvider;
        }

        [Test]
        public void Initialisation_TargetIsNull()
        {
            Assert.IsNull(_targetProvider.Target);
        }

        #region In range target
        [Test]
        public void NewInRangeTarget_EmitsEvents()
        {
            int eventCount = 0;
            _highestPriorityTargetProvider.NewInRangeTarget += (sender, e) => eventCount++;

            _targetConsumer.Target = null;

            Assert.AreEqual(1, eventCount);
        }

        [Test]
        public void NullInRangeTarget_DoesNothing()
        {
            _targetConsumer.Target = null;

            _targetRanker.DidNotReceiveWithAnyArgs().RankTarget(default(ITarget));
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void FirstInRangeTarget_BecomesOverallTarget()
        {
            NewInRangeTarget(_inRangeTarget, _lowRank);
            Assert.AreSame(_inRangeTarget, _targetProvider.Target);
        }

        /// <summary>
        /// We trust the target processor that is assigning our in range target
        /// to prioritise correclty, so we replace our in range target regardless
        /// of rank compared to the current in range target.  For example, this 
        /// may happen because the current in range target is destroyed.
        /// </summary>
        [Test]
        public void SecondInRangeTarget_LowerPriority_StillReplacesOverallTarget()
        {
            // First in range target, high priority
            NewInRangeTarget(_inRangeTarget, _highRank);
            Assert.AreSame(_inRangeTarget, _targetProvider.Target);

            // Second in range target, low priority
            NewInRangeTarget(_inRangeTarget2, _lowRank);
            Assert.AreSame(_inRangeTarget2, _targetProvider.Target);
        }
        #endregion In range target

        #region Attacking target
        [Test]
        public void NullAttackingTarget_DoesNothing()
        {
            _parentDamagable.Damaged += Raise.EventWith(_parentDamagable, new DamagedEventArgs(damageSource: null));

            _targetRanker.DidNotReceiveWithAnyArgs().RankTarget(default(ITarget));
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void FirstAttackingTarget_BecomesOverallTarget()
        {
            NewAttackingTarget(_attackingTarget, _lowRank);
            Assert.AreSame(_attackingTarget, _targetProvider.Target);
        }

        [Test]
        public void SecondAttackingTarget_SameRank_DoesNothing()
        {
            // First attacking target, high priority
            NewAttackingTarget(_attackingTarget, _highRank);
            Assert.AreSame(_attackingTarget, _targetProvider.Target);

            // Second attacking target, low priority
            NewAttackingTarget(_attackingTarget2, _lowRank);
            Assert.AreSame(_attackingTarget, _targetProvider.Target);
        }

        [Test]
        public void SecondAttackingTarget_HigherRank_ReplacesOverallTarget()
        {
            // First attacking target, low priority
            NewAttackingTarget(_attackingTarget, _lowRank);
            Assert.AreSame(_attackingTarget, _targetProvider.Target);

            // Second attacking target, high priority
            NewAttackingTarget(_attackingTarget2, _highRank);
            Assert.AreSame(_attackingTarget2, _targetProvider.Target);
        }
        #endregion Attacking target

        #region Combination of in range and attacking targets
        [Test]
        public void InRangeTarget_SameRankAttackingTarget_OverallTargetIsInRangeTarget()
        {
            // In range target, low priority
            NewInRangeTarget(_inRangeTarget, _lowRank);
            Assert.AreSame(_inRangeTarget, _targetProvider.Target);

            // Attacking target, same low priority
            NewAttackingTarget(_attackingTarget, _lowRank);
            Assert.AreSame(_inRangeTarget, _targetProvider.Target);
        }

        [Test]
        public void InRangeTarget_HigherRankAttackingTarget_OverallTargetIsAttackingTarget()
        {
            // In range target, low priority
            NewInRangeTarget(_inRangeTarget, _lowRank);
            Assert.AreSame(_inRangeTarget, _targetProvider.Target);

            // Attacking target, high priority
            NewAttackingTarget(_attackingTarget, _highRank);
            Assert.AreSame(_attackingTarget, _targetProvider.Target);
        }

        [Test]
        public void AttackingTarget_SameRankInRangeTarget_OverallTargetIsInRangeTarget()
        {
            // Attacking target, low priority
            NewAttackingTarget(_attackingTarget, _lowRank);
            Assert.AreSame(_attackingTarget, _targetProvider.Target);

            // In range target, same low priority
            NewInRangeTarget(_inRangeTarget, _lowRank);
            Assert.AreSame(_inRangeTarget, _targetProvider.Target);
        }

        [Test]
        public void AttackingTarget_LowerRankInRangeTarget_OverallTargetIsAttackingTarget()
        {
            // Attacking target, high priority
            NewAttackingTarget(_attackingTarget, _highRank);
            Assert.AreSame(_attackingTarget, _targetProvider.Target);

            // In range target, low priority
            NewInRangeTarget(_inRangeTarget, _lowRank);
            Assert.AreSame(_attackingTarget, _targetProvider.Target);   
        }
        #endregion Combination of in range and attacking targets

        private void NewInRangeTarget(ITarget target, int rank)
        {
            _targetRanker.RankTarget(target).Returns(rank);
            _targetConsumer.Target = target;
            _targetRanker.Received().RankTarget(target);
        }

        private void NewAttackingTarget(ITarget target, int rank)
        {
            _targetRanker.RankTarget(target).Returns(rank);
            _parentDamagable.Damaged += Raise.EventWith(_parentDamagable, new DamagedEventArgs(target));
            _targetRanker.Received().RankTarget(target);
        }
    }
}
