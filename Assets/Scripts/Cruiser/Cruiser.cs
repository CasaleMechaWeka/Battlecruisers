using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FELIX  Refactor, create Artillery class?
// FELIX  Allow speed up of fire rate (for when engineers are helping?)
public class Cruiser : MonoBehaviour, ICruiser
{
	public Artillery artilleryPrefab;
	public GameObject otherCruiser;

	public Factory factoryPrefab;
	// FELIX  Should not be part of factory logic
	public AttackBoatController boatPrefab;

	// FELIX  TEMP  Only create buildings when user builds them
	void Start ()
	{
//		// Artillery
//		Vector2 position = transform.position;
//		position.y += 2;
//
//		IArtillery artillery = Instantiate(artilleryPrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)));
//		// FELIX  Don't hardcode
//		ITurretStats turretStats = new TurretStats(1, 1, 20, 24, ignoreGravity: false);
//		artillery.TurretStats = turretStats;
//		artillery.Target = otherCruiser;

		// Naval factory
		boatPrefab.BuildTimeInS = 3;
		boatPrefab.VelocityInMPerS = 4;
		// FELIX
		boatPrefab.TurretStats = null;

		Vector2 factoryPosition = transform.position;
		factoryPosition.x += 5;
		IFactory factory = Instantiate(factoryPrefab, factoryPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
		factory.BuildPoints = 1;
		factory.Unit = boatPrefab;
	}

	public void TakeDamage(float damage)
	{
		Debug.Log("Ich bin schwer verwundet, und kann mich nicht bewegen! " + damage);
	}
}
