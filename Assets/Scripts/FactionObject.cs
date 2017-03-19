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

	public abstract class FactionObject : MonoBehaviour, IDamagable
	{
		public Faction faction;
		public float health;
		public bool IsDestroyed { get { return health <= 0; } }

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
		private Renderer _renderer;

		public string buildableName;
		public string description;
		public int numOfDronesRequired;
		public int buildTimeInS;

		public virtual float Damage { get { return 0; } }

		public virtual Vector3 Size 
		{ 
			get 
			{ 
				return _renderer.bounds.size; 
			} 
		}

		protected Sprite _sprite;
		public virtual Sprite Sprite
		{
			get
			{
				if (_sprite == null)
				{
					_sprite = GetComponent<SpriteRenderer>().sprite;
				}
				return _sprite;
			}
		}

		void Awake()
		{
			Debug.Log("BuildableObject.Awake()");
			_renderer = GetComponent<Renderer>();
		}
	}
}
