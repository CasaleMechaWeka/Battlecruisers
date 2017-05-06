using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

	/// <summary>
	/// Used for prioritising targets.  Can become a lot more fine graind :)
	/// </summary>
	public enum TargetValue
	{
		Low, Medium, High
	}

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
		/// <value><c>true</c> if healht is 0; otherwise, <c>false</c>.</value>
		bool IsDestroyed { get; }
		float Health { get; }

		// When health reaches 0
		event EventHandler<DestroyedEventArgs> Destroyed;

		// When health changes
		event EventHandler<HealthChangedEventArgs> HealthChanged;

		void TakeDamage(float damageAmount);
		void Repair(float repairAmount);
	}

	public interface ITarget : IDamagable
	{
		Faction Faction { get; }
		TargetType TargetType { get; }
		GameObject GameObject { get; }
		Vector2 Velocity { get; }
		IList<TargetType> AttackCapabilities { get; }
		TargetValue TargetValue { get; }
	}
}
