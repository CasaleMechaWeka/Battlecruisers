using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
	Friend, Enemy
}

public enum Direction
{
	Left, Right
}

public interface IUnit
{
	UnitType Type { get; }
	Direction FacingDirection { get; }
	float Health { get; }
	bool IsDestroyed { get; }
}

public class Unit : MonoBehaviour, IUnit
{
	public UnitType Type { get; protected set; }
	public Direction FacingDirection { get; protected set; }
	public float Health { get; protected set; }
	public bool IsDestroyed { get { return Health <= 0; } }
}
