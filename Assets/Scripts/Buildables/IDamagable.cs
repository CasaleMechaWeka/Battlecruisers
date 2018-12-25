using System;
using UnityEngine;

namespace BattleCruisers.Buildables
{
    public class DamagedEventArgs : EventArgs
    {
        public ITarget DamageSource { get; private set; }

        public DamagedEventArgs(ITarget damageSource)
        {
            DamageSource = damageSource;
        }
    }

	public interface IDamagable : IDestructable
	{
		/// <value><c>true</c> if health is 0; otherwise, <c>false</c>.</value>
		bool IsDestroyed { get; }
		float Health { get; }
        float MaxHealth { get; }
		GameObject GameObject { get; }

        event EventHandler<DamagedEventArgs> Damaged;
		event EventHandler HealthChanged;

		void TakeDamage(float damageAmount, ITarget damageSource);
	}
}
