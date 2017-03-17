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
		private ITurretStats _turretStats;
		private float _maxRange;
		private Vector2 _shellVelocity;
		private float _timeSinceLastFireInS;

		public GameObject turretBase;
		public TurretBarrelController turretBarrelController;
		public GameObject turretBarrel;
		public GameObject projectileSpawner;
		// FELIX  Allow to vary depending on artillery?
		public Rigidbody2D shellPrefab;

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
					if (_turretStats == null)
					{
						throw new InvalidOperationException();
					}

					_maxRange = FindMaxRange(_turretStats.BulletVelocityInMPerS);

					if (_target != null)
					{
						turretBarrelController.OnTarget -= OnTarget;
					}

					turretBarrelController.StartTrackingTarget(_target, _turretStats.BulletVelocityInMPerS);
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

		public override void Initialise(BattleCruisers.UI.UIManager uiManager, Cruiser parentCruiser, Cruiser enemyCruiser, BuildingFactory buildingFactory)
		{
			base.Initialise(uiManager, parentCruiser, enemyCruiser, buildingFactory);
			_turretStats = buildingFactory.GetTurretStats(buildingName);
		}

		// FELIX  Limit fire rate :P
		private void OnTarget(object sender, EventArgs e)
		{
			if (_timeSinceLastFireInS >= _turretStats.FireIntervalInS)
			{
				Fire(turretBarrelController.DesiredAngleInRadians);
				_timeSinceLastFireInS = 0;
			}
		}

		private void Fire(float angleInRadians)
		{
//			Debug.Log("Turret.Fire()");

			Rigidbody2D shell = Instantiate(shellPrefab, projectileSpawner.transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
			if (_turretStats.IgnoreGravity)
			{
				shell.gravityScale = 0;
			}
			shell.GetComponent<IShellController>().Damage = _turretStats.Damage;
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
			float velocityX = (float)(_turretStats.BulletVelocityInMPerS * Math.Cos(angleInRadians)) * xMultipler;
			float velocityY = (float)(_turretStats.BulletVelocityInMPerS * Math.Sin(angleInRadians));

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
