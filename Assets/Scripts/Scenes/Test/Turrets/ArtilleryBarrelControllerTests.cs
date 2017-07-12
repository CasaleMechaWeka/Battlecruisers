using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Utils;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
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

			BarrelController[] artilleryBarrels = GameObject.FindObjectsOfType<BarrelController>() as BarrelController[];
			foreach (BarrelController barrel in artilleryBarrels)
			{
				IRotationMovementController rotationMovementController = new RotationMovementController(angleCalculator, barrel.TurretStats.turretRotateSpeedInDegrees, barrel.transform);
				barrel.Target = target;
				barrel.Initialise(null, angleCalculator, rotationMovementController);
			}
		}
	}
}
