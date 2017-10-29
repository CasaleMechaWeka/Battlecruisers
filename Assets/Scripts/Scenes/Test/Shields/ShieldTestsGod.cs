using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Shields
{
    public class ShieldTestsGod : MonoBehaviour 
	{
		void Start () 
		{
            // Setup shield
            ShieldStats shieldStats = FindObjectOfType<ShieldStats>();
            shieldStats.BoostMultiplier = 1;
            ShieldController shield = FindObjectOfType<ShieldController>();
            shield.StaticInitialise();
			shield.Initialise(Faction.Reds);


            // Setup turret
            BarrelController turret = FindObjectOfType<BarrelController>();
            turret.StaticInitialise();

            IList<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(shield.Faction, targetTypes);
            ITargetPositionPredictor targetPositionPredictor = new DummyTargetPositionpredictor();
			IAngleCalculator angleCalculator = new AngleCalculator(new TargetPositionPredictorFactory());
			IRotationMovementController rotationMovementController = new RotationMovementController(angleCalculator, turret.TurretStats.TurretRotateSpeedInDegrees, turret.transform);
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(new Helper());

            turret.Initialise(targetFilter, targetPositionPredictor, angleCalculator, rotationMovementController, args.FactoryProvider);
			turret.Target = shield;
		}
	}
}
