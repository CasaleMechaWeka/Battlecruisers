using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
	public class FrigateController : ShipController
	{
        // FELIX
		public override float Damage { get { return 0; } }
		// FELIX
		protected override float EnemyDetectionRangeInM { get { return 5; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

            _attackCapabilities.Add(TargetType.Aircraft);

			// FELIX  Retrieve all turrets :D
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			// FELIX  Initialise all turrets :D

			//IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
			//Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			//ITargetFilter turretBarrelFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
			//IRotationMovementController rotationMovementController = _movementControllerFactory.CreateRotationMovementController(_turretBarrelController.TurretStats.turretRotateSpeedInDegrees, _turretBarrelController.transform);
			//_turretBarrelController.Initialise(turretBarrelFilter, angleCalculator, rotationMovementController);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			// FELIX  Turrets :D
			//_enemyStoppingTargetProcessor.AddTargetConsumer(_turretBarrelController);
		}
	}
}
