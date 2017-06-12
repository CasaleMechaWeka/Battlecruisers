using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public abstract class Turret : Building, ITargetConsumer
	{
		private GameObject _turretBase;
		private Renderer _turretBaseRenderer;
		private GameObject _turretBarrel;
		private Renderer _turretBarrelRenderer;
		protected TurretBarrelController _barrelController;

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
			get { return _barrelController.Target; }
			set { _barrelController.Target = value; }
		}

		public override Sprite Sprite
		{
			get
			{
				if (_sprite == null)
				{
					_sprite = _buildableProgress.FillableImageSprite;
				}
				return _sprite;
			}
		}

		public override float Damage { get { return _barrelController.TurretStats.DamagePerS; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_turretBase = transform.Find("Base").gameObject;
			_turretBaseRenderer = _turretBase.GetComponent<Renderer>();
			Assert.IsNotNull(_turretBaseRenderer);
			
			_turretBarrel = transform.Find("BarrelWrapper/Barrel").gameObject;
			_turretBarrelRenderer = _turretBarrel.GetComponent<Renderer>();
			Assert.IsNotNull(_turretBarrelRenderer);
			
			_barrelController = gameObject.GetComponentInChildren<TurretBarrelController>();
			Assert.IsNotNull(_barrelController);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();
			InitialiseTurretBarrel();
		}

		protected virtual void InitialiseTurretBarrel()
		{
			_barrelController.Initialise(CreateTargetFilter(), CreateAngleCalculator());
		}

		protected virtual ITargetFilter CreateTargetFilter()
		{
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			return _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
		}

		protected abstract IAngleCalculator CreateAngleCalculator();

		protected override void EnableRenderers(bool enabled)
		{
			_turretBaseRenderer.enabled = enabled;
			_turretBarrelRenderer.enabled = enabled;
		}
	}
}
