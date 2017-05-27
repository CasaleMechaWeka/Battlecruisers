using BattleCruisers.Projectiles;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class MissileStationaryTargetTestsGod : MissileTestsGod 
	{
		public AircraftController target;

		void Start () 
		{
			Helper helper = new Helper();

			helper.InitialiseBuildable(target);
			target.StartConstruction();

			SetupMissiles(target);
		}
	}
}
