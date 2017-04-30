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
	}

	public abstract class FactionObject : MonoBehaviour, IFactionable
	{
		private bool _wasDestroyTriggeredInternally;

		public float maxHealth;
		public bool IsDestroyed { get { return Health == 0; } }
		public Faction Faction { get; protected set; }
		public GameObject GameObject { get { return gameObject; } }
		public abstract TargetType TargetType { get; }

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
					OnHealthGone();
				}
				else
				{
					_health = value;
				}

				if (HealthChanged != null)
				{
					Logging.Log(Tags.FACTION_OBJECT, $"HealthChanged  {this}");
					HealthChanged.Invoke(this, new HealthChangedEventArgs(_health));
				}
			}
		}

		public void Awake()
		{
			_wasDestroyTriggeredInternally = false;
			_health = maxHealth;

			OnAwake();
		}

		protected virtual void OnAwake() { }

		protected virtual void OnFullyRepaired() { }

		protected virtual void OnHealthGone()
		{
			Destroy();
		}

		public void Destroy()
		{
			_wasDestroyTriggeredInternally = true;

			Destroy(gameObject);
			OnDestroyed();
			InvokeDestroyedEvent();
		}

		protected virtual void OnDestroyed() { }

		protected void InvokeDestroyedEvent()
		{
			if (Destroyed != null)
			{
				Destroyed.Invoke(this, EventArgs.Empty);
			}
		}

		public void TakeDamage(float damageAmount)
		{
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

		/// <summary>
		/// We should only ever be destroyed via our Destroy() method, not via Unity's
		/// Destroy(gameObject).  This ensures our OnDestroyed callback and Destroyed
		/// events are always called.
		/// </summary>
		void OnDestroy()
		{
			Assert.IsTrue(_wasDestroyTriggeredInternally);
		}
	}
}
