using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Units
{
	public enum Faction
	{
		Blues, Reds
	}

	public enum UnitCategory
	{
		Naval, Aircraft, Ultra
	}

	public enum Direction
	{
		Left, Right, Up, Down
	}

	public class Unit : MonoBehaviour
	{
		public string unitName;
		public string description;
		public int numOfDronesRequired;
		public int buildTimeInS;
		public UnitCategory category;
		public float health;
		public Faction faction;
		// FELIX  Remove?
		public Direction facingDirection;

		public bool IsDestroyed { get { return health <= 0; } }
	}
}
