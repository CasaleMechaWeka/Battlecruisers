using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : MonoBehaviour, IArtillery
{
	// FELIX  Allow to vary depending on artillery?
	public Rigidbody2D shellPrefab;

	// FELIX  Change if artillery has to point towards enemy first
	private float _fireDelayInS = 1f; // The amount of time before Fireing starts.

	private const float GRAVITY = 9.8f;  // m/s^2
	private const float PI = 3.14f;

	private GameObject _target;
	public GameObject Target 
	{ 
		set
		{
			_target = value;
			if (_target == null)
			{
				StopFiring();
			}
			else
			{
				StartFiring();
			}
		}
		get { return _target; }
	}

	public ITurretStats TurretStats { set; private get; }
	public Rigidbody2D ShellPrefab { set; private get; }
	// FELIX:  use or remove
	public Vector2 ShellOrigin { set; private get; }

	private void StartFiring()
	{
		if (TurretStats == null)
		{
			throw new InvalidOperationException();
		}

		float fireIntervalInS = 1 / TurretStats.FireRatePerS;
		InvokeRepeating("Fire", _fireDelayInS, fireIntervalInS);
	}

	private void StopFiring()
	{
		CancelInvoke("Fire");
	}

	private void Fire()
	{
		Debug.Log("Fire()");

		if (TurretStats == null || ShellOrigin == null)
		{
			throw new InvalidOperationException();
		}

		// FELIX  Test for shooting both from left to right, and vice versa
		float distance = transform.position.x - Target.transform.position.x;
		float angleInRadians = CalculateAngleLaunch(TurretStats.BulletVelocityInMPerS, distance);

		Debug.Log($"angle: {angleInRadians}");

		float zRotation = 0;
		if (angleInRadians < 0)
		{
			angleInRadians *= -1;
			zRotation = 180;
		}

		Rigidbody2D shell = Instantiate(shellPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, zRotation))) as Rigidbody2D;

		float velocityX = (float)(TurretStats.BulletVelocityInMPerS * Math.Cos(angleInRadians));
		float velocityY = (float)(TurretStats.BulletVelocityInMPerS * Math.Sin(angleInRadians));

		shell.velocity = new Vector2(velocityX, velocityY);
	}

	/// <summary>
	/// Assumes no y axis difference in source and target
	/// </summary>
	private float CalculateAngleLaunch(float velocityInMPerS, float distanceInM)
	{
		Debug.Log($"CalculateAngleLaunch() velocityInMPerS: {velocityInMPerS}  distanceInM: {distanceInM}");
		return (float) (0.5 * Math.Asin(GRAVITY * distanceInM / (velocityInMPerS * velocityInMPerS)));
	}
}
