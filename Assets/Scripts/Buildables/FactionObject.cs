using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleCruisers.Buildables
{
	public enum Faction
	{
		Blues, Reds
	}

	public interface IDamagable
	{
		// FELIX  Avoid polling?  Use events?
		bool IsDestroyed { get; }

		void TakeDamage(float damageAmount);
//		void Repair(float repairAmount);
		// FELIX  On fully damaged/repaired?
	}

	public abstract class FactionObject : MonoBehaviour, IDamagable
	{
		public float health;
		public bool IsDestroyed { get { return health == 0; } }
		public Faction Faction { get; protected set; }

		public event EventHandler Destroyed;

		public virtual void TakeDamage(float damageAmount)
		{
			health -= damageAmount;
			if (health <= 0)
			{
				health = 0;
				Destroy(gameObject);
			}
		}

		void OnDestroy()
		{
			OnDestroyed();
		}

		protected virtual void OnDestroyed() 
		{ 
			if (Destroyed != null)
			{
				Destroyed.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
