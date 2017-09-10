using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets
{
    public class MissileBarrelTestGod : MonoBehaviour 
    {
	    void Start() 
        {
            Helper helper = new Helper();


            // Setup target
            AirFactory target = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(target, Faction.Reds);
            target.StartConstruction();


            // Setup missile barrel controller
            IList<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(Faction.Reds, targetTypes);
            IAngleCalculator angleCalculator = new StaticAngleCalculator(targetPositionPredictorFactory: null, desiredAngleInDegrees: 90);
            IRotationMovementController rotationMovementController = new DummyRotationMovementController();
            IAngleCalculatorFactory angleCalculatorFactory = new AngleCalculatorFactory();
            ITargetPositionPredictorFactory targetPositionPredictorFactory = new TargetPositionPredictorFactory();
            IMovementControllerFactory movementControllerFactory = new MovementControllerFactory(angleCalculatorFactory, targetPositionPredictorFactory);

            MissileBarrelController missileBarrel = FindObjectOfType<MissileBarrelController>();
            missileBarrel.StaticInitialise();
            missileBarrel.Initialise(targetFilter, angleCalculator, rotationMovementController, movementControllerFactory, targetPositionPredictorFactory);
            missileBarrel.Target = target;
	    }
	}
}
