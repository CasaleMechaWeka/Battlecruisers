using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets
{
    public class LaserTurretTestGod : MonoBehaviour 
    {
	    void Start () 
        {
            // Setup laser barrel
            LaserBarrelController laserBarrel = FindObjectOfType<LaserBarrelController>();
            laserBarrel.StaticInitialise();

            ITargetFilter targetFilter = new DummyTargetFilter(isMatchResult: true);
            ITargetPositionPredictorFactory targetPositionPredictorFactory = new TargetPositionPredictorFactory();
            IAngleCalculator angleCalculator = new AngleCalculator(targetPositionPredictorFactory);
            IRotationMovementController rotationMovementController = new DummyRotationMovementController(isOnTarget: true);
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(new Helper());

            laserBarrel.Initialise(targetFilter, angleCalculator, rotationMovementController, args.FactoryProvider);
            

            // Setup target
            AirFactory airFactory = FindObjectOfType<AirFactory>();
            new Helper().InitialiseBuilding(airFactory);
            airFactory.Destroyed += (sender, e) => laserBarrel.Target = null;
			laserBarrel.Target = airFactory;
	    }
	}
}
