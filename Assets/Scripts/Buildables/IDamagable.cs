using System;
using UnityEngine;

namespace BattleCruisers.Buildables
{
    public class DestroyedEventArgs : EventArgs
	{
		public ITarget DestroyedTarget { get; private set; }

		public DestroyedEventArgs(ITarget destroyedTarget)
		{
			DestroyedTarget = destroyedTarget;
		}
	}

	public class HealthChangedEventArgs : EventArgs
	{
		public float NewHealth { get; private set; }

		public HealthChangedEventArgs(float newHealth)
		{
			NewHealth = newHealth;
		}
	}

	public interface IDamagable
	{
		/// <value><c>true</c> if health is 0; otherwise, <c>false</c>.</value>
		bool IsDestroyed { get; }
		float Health { get; }
		GameObject GameObject { get; }

		// When health reaches 0
		event EventHandler<DestroyedEventArgs> Destroyed;

		// When health changes
		event EventHandler<HealthChangedEventArgs> HealthChanged;

		void TakeDamage(float damageAmount);
		void Destroy();
	}
}
