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
		private Renderer _turretBaseRenderer;
		private Renderer _turretBarrelRenderer;

		public GameObject turretBase;
		public GameObject turretBarrel;
		public TurretBarrelController turretBarrelController;

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

		public GameObject Target 
		{ 
			set
			{
				turretBarrelController.Target = value;
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

		public override float Damage { get { return turretBarrelController.turretStats.DamagePerS; } }

		protected override void OnAwake()
		{
			base.OnAwake();
		
			_turretBaseRenderer = turretBase.GetComponent<Renderer>();
			_turretBarrelRenderer = turretBarrel.GetComponent<Renderer>();
		}

		protected override void EnableRenderers(bool enabled)
		{
			_turretBaseRenderer.enabled = enabled;
			_turretBarrelRenderer.enabled = enabled;
		}
	}
}
