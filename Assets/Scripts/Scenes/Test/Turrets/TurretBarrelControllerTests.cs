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
	public class TurretBarrelControllerTests : MonoBehaviour 
	{
		public GameObject targetGameObject;

		void Start()
		{
			ITarget target = Substitute.For<ITarget>();
			Vector2 targetPosition = targetGameObject.transform.position;
			target.Position.Returns(targetPosition);

			IAngleCalculator angleCalculator = new AngleCalculator(new TargetPositionPredictorFactory());

			BarrelController[] turretBarrels = GameObject.FindObjectsOfType<BarrelController>() as BarrelController[];
			foreach (BarrelController barrel in turretBarrels)
			{
				IRotationMovementController rotationMovementController = new RotationMovementController(angleCalculator, barrel.TurretStats.turretRotateSpeedInDegrees, barrel.transform);
				barrel.Target = target;
				barrel.Initialise(null, angleCalculator, rotationMovementController);
			}
		}
	}
}
