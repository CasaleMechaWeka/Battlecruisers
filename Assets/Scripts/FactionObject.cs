using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers
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
//
//	public interface IFactionObject : IDamagable
//	{
//		Faction Faction { get; }
//		GameObject GameObject { get; }
//	}

	public abstract class FactionObject : MonoBehaviour, IDamagable
	{
		public Faction faction;
		public float health;
		public bool IsDestroyed { get { return health <= 0; } }
//		public GameObject GameObject { get { return gameObject; } }

		public virtual void TakeDamage(float damageAmount)
		{
			health -= damageAmount;
			if (health <= 0)
			{
				Destroy(gameObject);
			}
		}
	}

	public abstract class BuildableObject : FactionObject
	{
		public string buildableName;
		public string description;
		public int numOfDronesRequired;
		public int buildTimeInS;
	}
}
