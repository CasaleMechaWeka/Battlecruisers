using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class AttackBoatController : ShipController
	{
		private ShellTurretBarrelController _turretBarrelController;

        public override float Damage { get { return _turretBarrelController.TurretStats.DamagePerS; } }
		public override TargetType TargetType { get { return TargetType.Ships; } }
        protected override float EnemyDetectionRangeInM { get { return _turretBarrelController.TurretStats.rangeInM; } }

        public override void StaticInitialise()
		{
			base.StaticInitialise();

            _turretBarrelController = gameObject.GetComponentInChildren<ShellTurretBarrelController>();
			Assert.IsNotNull(_turretBarrelController);
			_turretBarrelController.StaticInitialise();
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();
			
			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            ITargetFilter turretBarrelFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
			IRotationMovementController rotationMovementController = _movementControllerFactory.CreateRotationMovementController(_turretBarrelController.TurretStats.turretRotateSpeedInDegrees, _turretBarrelController.transform);
			_turretBarrelController.Initialise(turretBarrelFilter, angleCalculator, rotationMovementController);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

            _enemyStoppingTargetProcessor.AddTargetConsumer(_turretBarrelController);
		}
	}
}
