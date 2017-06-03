using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Scenes.Test;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
	public class BombingTestsGod : MonoBehaviour 
	{
		public BomberController bomberToLeft, bomberToRight;
		public List<Vector2> patrolPoints;

		void Start() 
		{
			Helper helper = new Helper();

			AirFactory factory = GameObject.FindObjectOfType<AirFactory>();
			helper.InitialiseBuildable(factory, Faction.Blues);

			ITargetFilter targetFilter = new FactionAndTargetTypeFilter(factory.Faction, factory.TargetType);
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(factory.GameObject, targetFilter);
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: patrolPoints);

			helper.InitialiseBuildable(bomberToLeft, Faction.Reds, aircraftProvider: aircraftProvider, targetsFactory: targetsFactory, parentCruiserDirection: Direction.Right);
			bomberToLeft.StartConstruction();

			helper.InitialiseBuildable(bomberToRight, Faction.Reds, aircraftProvider: aircraftProvider, targetsFactory: targetsFactory, parentCruiserDirection: Direction.Left);
			bomberToRight.StartConstruction();
		}
	}
}
