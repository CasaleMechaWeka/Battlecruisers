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

	public interface IDamagable
	{
		bool IsDestroyed { get; }

		event EventHandler Destroyed;
		event EventHandler FullyRepaired;

		void TakeDamage(float damageAmount);
	}

	public abstract class FactionObject : MonoBehaviour, IDamagable
	{
		public float maxHealth;
		public bool IsDestroyed { get { return maxHealth == 0; } }
		public Faction Faction { get; protected set; }

		public event EventHandler Destroyed;
		public event EventHandler FullyRepaired;

		private float _health;
		protected float Health
		{
			get { return _health; }
			set
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
			}
		}

		void Start()
		{
			Health = maxHealth;
		}

		protected virtual void OnFullyRepaired() { }

		protected virtual void OnDestroyed()
		{
			Destroy(gameObject);
		}

		public virtual void TakeDamage(float damageAmount)
		{
			Assert.IsTrue(Health > 0);
			Health -= damageAmount;
		}
	}
}
