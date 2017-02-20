using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : MonoBehaviour, IArtillery
{
	public GameObject Target { set; private get; }
	public ITurretStats TurretStats { set; private get; }
	public Rigidbody2D ShellPrefab { set; private get; }
	public Vector2 ShellOrigin { set; private get; }

	// FELIX  TEMP
	public Rigidbody2D shellPrefab;
	public Vector2 initialVelocity;

	public float spawnTime;		// The amount of time between each spawn.
	public float spawnDelay;		// The amount of time before spawning starts.

	private const float GRAVITY = 9.8f;  // m/s^2
	private const float PI = 3.14f;

	void Start()
	{
		Debug.Log("Artillery.Start()");

		Debug.Log($"spawnDelay: {spawnDelay}  spawnTime: {spawnTime}");
		InvokeRepeating("Spawn", spawnDelay, spawnTime);
	}

	void Spawn()
	{
		Debug.Log("Spawn()");

		// FELIX  Test for shooting both from left to right, and vice versa
		float velocityMagnitude = 20;
		float distance = 40;  // m
//		float distance = -20 -20;  // m
		float angleInRadians = CalculateAngleLaunch(velocityMagnitude, distance);
		float angleInDegrees = angleInRadians * 180 / PI;

		Debug.Log($"angle: {angleInDegrees}");

		Rigidbody2D shell = Instantiate(shellPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
//		Rigidbody2D shell = Instantiate(shellPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angleInDegrees))) as Rigidbody2D;

		float velocityX = (float)(velocityMagnitude * Math.Cos(angleInRadians));
		float velocityY = (float)(velocityMagnitude * Math.Sin(angleInRadians));

		shell.velocity = new Vector2(velocityX, velocityY);
	}

	// FELIX  Move to separate class
	// FELIX  Assumes no y axis change
	private float CalculateAngleLaunch(float velocityInMPerS, float distanceInM)
	{
		Debug.Log($"CalculateAngleLaunch() velocityInMPerS: {velocityInMPerS}  distanceInM: {distanceInM}");
		return (float) (0.5 * Math.Asin(GRAVITY * distanceInM / (velocityInMPerS * velocityInMPerS)));


//		bool isXVelocityPositive = target.x > source.x;
//		float distance = Math.Abs(target.x - source.x);
//
//		float xVelocity = distance / (timeInMs * 1000);
//		float yVelocity = 
	}
}
