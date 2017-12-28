using System;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProviders
{
    /// <summary>
    /// Provides the highest priority target that is either:
    /// A) In range (in range target processor must assign targets to this class
    ///     via the ITargetConsumer.Target property).
    /// B) Attacking the parent damagable
    /// </summary>
    public class HighestPriorityTargetProvider : BroadcastingTargetProvider, IHighestPriorityTargetProvider
    {
        private readonly ITargetRanker _targetRanker;
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

        public HighestPriorityTargetProvider(ITargetRanker targetRanker, IDamagable parentDamagable)
        {
            Helper.AssertIsNotNull(targetRanker, parentDamagable);

            _targetRanker = targetRanker;
            _parentDamagable = parentDamagable;

            _parentDamagable.Damaged += _parentDamagable_Damaged;

            _nullTarget = new RankedTarget(BaseTargetRanker.MIN_TARGET_RANK, target: null);
            _attackingTarget = _nullTarget;
            _inRangeTarget = _nullTarget;

            Target = null;
        }

        // Highest priority in range target
        private IRankedTarget _inRangeTarget;
        ITarget ITargetConsumer.Target 
        { 
            set 
            {
                Logging.Log(Tags.TARGET_PROVIDERS, "Assigned target: " + value);

                int targetRank = value != null ? _targetRanker.RankTarget(value) : BaseTargetRanker.MIN_TARGET_RANK;
                _inRangeTarget = new RankedTarget(targetRank, value);
                UpdateHighestPriorityTarget();

                if (NewInRangeTarget != null)
                {
                    NewInRangeTarget.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler NewInRangeTarget;

        private void _parentDamagable_Damaged(object sender, DamagedEventArgs e)
        {
            Logging.Log(Tags.TARGET_PROVIDERS, "Parent damaged by: " + e.DamageSource);

            if (e.DamageSource != null && !e.DamageSource.IsDestroyed)
            {
                int newRank = _targetRanker.RankTarget(e.DamageSource);

                if (newRank > AttackingTarget.Rank)
                {
                    AttackingTarget = new RankedTarget(newRank, e.DamageSource);
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
