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
	public class DeathstarTestGod : MonoBehaviour 
	{
		public List<Vector2> patrolPoints;

		void Start() 
		{
			Helper helper = new Helper();


			// Setup target
			Buildable target = GameObject.FindObjectOfType<AirFactory>();
			helper.InitialiseBuildable(target, Faction.Blues);


			// Setup deathstar
			// FELIX
//			ITargetFilter targetFilter = new FactionAndTargetTypeFilter(target.Faction, target.TargetType);
//			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target.GameObject, targetFilter);
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(deathstarPatrolPoints: patrolPoints);

			DeathstarController deathstar = GameObject.FindObjectOfType<DeathstarController>();
			helper.InitialiseBuildable(deathstar, Faction.Reds, aircraftProvider: aircraftProvider);
			deathstar.StartConstruction();
		}
	}
}
