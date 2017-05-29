using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
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
		public FighterController fighter1, fighter2;

		void Start() 
		{
			Helper helper = new Helper();

			helper.InitialiseBuildable(fighter1, faction: Faction.Reds);
			fighter1.StartConstruction();

			helper.InitialiseBuildable(fighter2, faction: Faction.Blues);
			fighter2.StartConstruction();
		}
	}
}
