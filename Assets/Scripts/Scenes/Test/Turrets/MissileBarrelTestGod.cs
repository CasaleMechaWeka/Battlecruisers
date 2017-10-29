using System.Collections.Generic;
using BattleCruisers.Buildables;
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
            ITargetPositionPredictor targetPositionPredictor = new DummyTargetPositionpredictor();
            IAngleCalculator angleCalculator = new StaticAngleCalculator(desiredAngleInDegrees: 90);
            IRotationMovementController rotationMovementController = new DummyRotationMovementController();
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);

            MissileBarrelController missileBarrel = FindObjectOfType<MissileBarrelController>();
            missileBarrel.StaticInitialise();
            missileBarrel.Initialise(targetFilter, targetPositionPredictor, angleCalculator, rotationMovementController, args.FactoryProvider);
            missileBarrel.Target = target;
	    }
	}
}
