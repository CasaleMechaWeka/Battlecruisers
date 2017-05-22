using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI.BattleScene;
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
		public float maxHealth;
		public bool IsDestroyed { get { return Health == 0; } }
		public Faction Faction { get; protected set; }
		public GameObject GameObject { get { return gameObject; } }
		public abstract TargetType TargetType { get; }
		public virtual TargetValue TargetValue { get { return TargetValue.Low; } }
		public virtual Vector2 Velocity { get { return new Vector2(0, 0); } }

		public event EventHandler<DestroyedEventArgs> Destroyed;
		public event EventHandler<HealthChangedEventArgs> HealthChanged;

		protected IList<TargetType> _attackCapabilities;
		public virtual IList<TargetType> AttackCapabilities { get { return _attackCapabilities; } }

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
					Logging.Log(Tags.TARGET, "HealthChanged  " + this);
					HealthChanged.Invoke(this, new HealthChangedEventArgs(_health));
				}
			}
		}

		public void Awake()
		{
			_health = maxHealth;
			_attackCapabilities = new List<TargetType>();

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
			OnDestroyed();
			InvokeDestroyedEvent();

			// All targets are wrapped by a UnitWrapper or BuildingWrapper, which contains
			// both the target and the health bar.  Hence destroy wrapper, so health bar
			// gets destroyed at the same time as the target.
			Destroy(transform.parent.gameObject);
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
	}
}
