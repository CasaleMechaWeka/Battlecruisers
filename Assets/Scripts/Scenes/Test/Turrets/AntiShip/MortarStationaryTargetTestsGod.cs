using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
	public class MortarStationaryTargetTestsGod : MonoBehaviour 
	{
		public GameObject target;
		public DefensiveTurret mortar;

		void Start () 
		{
			Helper helper = new Helper();
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target);

			helper.InitialiseBuildable(mortar, faction: Faction.Reds, targetsFactory: targetsFactory);
			mortar.StartConstruction();
		}
	}
}
