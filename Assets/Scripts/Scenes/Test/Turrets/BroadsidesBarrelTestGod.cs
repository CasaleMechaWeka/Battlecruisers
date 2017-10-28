using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BroadsidesBarrelTestGod : MonoBehaviour
	{
		void Start()
		{
            // Initialise target
            Helper helper = new Helper();
            Factory target = FindObjectOfType<Factory>();
            helper.InitialiseBuilding(target);


            // Initialise double barrel
			IAngleCalculator angleCalculator = new ArtilleryAngleCalculator(new TargetPositionPredictorFactory());
            IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter()
            {
                Target = target
            };

            ShellTurretBarrelController doubleBarrel = FindObjectOfType<ShellTurretBarrelController>();
			doubleBarrel.StaticInitialise();
			IRotationMovementController rotationMovementController = new RotationMovementController(angleCalculator, doubleBarrel.TurretStats.TurretRotateSpeedInDegrees, doubleBarrel.transform);
			doubleBarrel.Target = target;
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);
            doubleBarrel.Initialise(targetFilter, angleCalculator, rotationMovementController, args.FactoryProvider.DamageApplierFactory);
		}
	}
}
