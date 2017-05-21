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
	public class DogfightTestsGod : MonoBehaviour 
	{
		private Helper _helper;

		public FighterController fighter1, fighter2;

		void Start() 
		{
			_helper = new Helper();

			SetupPair(fighter1, fighter2);
		}

		private void SetupPair(FighterController fighter1, FighterController fighter2)
		{
			ITargetsFactory targetsFactory1 = _helper.CreateTargetsFactory(fighter2.GameObject);
			ITargetsFactory targetsFactory2 = _helper.CreateTargetsFactory(fighter1.GameObject);

			_helper.InitialiseBuildable(fighter1, faction: Faction.Reds, targetsFactory: targetsFactory1);
			fighter1.StartConstruction();

			_helper.InitialiseBuildable(fighter2, faction: Faction.Blues, targetsFactory: targetsFactory2);
			fighter2.StartConstruction();
		}
	}
}
