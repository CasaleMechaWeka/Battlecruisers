using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Units.Aircraft;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
	public class StationaryTargetTestGod : MonoBehaviour 
	{
		private Helper _helper;

		public FighterController fighter1, fighter2, fighter3;
		public AircraftController targetAircraft1, targetAircraft2, targetAircraft3;

		void Start() 
		{
			_helper = new Helper();

			SetupPair(fighter1, targetAircraft1);
			SetupPair(fighter2, targetAircraft2);
			SetupPair(fighter3, targetAircraft3);
		}

		private void SetupPair(FighterController fighter, AircraftController target)
		{
			ITargetsFactory targetsFactory = _helper.CreateTargetsFactory(target.GameObject);
			_helper.InitialiseBuildable(fighter, faction: Faction.Reds, targetsFactory: targetsFactory);
			fighter.StartConstruction();

			_helper.InitialiseBuildable(target, faction: Faction.Blues);
			target.StartConstruction();
		}
	}
}
