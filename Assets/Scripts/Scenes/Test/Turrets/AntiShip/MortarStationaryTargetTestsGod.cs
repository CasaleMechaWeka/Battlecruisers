using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Buildables.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
	public class MortarStationaryTargetTestsGod : MonoBehaviour 
	{
		public GameObject target;

		void Start () 
		{
			Helper helper = new Helper();

			// Setup mortars
			MortarController[] mortars = GameObject.FindObjectsOfType(typeof(MortarController)) as MortarController[];
			foreach (MortarController mortar in mortars)
			{
				ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target);
				helper.InitialiseBuildable(mortar, targetsFactory: targetsFactory);
				mortar.StartConstruction();
			}
		}
	}
}
