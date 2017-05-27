using BattleCruisers.Projectiles;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class MissileTestsGod : MonoBehaviour 
	{
		public MissileController missile;
		public AircraftController target;

		void Start () 
		{
			Helper helper = new Helper();

			helper.InitialiseBuildable(target);
			target.StartConstruction();

			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			MissileStats missileStats = new MissileStats(damage: 50, maxVelocityInMPerS: 30);
			Vector2 initialVelocity = new Vector2(5, 5);

			missile.Initialise(target, targetFilter, missileStats, initialVelocity);
		}
	}
}
