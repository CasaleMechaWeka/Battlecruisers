using BattleCruisers.Cruisers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildings.Turrets
{
	public class Turret : Building
	{
		private Renderer _turretBaseRenderer;
//		private 

		public GameObject turretBase;
		public GameObject turretBarrelWrapper;
		public GameObject turretBarrel;
		public ProjectileSpawner projectileSpawner;

		private const float ROTATE_SPEED_IN_DEGREES_PER_S = 3;

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

		private GameObject _target;
		public GameObject Target
		{ 
			protected get { return _target; }
			set
			{
				_target = value;
				projectileSpawner.Target = _target;
			}
		}

		private ITurretStats _turretStats;
		public ITurretStats TurretStats
		{
			private get { return _turretStats; }
			set
			{
				_turretStats = value;
				projectileSpawner.TurretStats = _turretStats;
			}
		}

		void Awake()
		{
			Debug.Log("Turret.Awake()");
			_turretBaseRenderer = turretBase.GetComponent<Renderer>();
		}

		void Update()
		{
//			RotateTurret();
		}

		private void RotateTurret()
		{
			turretBarrelWrapper.transform.Rotate(Vector3.forward * Time.deltaTime * ROTATE_SPEED_IN_DEGREES_PER_S);
			Debug.Log(turretBarrelWrapper.transform.rotation.z);
		}
	}
}
