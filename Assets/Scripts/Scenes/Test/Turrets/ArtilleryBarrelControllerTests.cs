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
	public class ArtilleryBarrelControllerTests : MonoBehaviour 
	{
		public GameObject targetGameObject;

		void Start()
		{
			ITarget target = Substitute.For<ITarget>();
			target.GameObject.Returns(targetGameObject);

			IAngleCalculator angleCalculator = new ArtilleryAngleCalculator(new TargetPositionPredictorFactory());

			TurretBarrelController[] artilleryBarrels = GameObject.FindObjectsOfType<TurretBarrelController>() as TurretBarrelController[];
			foreach (TurretBarrelController barrel in artilleryBarrels)
			{
				barrel.Target = target;
				barrel.Initialise(Faction.Blues, angleCalculator);
			}
		}
	}
}
