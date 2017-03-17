using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace BattleCruisers.Buildings.Turrets
{
	public class Turret : Building
	{
		private Renderer _turretBaseRenderer;
		private float _maxRange;
		private Vector2 _shellVelocity;
		private float _timeSinceLastFireInS;

		public GameObject turretBase;
		public TurretBarrelController turretBarrelController;
		public GameObject turretBarrel;
		public GameObject projectileSpawner;
		// FELIX  Allow to vary depending on artillery?
		public Rigidbody2D shellPrefab;

		public ITurretStats TurretStats { private get; set; }

		private GameObject _target;
		public GameObject Target 
		{ 
			get { return _target; }
			set
			{
				_target = value;

				if (_target == null)
				{
					turretBarrelController.StopTrackingTarget();
					turretBarrelController.OnTarget -= OnTarget;
				}
				else
				{
					if (TurretStats == null)
					{
						throw new InvalidOperationException();
					}

					_maxRange = FindMaxRange(TurretStats.BulletVelocityInMPerS);

					if (_target != null)
					{
						turretBarrelController.OnTarget -= OnTarget;
					}

					turretBarrelController.StartTrackingTarget(_target, TurretStats.BulletVelocityInMPerS);
					turretBarrelController.OnTarget += OnTarget;
				}
			}
		}

		public override Vector3 Size 
		{ 
			get 
			{ 
				return _turretBaseRenderer.bounds.size;
			} 
		}

		public override Sprite BuildingSprite
		{
			get
			{
				if (_buidlingSprite == null)
				{
					_buidlingSprite = turretBarrel.GetComponent<SpriteRenderer>().sprite;
				}
				return _buidlingSprite;
			}
		}

		void Awake()
		{
			Debug.Log("Turret.Awake()");
			_turretBaseRenderer = turretBase.GetComponent<Renderer>();
			_timeSinceLastFireInS = float.MaxValue;
		}

		void Update()
		{
			_timeSinceLastFireInS += Time.deltaTime;
		}

		// FELIX  Limit fire rate :P
		private void OnTarget(object sender, EventArgs e)
		{
			if (_timeSinceLastFireInS >= TurretStats.FireIntervalInS)
			{
				Fire(turretBarrelController.DesiredAngleInRadians);
				_timeSinceLastFireInS = 0;
			}
		}

		private void Fire(float angleInRadians)
		{
//			Debug.Log("Turret.Fire()");

			Rigidbody2D shell = Instantiate(shellPrefab, projectileSpawner.transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
			if (TurretStats.IgnoreGravity)
			{
				shell.gravityScale = 0;
			}
			shell.GetComponent<IShellController>().Damage = TurretStats.Damage;
			shell.velocity = FindShellVelocity(turretBarrelController.DesiredAngleInRadians);
		}

		private Vector2 FindShellVelocity(float angleInRadians)
		{
			float distance = Math.Abs(transform.position.x - _target.transform.position.x);

			if (distance > _maxRange)
			{
				throw new InvalidOperationException();
			}

			float xMultipler = distance < 0 ? -1 : 1;
			float velocityX = (float)(TurretStats.BulletVelocityInMPerS * Math.Cos(angleInRadians)) * xMultipler;
			float velocityY = (float)(TurretStats.BulletVelocityInMPerS * Math.Sin(angleInRadians));

//			Debug.Log($"Turret.FindShellVelocity():  angleInRadians: {angleInRadians}  velocityX: {velocityX}  velocityY: {velocityY}");

			return new Vector2(velocityX, velocityY);
		}

		/// <summary>
		/// Assumes no y axis difference in source and target
		/// </summary>
		private float FindMaxRange(float velocityInMPerS)
		{
			return (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;
		}
	}
}
