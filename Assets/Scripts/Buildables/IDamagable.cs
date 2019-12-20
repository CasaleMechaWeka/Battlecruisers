using System;
using UnityEngine;

namespace BattleCruisers.Buildables
{
    public class DestroyedEventArgs : EventArgs
	{
		public ITarget DestroyedTarget { get; }

		public DestroyedEventArgs(ITarget destroyedTarget)
		{
			DestroyedTarget = destroyedTarget;
		}
	}

    public class DamagedEventArgs : EventArgs
    {
        public ITarget DamageSource { get; }

        public DamagedEventArgs(ITarget damageSource)
        {
            DamageSource = damageSource;
        }
    }

	public interface IDamagable
	{
		/// <value><c>true</c> if health is 0; otherwise, <c>false</c>.</value>
		bool IsDestroyed { get; }
		float Health { get; }
        float MaxHealth { get; }
		GameObject GameObject { get; }
        ITarget LastDamagedSource { get; }

        event EventHandler<DamagedEventArgs> Damaged;
		event EventHandler HealthChanged;
        // When health reaches 0
        event EventHandler<DestroyedEventArgs> Destroyed;

		void TakeDamage(float damageAmount, ITarget damageSource);
		void Destroy();
	}
}
