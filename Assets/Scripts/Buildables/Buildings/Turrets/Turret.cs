using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public abstract class Turret : Building, ITargetConsumer
	{
		private GameObject _turretBase;
		private Renderer _turretBaseRenderer;
		private GameObject _turretBarrel;
		private Renderer _turretBarrelRenderer;
		protected BarrelController _barrelController;

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
			
			_barrelController = gameObject.GetComponentInChildren<BarrelController>();
			Assert.IsNotNull(_barrelController);
			_barrelController.StaticInitialise();
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();
			InitialiseTurretBarrel();
		}

		protected virtual void InitialiseTurretBarrel()
		{
			_barrelController.Initialise(CreateTargetFilter(), CreateAngleCalculator(), CreateRotationMovementController());
		}

		protected virtual ITargetFilter CreateTargetFilter()
		{
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            return _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
		}

		protected abstract IAngleCalculator CreateAngleCalculator();

		protected virtual IRotationMovementController CreateRotationMovementController()
		{
			return _movementControllerFactory.CreateRotationMovementController(_barrelController.TurretStats.turretRotateSpeedInDegrees, _barrelController.transform);
		}

		protected override void EnableRenderers(bool enabled)
		{
			_turretBaseRenderer.enabled = enabled;
			_turretBarrelRenderer.enabled = enabled;
		}
	}
}
