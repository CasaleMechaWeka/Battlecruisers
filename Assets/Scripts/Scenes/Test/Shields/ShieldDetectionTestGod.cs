using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Shields
{
	public class ShieldDetectionTestGod : MonoBehaviour 
	{
		void Start () 
		{
			Helper helper = new Helper();


			// Setup shields
			ShieldGenerator[] shields = GameObject.FindObjectsOfType<ShieldGenerator>();
			Assert.IsTrue(shields.Length > 0);

			foreach (ShieldGenerator shield in shields)
			{
				helper.InitialiseBuildable(shield, Faction.Blues);
				shield.StartConstruction();
			}


			// Setup attack boats
			AttackBoatController[] attackBoats = GameObject.FindObjectsOfType<AttackBoatController>();
			Assert.IsTrue(attackBoats.Length > 0);

			foreach (AttackBoatController attackBoat in attackBoats)
			{
				helper.InitialiseBuildable(attackBoat, Faction.Reds, parentCruiserDirection: Direction.Left);
				attackBoat.StartConstruction();
			}
		}
	}
}
