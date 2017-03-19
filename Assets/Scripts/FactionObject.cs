using BattleCruisers.Cruisers;
using BattleCruisers.UI;
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

		protected UIManager _uiManager;
		protected Cruiser _parentCruiser;
		protected Cruiser _enemyCruiser;

		public string buildableName;
		public string description;
		public int numOfDronesRequired;
		public int buildTimeInS;
		public SlotType slotType;

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

		public virtual void InitiateDelete() { }

		public virtual void Initialise(UIManager uiManager, Cruiser parentCruiser, Cruiser enemyCruiser, BuildableFactory buildingFactory)
		{
			_uiManager = uiManager;
			_parentCruiser = parentCruiser;
			_enemyCruiser = enemyCruiser;
		}

		// For copying private members, and non-MonoBehaviour or primitive types (eg: ITurretStats).
		public virtual void Initialise(BuildableObject buildable)
		{
			_uiManager = buildable._uiManager;
			_parentCruiser = buildable._parentCruiser;
			_enemyCruiser = buildable._enemyCruiser;
		}

		void Awake()
		{
			Debug.Log("BuildableObject.Awake()");
			_renderer = GetComponent<Renderer>();
		}
	}
}
