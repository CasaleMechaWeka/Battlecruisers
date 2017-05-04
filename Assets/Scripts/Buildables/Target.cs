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
	public abstract class Target : MonoBehaviour, ITarget
	{
		private bool _wasDestroyTriggeredInternally;

		public float maxHealth;
		public bool IsDestroyed { get { return Health == 0; } }
		public Faction Faction { get; protected set; }
		public GameObject GameObject { get { return gameObject; } }
		public abstract TargetType TargetType { get; }
		public virtual Vector2 Velocity { get { return new Vector2(0, 0); } }

		public event EventHandler<DestroyedEventArgs> Destroyed;
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
					Logging.Log(Tags.TARGET, $"HealthChanged  {this}");
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
				Destroyed.Invoke(this, new DestroyedEventArgs(this));
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
