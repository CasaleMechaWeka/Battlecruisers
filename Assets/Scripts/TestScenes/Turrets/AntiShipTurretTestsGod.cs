using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.TestScenes.Utilities;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Turrets
{
	public class AntiShipTurretTestsGod : MonoBehaviour 
	{
		public AttackBoatController boat;
		public DefensiveTurret rightTurret;

		void Start () 
		{
			Helper helper = new Helper();

			helper.InitialiseBuildable(boat, faction: Faction.Blues);
			boat.StartConstruction();

			helper.InitialiseBuildable(rightTurret, faction: Faction.Reds);
			rightTurret.StartConstruction();
		}
	}
}
