using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
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
		event EventHandler Destroyed;
		// When health changes
		event EventHandler<HealthChangedEventArgs> HealthChanged;
		// When health reaches its maximum value
		event EventHandler FullyRepaired;

		void TakeDamage(float damageAmount);
		void Repair(float repairAmount);
	}

	public abstract class FactionObject : MonoBehaviour, IDamagable
	{
		public float maxHealth;
		public bool IsDestroyed { get { return maxHealth == 0; } }
		public Faction Faction { get; protected set; }

		public event EventHandler Destroyed;
		public event EventHandler<HealthChangedEventArgs> HealthChanged;
		public event EventHandler FullyRepaired;

		private float _health;
		public float Health
		{
			get { return _health; }
			private set
			{
				if (value >= maxHealth)
				{
					_health = maxHealth;

					OnFullyRepaired();

					if (FullyRepaired != null)
					{
						FullyRepaired.Invoke(this, EventArgs.Empty);
					}
				}
				else if (value <= 0)
				{
					_health = 0;

					OnDestroyed();

					if (Destroyed != null)
					{
						Destroyed.Invoke(this, EventArgs.Empty);
					}
				}
				else
				{
					_health = value;
				}

				if (HealthChanged != null)
				{
					HealthChanged.Invoke(this, new HealthChangedEventArgs(_health));
				}
			}
		}

		public void Awake()
		{
			_health = maxHealth;

			OnAwake();
		}

		protected virtual void OnAwake() { }

		protected virtual void OnFullyRepaired() { }

		protected virtual void OnDestroyed()
		{
			Destroy(gameObject);
		}

		public void TakeDamage(float damageAmount)
		{
			if (Health <= 0)
			{
				int wtf = 12;
			}

			Assert.IsTrue(Health > 0);
			Health -= damageAmount;
			OnTakeDamage();
		}

		protected virtual void OnTakeDamage() { }

		public void Repair(float repairAmount)
		{
			Assert.IsTrue(Health < maxHealth);
			Health += repairAmount;
			OnRepair();
		}

		protected virtual void OnRepair() { }
	}
}
