using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class MortarStationaryTargetTestsGod : MonoBehaviour 
	{
		void Start () 
		{
			Helper helper = new Helper();


			// Setup target
			TestAircraftController target = FindObjectOfType<TestAircraftController>();
            target.PatrolPoints = new List<Vector2>() { new Vector2(0, -5), new Vector2(0, -5.01f) };
			target.SetTargetType(TargetType.Ships);  // So mortars will attack this
			helper.InitialiseBuildable(target, Faction.Blues);
			target.StartConstruction();


			// Setup mortars
			MortarController[] mortars = FindObjectsOfType(typeof(MortarController)) as MortarController[];
			foreach (MortarController mortar in mortars)
			{
				helper.InitialiseBuildable(mortar, Faction.Reds);
				mortar.StartConstruction();
			}
		}
	}
}
