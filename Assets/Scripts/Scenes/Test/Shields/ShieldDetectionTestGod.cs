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
		private ShieldGenerator _shield;
		private AttackBoatController _attackBoat;

		void Start () 
		{
			Helper helper = new Helper();

			// Setup shield
			_shield = GameObject.FindObjectOfType<ShieldGenerator>();
			Assert.IsNotNull(_shield);
			helper.InitialiseBuildable(_shield, Faction.Blues);
			_shield.StartConstruction();

			// Setup turret
			_attackBoat = GameObject.FindObjectOfType<AttackBoatController>();
			Assert.IsNotNull(_attackBoat);
			helper.InitialiseBuildable(_attackBoat, Faction.Reds, parentCruiserDirection: Direction.Left);
			_attackBoat.StartConstruction();
		}
	}
}
