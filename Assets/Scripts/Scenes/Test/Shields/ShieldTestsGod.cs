using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Shields
{
	public class ShieldTestsGod : MonoBehaviour 
	{
		public ShieldController shield;
		public BarrelController turret;

		void Start () 
		{
			shield.Initialise(Faction.Reds);

			ITargetFilter targetFilter = new FactionAndTargetTypeFilter(shield.Faction, TargetType.Buildings);
			IAngleCalculator angleCalculator = new AngleCalculator(new TargetPositionPredictorFactory());
			IRotationMovementController rotationMovementController = new RotationMovementController(angleCalculator, turret.TurretStats.turretRotateSpeedInDegrees, turret.transform);
			turret.Initialise(targetFilter, angleCalculator, rotationMovementController);
			turret.Target = shield;
		}
	}
}
