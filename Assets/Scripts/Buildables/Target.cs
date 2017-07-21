using System;
using System.Collections.Generic;
using BattleCruisers.Utils;
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
		public virtual bool IsDetectable { get { return true; } }
		public virtual TargetValue TargetValue { get { return TargetValue.Low; } }
		public virtual Vector2 Velocity { get { return new Vector2(0, 0); } }
		public Vector2 Position { get { return gameObject.transform.position; } }

		public event EventHandler<DestroyedEventArgs> Destroyed;
		public event EventHandler<HealthChangedEventArgs> HealthChanged;

		protected List<TargetType> _attackCapabilities;
		public virtual List<TargetType> AttackCapabilities { get { return _attackCapabilities; } }
		protected bool IsStaticallyInitialised { get { return _attackCapabilities != null; } }

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

		public virtual void StaticInitialise()
		{
			_health = maxHealth;
			_attackCapabilities = new List<TargetType>();
		}

		protected virtual void OnFullyRepaired() { }

		protected virtual void OnHealthGone()
		{
			Destroy();
		}

		public void Destroy()
		{
			OnDestroyed();
			InvokeDestroyedEvent();
			InternalDestroy();
		}

		protected virtual void InternalDestroy()
		{
			Destroy(gameObject);
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
