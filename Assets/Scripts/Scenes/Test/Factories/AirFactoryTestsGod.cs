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
	public class AirFactoryTestsGod : MonoBehaviour 
	{
		public AirFactory airFactoryFacingRight, airFactoryFacingLeft;
		public UnitWrapper aircraftPrefab;
		public List<Vector2> patrolPoints;

		void Start () 
		{
			Helper helper = new Helper();

			IPrefabFactory prefabFactory = Substitute.For<IPrefabFactory>();
			prefabFactory.CreateUnit(aircraftPrefab).Returns(callInfo => Instantiate(aircraftPrefab).Unit);

			helper.InitialiseBuildable(airFactoryFacingRight, prefabFactory: prefabFactory, parentCruiserDirection: Direction.Right);
			helper.InitialiseBuildable(airFactoryFacingLeft, prefabFactory: prefabFactory, parentCruiserDirection: Direction.Left);

			airFactoryFacingRight.CompletedBuildable += Factory_CompletedBuildable;
			airFactoryFacingRight.StartedBuildingUnit += Factory_StartedBuildingUnit;

			airFactoryFacingLeft.CompletedBuildable += Factory_CompletedBuildable;
			airFactoryFacingLeft.StartedBuildingUnit += Factory_StartedBuildingUnit;

			airFactoryFacingRight.StartConstruction();
			airFactoryFacingLeft.StartConstruction();
		}

		private void Factory_CompletedBuildable(object sender, EventArgs e)
		{
			((Factory)sender).UnitWrapper = aircraftPrefab;
		}

		private void Factory_StartedBuildingUnit(object sender, StartedConstructionEventArgs e)
		{
			e.Buildable.CompletedBuildable += Aircraft_CompletedBuildable;
		}

		private void Aircraft_CompletedBuildable (object sender, EventArgs e)
		{
			AircraftController aircraft = (AircraftController)sender;
			aircraft.PatrolPoints = patrolPoints;
			aircraft.StartPatrolling();
		}
	}
}
