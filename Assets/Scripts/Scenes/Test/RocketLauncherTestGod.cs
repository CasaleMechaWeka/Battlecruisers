using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Movement;
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
		void Start()
		{
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
	}
}
