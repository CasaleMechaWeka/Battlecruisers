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
		private GameObject _turretBase;
		private Renderer _turretBaseRenderer;
		private GameObject _turretBarrel;
		private Renderer _turretBarrelRenderer;
		protected TurretBarrelController _turretBarrelController;

		protected override Renderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					_renderer = _turretBase.GetComponent<Renderer>();
				}
				return _renderer;
			}
		}

		public ITarget Target 
		{ 
			get { return _turretBarrelController.Target; }
			set { _turretBarrelController.Target = value; }
		}

		public override Sprite Sprite
		{
			get
			{
				if (_sprite == null)
				{
					_sprite = _buildableProgress.fillableImage.sprite;
				}
				return _sprite;
			}
		}

		public override float Damage { get { return _turretBarrelController.turretStats.DamagePerS; } }

		protected override void OnAwake()
		{
			base.OnAwake();
		
			_turretBase = transform.Find("Base").gameObject;
			_turretBaseRenderer = _turretBase.GetComponent<Renderer>();
			Assert.IsNotNull(_turretBaseRenderer);

			_turretBarrel = transform.Find("BarrelWrapper/Barrel").gameObject;
			_turretBarrelRenderer = _turretBarrel.GetComponent<Renderer>();
			Assert.IsNotNull(_turretBarrelRenderer);

			_turretBarrelController = gameObject.GetComponentInChildren<TurretBarrelController>();
			Assert.IsNotNull(_turretBarrelController);
		}

		protected override void EnableRenderers(bool enabled)
		{
			_turretBaseRenderer.enabled = enabled;
			_turretBarrelRenderer.enabled = enabled;
		}
	}
}
