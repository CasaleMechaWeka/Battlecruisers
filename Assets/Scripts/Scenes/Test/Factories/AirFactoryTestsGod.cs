using System.Collections.Generic;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class AirFactoryTestsGod : FactoryTestGod
	{
		public List<Vector2> patrolPoints;

		protected override void OnStart()
		{
			TestAircraftController aircraft = (TestAircraftController)facingRightUnit.Buildable;
			aircraft.patrolPoints = patrolPoints;
		}
	}
}
