using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class Turret : Building, ITargetConsumer
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

		public ITarget Target 
		{ 
			get { return turretBarrelController.Target; }
			set { turretBarrelController.Target = value; }
		}

		public override Sprite Sprite
		{
			get
			{
				if (_sprite == null)
				{
					_sprite = buildableProgress.fillableImage.sprite;
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
