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
	private float _maxRange;

	private const float GRAVITY = 9.8f;  // m/s^2
	private const float PI = 3.14f;

	private GameObject _target;
	public GameObject Target 
	{ 
		set
		{
			_target = value;

			_maxRange = FindMaxRange(TurretStats.BulletVelocityInMPerS);

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

		if (TurretStats == null)
		{
			throw new InvalidOperationException();
		}

		float distance = transform.position.x - Target.transform.position.x;
		float xMultipler = -1;

		if (distance < 0)
		{
			distance *= -1;
			xMultipler = 1;
		}

		if (distance > _maxRange)
		{
			throw new InvalidOperationException();
		}

		float angleInRadians = FindAngleLaunch(TurretStats.BulletVelocityInMPerS, distance);
		Debug.Log($"angle: {angleInRadians}");

		Rigidbody2D shell = Instantiate(shellPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
		shell.GetComponent<IBulletController>().Damage = TurretStats.Damage;

		float velocityX = (float)(TurretStats.BulletVelocityInMPerS * Math.Cos(angleInRadians)) * xMultipler;
		float velocityY = (float)(TurretStats.BulletVelocityInMPerS * Math.Sin(angleInRadians));

		Debug.Log($"velocityX: {velocityX}  velocityY: {velocityY}");

		shell.velocity = new Vector2(velocityX, velocityY);
	}

	/// <summary>
	/// Assumes no y axis difference in source and target
	/// </summary>
	private float FindAngleLaunch(float velocityInMPerS, float distanceInM)
	{
		Debug.Log($"CalculateAngleLaunch() velocityInMPerS: {velocityInMPerS}  distanceInM: {distanceInM}");
		return (float) (0.5 * Math.Asin(GRAVITY * distanceInM / (velocityInMPerS * velocityInMPerS)));
	}

	/// <summary>
	/// Assumes no y axis difference in source and target
	/// </summary>
	private float FindMaxRange(float velocityInMPerS)
	{
		return (velocityInMPerS * velocityInMPerS) / GRAVITY;
	}
}
