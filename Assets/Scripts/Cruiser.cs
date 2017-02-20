using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FELIX  Refactor, create Artillery class?
// FELIX  Allow speed up of fire rate (for when engineers are helping?)
public class Cruiser : MonoBehaviour 
{
	public Rigidbody2D artilleryPrefab;

	// FELIX  TEMP
	private Rigidbody2D _artillery;

	void Start ()
	{
		Vector2 position = transform.position;
		position.x += 2;
		position.y += 2;
		_artillery = Instantiate(artilleryPrefab, position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
	}
}
