using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.Offensive;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets;

namespace BattleCruisers.Scenes.Test
{
	public class RailgunTestGod : MonoBehaviour 
	{
		void Start()
		{
			Helper helper = new Helper();


			// Setup target
			Buildable target = GameObject.FindObjectOfType<AirFactory>();
			helper.InitialiseBuildable(target, Faction.Reds);
			target.StartConstruction();


			// Setup railgun
			Buildable railgun = GameObject.FindObjectOfType<RailgunController>();
			ITargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target.GameObject, targetFilter);
			helper.InitialiseBuildable(railgun, Faction.Blues, targetsFactory: targetsFactory);
			railgun.StartConstruction();
		}
	}
}
