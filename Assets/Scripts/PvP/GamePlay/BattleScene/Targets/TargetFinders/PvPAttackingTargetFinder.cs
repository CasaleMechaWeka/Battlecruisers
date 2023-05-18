using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders
{
    /// <summary>
    /// Tracks the highest priority target that is attacking us.
    /// </summary>
    public class PvPAttackingTargetFinder : IPvPTargetFinder
    {
        private readonly IPvPDamagable _parentDamagable;
        private readonly IPvPTargetFilter _targetFilter;

        public event EventHandler<PvPTargetEventArgs> TargetFound;
        public event EventHandler<PvPTargetEventArgs> TargetLost;

        public PvPAttackingTargetFinder(IPvPDamagable parentDamagable, IPvPTargetFilter targetFilter)
        {
            PvPHelper.AssertIsNotNull(parentDamagable, targetFilter);

            _parentDamagable = parentDamagable;
            _targetFilter = targetFilter;

            _parentDamagable.Damaged += _parentDamagable_Damaged;
        }

        private void _parentDamagable_Damaged(object sender, PvPDamagedEventArgs e)
        {
            // Logging.Log(Tags.TARGET_FINDER, "Parent damaged by: " + e.DamageSource);

            if (e.DamageSource != null
                && !e.DamageSource.IsDestroyed
                && _targetFilter.IsMatch(e.DamageSource))
            {
                e.DamageSource.Destroyed += AttackingTarget_Destroyed;

                TargetFound?.Invoke(this, new PvPTargetEventArgs(e.DamageSource));
            }
        }

        private void AttackingTarget_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= AttackingTarget_Destroyed;

            TargetLost?.Invoke(this, new PvPTargetEventArgs(e.DestroyedTarget));
        }

        public void DisposeManagedState()
        {
            _parentDamagable.Damaged -= _parentDamagable_Damaged;
        }
    }
}
