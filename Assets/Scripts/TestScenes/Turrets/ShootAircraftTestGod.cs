using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.TestScenes.Utilities;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Turrets
{
	public class ShootAircraftTestGod : MonoBehaviour 
	{
		public BomberController bomber;
		public DefensiveTurret turret;

		void Start () 
		{
			Helper helper = new Helper();

			helper.InitialiseBuildable(turret, faction: Faction.Reds);
			turret.StartConstruction();

			ITargetFinderFactory targetFinderFactory = helper.CreateTargetFinderFactory(turret);
			helper.InitialiseBuildable(bomber, faction: Faction.Blues, targetFinderFactory: targetFinderFactory);
			bomber.StartConstruction();
		}
	}
}
