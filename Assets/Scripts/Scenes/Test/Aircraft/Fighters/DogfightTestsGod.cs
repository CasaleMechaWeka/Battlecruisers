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
using BcUtils = BattleCruisers.Utils;

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
			BcUtils.IFactoryProvider factoryProvider1 = _helper.CreateFactoryProvider(fighter2.GameObject);
			_helper.InitialiseBuildable(fighter1, faction: Faction.Reds, factoryProvider: factoryProvider1);
			fighter1.StartConstruction();

			BcUtils.IFactoryProvider factoryProvider2 = _helper.CreateFactoryProvider(fighter1.GameObject);
			_helper.InitialiseBuildable(fighter2, faction: Faction.Blues, factoryProvider: factoryProvider2);
			fighter2.StartConstruction();
		}
	}
}
