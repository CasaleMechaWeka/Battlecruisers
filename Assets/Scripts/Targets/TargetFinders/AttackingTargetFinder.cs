using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetFinders
{
    /// <summary>
    /// Tracks the highest priority target that is attacking us.
    /// </summary>
    public class AttackingTargetFinder : ITargetFinder
    {
        private readonly ITargetFilter _targetFilter;
        private readonly IDamagable _parentDamagable;

        public event EventHandler<TargetEventArgs> TargetFound;
        public event EventHandler<TargetEventArgs> TargetLost;

        public AttackingTargetFinder(ITargetFilter targetFilter, IDamagable parentDamagable)
        {
            Helper.AssertIsNotNull(targetFilter, parentDamagable);

            _targetFilter = targetFilter;
            _parentDamagable = parentDamagable;
        }

        public void StartFindingTargets()
        {
            _parentDamagable.Damaged += _parentDamagable_Damaged;
        }

        private void _parentDamagable_Damaged(object sender, DamagedEventArgs e)
        {
            Logging.Log(Tags.TARGET_FINDER, "Parent damaged by: " + e.DamageSource);

            if (e.DamageSource != null
                && !e.DamageSource.IsDestroyed
                && _targetFilter.IsMatch(e.DamageSource))
            {
                e.DamageSource.Destroyed += AttackingTarget_Destroyed;

                if (TargetFound != null)
                {
                    TargetFound.Invoke(this, new TargetEventArgs(e.DamageSource));
                }
            }
        }

        private void AttackingTarget_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= AttackingTarget_Destroyed;

            if (TargetLost != null)
            {
                TargetLost.Invoke(this, new TargetEventArgs(e.DestroyedTarget));
            }
        }

        public void DisposeManagedState()
        {
            _parentDamagable.Damaged -= _parentDamagable_Damaged;
        }
    }
}
