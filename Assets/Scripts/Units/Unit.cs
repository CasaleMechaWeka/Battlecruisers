using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Units
{
	public enum UnitCategory
	{
		Naval, Aircraft, Ultra
	}

	public enum Direction
	{
		Left, Right, Up, Down
	}

	public class Unit : BuildableObject
	{
		private Renderer _renderer;

		public UnitCategory category;
		// FELIX  Remove?
		public Direction facingDirection;

		// FELIX  Only for ships!
		public float velocityInMPerS;

		public virtual Vector3 Size 
		{ 
			get 
			{ 
				return _renderer.bounds.size; 
			} 
		}

		protected Sprite _unitSprite;
		public virtual Sprite UnitSprite
		{
			get
			{
				if (_unitSprite == null)
				{
					_unitSprite = GetComponent<SpriteRenderer>().sprite;
				}
				return _unitSprite;
			}
		}

		void Awake()
		{
			Debug.Log("Unit.Awake()");
			_renderer = GetComponent<Renderer>();
		}
	}
}
