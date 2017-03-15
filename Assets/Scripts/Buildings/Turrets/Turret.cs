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

		void Awake()
		{
			Debug.Log("Turret.Awake()");
			_turretBaseRenderer = turretBase.GetComponent<Renderer>();
		}

		void Update()
		{
			RotateTurret();
		}

		private void RotateTurret()
		{
			turretBarrelWrapper.transform.Rotate(Vector3.forward * Time.deltaTime * ROTATE_SPEED_IN_DEGREES_PER_S);
			Debug.Log(turretBarrelWrapper.transform.rotation.z);
		}
	}
}
