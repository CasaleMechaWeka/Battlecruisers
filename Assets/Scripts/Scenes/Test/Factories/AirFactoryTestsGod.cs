using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Fetchers;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Buildables.Units.Aircraft;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Factories
{
	public class AirFactoryTestsGod : FactoryTestGod
	{
		public List<Vector2> patrolPoints;

		protected override void Factory_StartedBuildingUnit(object sender, StartedConstructionEventArgs e)
		{
			e.Buildable.CompletedBuildable += Aircraft_CompletedBuildable;
		}

		private void Aircraft_CompletedBuildable (object sender, EventArgs e)
		{
			TestAircraftController aircraft = (TestAircraftController)sender;
			aircraft.PatrolPoints = patrolPoints;
		}
	}
}
