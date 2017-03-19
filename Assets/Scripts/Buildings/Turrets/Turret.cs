using BattleCruisers.Cruisers;
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
		private Renderer _turretBaseRenderer;
		private Vector2 _shellVelocity;
		private float _timeSinceLastFireInS;
		private ITurretStats _turretStats;
		private ShellStats _shellStats;
		// FELIX  TEMP
		private Cruiser _enemyCruiser;

		public GameObject turretBase;
		public TurretBarrelController turretBarrelController;
		public GameObject turretBarrel;
		// FELIX  Allow to vary depending on artillery?
		public Rigidbody2D shellPrefab;
		public ShellSpawnerController shellSpawner;


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

		public override Vector3 Size 
		{ 
			get 
			{ 
				return _turretBaseRenderer.bounds.size;
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

		void Awake()
		{
			Debug.Log("Turret.Awake()");
			_turretBaseRenderer = turretBase.GetComponent<Renderer>();
			_timeSinceLastFireInS = float.MaxValue;

		}

		void Start()
		{
			if (category == BuildingCategory.Offence)
			{
				Target = _enemyCruiser.gameObject;
			}
		}

		void Update()
		{
			_timeSinceLastFireInS += Time.deltaTime;
		}

		public override void Initialise(BattleCruisers.UI.UIManager uiManager, Cruiser parentCruiser, Cruiser enemyCruiser, BuildingFactory buildingFactory)
		{
			base.Initialise(uiManager, parentCruiser, enemyCruiser, buildingFactory);
			_turretStats = buildingFactory.GetTurretStats(buildableName);
			_shellStats = new ShellStats(shellPrefab, _turretStats.Damage, _turretStats.IgnoreGravity, _turretStats.BulletVelocityInMPerS);
			shellSpawner.Initialise(_shellStats);
			this._enemyCruiser = enemyCruiser;
		}
		
		public override void Initialise(Building building)
		{
			base.Initialise(building);

			Turret turret = building as Turret;
			Assert.IsNotNull(turret);
			_turretStats = turret._turretStats;
			_shellStats = turret._shellStats;
			shellSpawner.Initialise(_shellStats);
			_enemyCruiser = turret._enemyCruiser;
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
	}
}
