using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.Offensive;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Buildables.Units.Aircraft;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class RocketLauncherTestGod : MonoBehaviour 
	{
		public Camera overviewCamera, closeUpCamera;

		void Start()
		{
			closeUpCamera.enabled = true;
			overviewCamera.enabled = false;


			Helper helper = new Helper();


			// Setup target
			AirFactory target = GameObject.FindObjectOfType<AirFactory>();
			helper.InitialiseBuildable(target, Faction.Blues);
			target.StartConstruction();


			// Setup rocket launcher
			RocketLauncherController rocketLauncher = GameObject.FindObjectOfType<RocketLauncherController>();
			ITargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target.GameObject, targetFilter);
			helper.InitialiseBuildable(rocketLauncher, Faction.Reds, targetsFactory: targetsFactory);
			rocketLauncher.StartConstruction();
		}

		public void ToggleCamera()
		{
			overviewCamera.enabled = !overviewCamera.enabled;
			closeUpCamera.enabled = !closeUpCamera.enabled;
		}
	}
}
