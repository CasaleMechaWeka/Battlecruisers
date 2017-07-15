using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
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
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft
{
	public class DeathstarTestGod : MonoBehaviour 
	{
		private Helper _helper;

		public Buildable leftTarget, rightTarget;
		public DeathstarController leftDeathstar, rightDeathstar;

		void Start()
		{
			_helper = new Helper();

			SetupPair(leftTarget, rightDeathstar, Faction.Blues);
			SetupPair(rightTarget, leftDeathstar, Faction.Reds);
		}

		private void SetupPair(Buildable target, DeathstarController deathstar, Faction targetFaction)
		{
			// Setup target
			_helper.InitialiseBuildable(target, targetFaction);
			
			
			// Setup deathstar
			Faction deathstarFaction = BcUtils.Helper.GetOppositeFaction(targetFaction);

			Vector2 parentCruiserPosition = deathstar.transform.position;
			Vector2 enemyCruiserPosition = target.transform.position;
			IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition);
			
			_helper.InitialiseBuildable(deathstar, deathstarFaction, aircraftProvider: aircraftProvider);
			deathstar.StartConstruction();
		}
	}
}
