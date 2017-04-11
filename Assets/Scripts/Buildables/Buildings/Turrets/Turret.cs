using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Detectors;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class Turret : Building
	{
		private Vector2 _shellVelocity;
		private float _timeSinceLastFireInS;
		private ShellStats _shellStats;

		private Renderer _turretBaseRenderer;
		private Renderer _turretBarrelRenderer;

		public GameObject turretBase;
		public TurretBarrelController turretBarrelController;
		public GameObject turretBarrel;
		// FELIX  Allow to vary depending on artillery?
		public Rigidbody2D shellPrefab;
		public ShellSpawnerController shellSpawner;
		public TurretStats turretStats;

		protected override Renderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					_renderer = turretBase.GetComponent<Renderer>();
				}
				return _renderer;
			}
		}

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
					turretBarrelController.StartTrackingTarget(_target, turretStats.bulletVelocityInMPerS);
					turretBarrelController.OnTarget += OnTarget;
				}
			}
		}

		public override Sprite Sprite
		{
			get
			{
				if (_sprite == null)
				{
					_sprite = turretBarrel.GetComponent<SpriteRenderer>().sprite;
				}
				return _sprite;
			}
		}

		public override float Damage { get { return turretStats.DamagePerS; } }

		protected override void OnAwake()
		{
			base.OnAwake();
		
			_turretBaseRenderer = turretBase.GetComponent<Renderer>();
			_turretBarrelRenderer = turretBarrel.GetComponent<Renderer>();

			_timeSinceLastFireInS = float.MaxValue;
			_shellStats = new ShellStats(shellPrefab, turretStats.damage, turretStats.ignoreGravity, turretStats.bulletVelocityInMPerS);
			shellSpawner.Initialise(_shellStats);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			_timeSinceLastFireInS += Time.deltaTime;
		}

		private void OnTarget(object sender, EventArgs e)
		{
			if (_timeSinceLastFireInS >= turretStats.FireIntervalInS)
			{
				Fire(turretBarrelController.DesiredAngleInRadians);
				_timeSinceLastFireInS = 0;
			}
		}

		private void Fire(float angleInRadians)
		{
			Direction fireDirection = _target.transform.position.x > transform.position.x ? Direction.Right : Direction.Left;
			shellSpawner.SpawnShell(angleInRadians, fireDirection);
		}

		protected override void EnableRenderers(bool enabled)
		{
			_turretBaseRenderer.enabled = enabled;
			_turretBarrelRenderer.enabled = enabled;
		}
	}
}
