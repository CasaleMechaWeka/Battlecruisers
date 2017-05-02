using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// FELIX  Change IFactionable to ITarget?
namespace BattleCruisers.Buildables
{
	public enum Faction
	{
		Blues, Reds
	}

	public enum TargetType
	{
		Aircraft, Ships, Cruiser, Buildings
	}

	public class DestroyedEventArgs : EventArgs
	{
		public IFactionable DestroyedFactionable { get; private set; }

		public DestroyedEventArgs(IFactionable destroyedFactionable)
		{
			DestroyedFactionable = destroyedFactionable;
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
		/// <value><c>true</c> if healht is 0; otherwise, <c>false</c>.</value>
		bool IsDestroyed { get; }
		float Health { get; }

		// When health reaches 0
		event EventHandler<DestroyedEventArgs> Destroyed;

		// When health changes
		event EventHandler<HealthChangedEventArgs> HealthChanged;

		// FELIX  Unused?  Remove?
		// When health reaches its maximum value
		event EventHandler FullyRepaired;

		void TakeDamage(float damageAmount);
		void Repair(float repairAmount);
	}

	public interface IFactionable : IDamagable
	{
		Faction Faction { get; }
		TargetType TargetType { get; }
		GameObject GameObject { get; }
		Vector2 Velocity { get; }
	}
}
