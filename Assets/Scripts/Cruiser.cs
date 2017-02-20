using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FELIX  Refactor, create Artillery class?
// FELIX  Allow speed up of fire rate (for when engineers are helping?)
public class Cruiser : MonoBehaviour 
{
	public Rigidbody2D shellPrefab;
	public Vector2 initialVelocity;

	public float spawnTime = 3f;		// The amount of time between each spawn.
	public float spawnDelay = 1f;		// The amount of time before spawning starts.


	void Start ()
	{
		InvokeRepeating("Spawn", spawnDelay, spawnTime);
	}

	private void Spawn ()
	{
		Rigidbody2D shell = Instantiate(shellPrefab, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
		shell.velocity = initialVelocity;
	}
}
