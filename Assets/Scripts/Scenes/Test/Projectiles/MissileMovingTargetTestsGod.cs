using System.Collections.Generic;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class MissileMovingTargetTestsGod : MissileTestsGod 
	{
		public List<Vector2> targetPatrolPoints;

		void Start () 
		{
			Helper helper = new Helper();

			TestAircraftController target = FindObjectOfType<TestAircraftController>();
			target.PatrolPoints = targetPatrolPoints;
			helper.InitialiseBuildable(target);
			target.StartConstruction();

			SetupMissiles(target);
		}
	}
}
