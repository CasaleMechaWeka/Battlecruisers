using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class ArtilleryBarrelControllerTests : MonoBehaviour 
	{
		public GameObject targetGameObject;

		void Start()
		{
			ITarget target = Substitute.For<ITarget>();
			target.GameObject.Returns(targetGameObject);

			IAngleCalculator angleCalculator = new ArtilleryAngleCalculator(new TargetPositionPredictorFactory());
			ITargetFilter targetFilter = Substitute.For<ITargetFilter>();

			BarrelController[] artilleryBarrels = FindObjectsOfType<BarrelController>();
			
            foreach (BarrelController barrel in artilleryBarrels)
			{
				barrel.StaticInitialise();
				IRotationMovementController rotationMovementController = new RotationMovementController(angleCalculator, barrel.TurretStats.turretRotateSpeedInDegrees, barrel.transform);
				barrel.Target = target;
				barrel.Initialise(targetFilter, angleCalculator, rotationMovementController);
			}
		}
	}
}
