using System;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProviders
{
    /// <summary>
    /// Provides the highest priority target that is either:
    /// A) In range (in range target processor must assign targets to this class
    ///     via the ITargetConsumer.Target property).
    /// B) Attacking the parent damagable
    /// </summary>
    /// FELIX  Could potentiall refactor. 
    /// 1. Create AttackingTargetFinder/Provider (give attacking targets high priority)
    /// 2. Use CompositeTargetTracker (when it exists :P)
    /// FELIX  Remove :)
    public class HighestPriorityTargetProvider : BroadcastingTargetProvider, IHighestPriorityTargetProvider
    {
        private readonly ITargetRanker _targetRanker;
        private readonly ITargetFilter _attackingTargetFilter;
        private readonly IDamagable _parentDamagable;
        private readonly IRankedTarget _nullTarget;

        // Highest priority target that is attacking our parent
        private IRankedTarget _attackingTarget;
        private IRankedTarget AttackingTarget
        {
            get { return _attackingTarget; }
            set
            {
                _attackingTarget = value;

                if (_attackingTarget.Target != null)
                {
                    _attackingTarget.Target.Destroyed += AttackingTarget_Destroyed;
				}

                UpdateHighestPriorityTarget();
            }
        }

        // Highest priority in range target
        private IRankedTarget _inRangeTarget;
        ITarget ITargetConsumer.Target
        {
            set
            {
                Logging.Log(Tags.TARGET_PROVIDERS, "Assigned target: " + value);

                int targetRank = value != null ? _targetRanker.RankTarget(value) : BaseTargetRanker.MIN_TARGET_RANK;
                _inRangeTarget = new RankedTarget(value, targetRank);
                UpdateHighestPriorityTarget();

                if (NewInRangeTarget != null)
                {
                    NewInRangeTarget.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler NewInRangeTarget;

        public HighestPriorityTargetProvider(ITargetRanker targetRanker, ITargetFilter attackingTargetFilter, IDamagable parentDamagable)
        {
            Helper.AssertIsNotNull(targetRanker, attackingTargetFilter, parentDamagable);

            _targetRanker = targetRanker;
            _attackingTargetFilter = attackingTargetFilter;
            _parentDamagable = parentDamagable;

            _parentDamagable.Damaged += _parentDamagable_Damaged;

            _nullTarget = new RankedTarget(target: null, rank: BaseTargetRanker.MIN_TARGET_RANK);
            _attackingTarget = _nullTarget;
            _inRangeTarget = _nullTarget;

            Target = null;
        }

        private void _parentDamagable_Damaged(object sender, DamagedEventArgs e)
        {
            Logging.Log(Tags.TARGET_PROVIDERS, "Parent damaged by: " + e.DamageSource);

            if (e.DamageSource != null 
                && !e.DamageSource.IsDestroyed
                && _attackingTargetFilter.IsMatch(e.DamageSource))
            {
                int newRank = _targetRanker.RankTarget(e.DamageSource);

                if (newRank > AttackingTarget.Rank)
                {
                    AttackingTarget = new RankedTarget(e.DamageSource, newRank);
                }
            }
        }

		private void UpdateHighestPriorityTarget()
		{
            Target = AttackingTarget.Rank > _inRangeTarget.Rank ? AttackingTarget.Target : _inRangeTarget.Target;

            Logging.Log(Tags.TARGET_PROVIDERS, "Highest priority target: " + Target);
		}

        private void AttackingTarget_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= AttackingTarget_Destroyed;

            if (ReferenceEquals(AttackingTarget.Target, e.DestroyedTarget))
            {
                // Destroyed attacking target was highest priority attacking target
                AttackingTarget = _nullTarget;
            }
        }

        public void DisposeManagedState()
        {
            _parentDamagable.Damaged -= _parentDamagable_Damaged;
        }
    }
}
