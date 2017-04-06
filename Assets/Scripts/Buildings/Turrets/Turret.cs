using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Units;
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
		private Vector2 _shellVelocity;
		private float _timeSinceLastFireInS;
		private ITurretStats _turretStats;
		private ShellStats _shellStats;

		private Renderer _turretBaseRenderer;
		private Renderer _turretBarrelRenderer;

		public GameObject turretBase;
		public TurretBarrelController turretBarrelController;
		public GameObject turretBarrel;
		// FELIX  Allow to vary depending on artillery?
		public Rigidbody2D shellPrefab;
		public ShellSpawnerController shellSpawner;

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
					if (_turretStats == null)
					{
						throw new InvalidOperationException();
					}

					if (_target != null)
					{
						turretBarrelController.OnTarget -= OnTarget;
					}

					turretBarrelController.StartTrackingTarget(_target, _turretStats.BulletVelocityInMPerS);
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

		public override float Damage { get { return _turretStats.DamangePerS; } }

		protected override void OnAwake()
		{
			base.OnAwake();
		
			_turretBaseRenderer = turretBase.GetComponent<Renderer>();
			_turretBarrelRenderer = turretBarrel.GetComponent<Renderer>();

			_timeSinceLastFireInS = float.MaxValue;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			_timeSinceLastFireInS += Time.deltaTime;
		}

		public override void Initialise(BattleCruisers.UI.UIManager uiManager, Cruiser parentCruiser, Cruiser enemyCruiser, BuildableFactory buildableFactory, IDroneManager droneManager)
		{
			base.Initialise(uiManager, parentCruiser, enemyCruiser, buildableFactory, droneManager);
			_turretStats = buildableFactory.GetTurretStats(buildableName);
			_shellStats = new ShellStats(shellPrefab, _turretStats.Damage, _turretStats.IgnoreGravity, _turretStats.BulletVelocityInMPerS);
			shellSpawner.Initialise(_shellStats);
		}
		
		public override void Initialise(Buildable buildable)
		{
			base.Initialise(buildable);

			Turret turret = buildable as Turret;
			Assert.IsNotNull(turret);
			_turretStats = turret._turretStats;
			_shellStats = turret._shellStats;
			shellSpawner.Initialise(_shellStats);
		}

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
			Direction fireDirection = _target.transform.position.x > transform.position.x ? Direction.Right : Direction.Left;
			shellSpawner.SpawnShell(angleInRadians, fireDirection);
		}

		protected override void OnBuildingCompleted()
		{
			base.OnBuildingCompleted();

			if (category == BuildingCategory.Offence)
			{
				Target = _enemyCruiser.gameObject;
			}
		}

		protected override void EnableRenderers(bool enabled)
		{
			_turretBaseRenderer.enabled = enabled;
			_turretBarrelRenderer.enabled = enabled;
		}
	}
}
