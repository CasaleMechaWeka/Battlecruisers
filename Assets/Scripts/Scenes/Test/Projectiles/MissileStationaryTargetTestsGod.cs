using BattleCruisers.Projectiles;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Buildables.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class MissileStationaryTargetTestsGod : MissileTestsGod 
	{
		void Start () 
		{
			Helper helper = new Helper();

			AircraftController target = GameObject.FindObjectOfType<AircraftController>();
			helper.InitialiseBuildable(target);
			target.StartConstruction();

			SetupMissiles(target);
		}
	}
}
