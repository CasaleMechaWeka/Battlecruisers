using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FELIX  Refactor, create Artillery class?
// FELIX  Allow speed up of fire rate (for when engineers are helping?)
public class Cruiser : MonoBehaviour 
{
	public Artillery artilleryPrefab;
	public GameObject otherCruiser;

	void Start ()
	{
		Vector2 position = transform.position;
		position.y += 2;

		IArtillery artillery = Instantiate(artilleryPrefab, position, Quaternion.Euler(new Vector3(0,0,0)));
		// FELIX  Don't hardcode
		ITurretStats turretStats = new TurretStats(1, 1, 20, 24);
		artillery.TurretStats = turretStats;
		artillery.Target = otherCruiser;
	}

	public void TakeDamage()
	{
		Debug.Log("Ich bin schwer verwundet, und kann mich nicht bewegen!");
	}
}
