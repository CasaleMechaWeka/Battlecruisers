using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildable
{
    public class DestroyedEventArgs : EventArgs
    {
        public IPvPTarget DestroyedTarget { get; }

        public DestroyedEventArgs(IPvPTarget destroyedTarget)
        {
            DestroyedTarget = destroyedTarget;
        }
    }

    public class DamagedEventArgs : EventArgs
    {
        public IPvPTarget DamageSource { get; }

        public DamagedEventArgs(IPvPTarget damageSource)
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

        event EventHandler<DamagedEventArgs> Damaged;
        event EventHandler HealthChanged;
        // When health reaches 0
        event EventHandler<DestroyedEventArgs> Destroyed;

        void TakeDamage(float damageAmount, IPvPTarget damageSource);
        void Destroy();
    }
}

