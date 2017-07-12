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
	public class BurstFireTestsGod : MonoBehaviour 
	{
		public BarrelController barrel1, barrel2, barrel3;
		public GameObject target1, target2, target3;

		void Start()
		{
			IAngleCalculator angleCalculator = new LeadingAngleCalculator(new TargetPositionPredictorFactory());

			InitialisePair(barrel1, target1, angleCalculator);
			InitialisePair(barrel2, target2, angleCalculator);
			InitialisePair(barrel3, target3, angleCalculator);
		}

		private void InitialisePair(BarrelController barrel, GameObject targetGameObject, IAngleCalculator angleCalculator)
		{
			ITarget target = Substitute.For<ITarget>();
			Vector2 targetPosition = targetGameObject.transform.position;
			target.Position.Returns(targetPosition);
			barrel.Target = target;
			IRotationMovementController rotationMovementController = new RotationMovementController(angleCalculator, barrel.TurretStats.turretRotateSpeedInDegrees, barrel.transform);
			barrel.Initialise(null, angleCalculator, rotationMovementController);
		}
	}
}
