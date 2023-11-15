using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public class PvPDestroyedEventArgs : EventArgs
    {
        public IPvPTarget DestroyedTarget { get; }

        public PvPDestroyedEventArgs(IPvPTarget destroyedTarget)
        {
            DestroyedTarget = destroyedTarget;
        }
    }

    public class PvPDamagedEventArgs : EventArgs
    {
        public IPvPTarget DamageSource { get; }

        public PvPDamagedEventArgs(IPvPTarget damageSource)
        {
            DamageSource = damageSource;
        }
    }

    public interface IPvPDamagable
    {
        /// <value><c>true</c> if health is 0; otherwise, <c>false</c>.</value>
        bool IsDestroyed { get; }
        float Health { get; }
        float MaxHealth { get; }
        GameObject GameObject { get; }
        IPvPTarget LastDamagedSource { get; }

        event EventHandler<PvPDamagedEventArgs> Damaged;
        event EventHandler HealthChanged;
        // When health reaches 0
        event EventHandler<PvPDestroyedEventArgs> Destroyed;

        void TakeDamage(float damageAmount, IPvPTarget damageSource);
        void Destroy();
    }
}

