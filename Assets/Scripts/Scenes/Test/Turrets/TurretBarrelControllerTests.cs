using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Predictors;
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

			TurretBarrelController[] turretBarrels = GameObject.FindObjectsOfType<TurretBarrelController>() as TurretBarrelController[];
			foreach (TurretBarrelController barrel in turretBarrels)
			{
				barrel.Target = target;
				barrel.Initialise(null, angleCalculator);
			}
		}
	}
}
