using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Shields
{
    public class ShieldDetectionTestGod : TestGodBase
	{
        private ShieldGenerator[] _shields;
        private AttackBoatController[] _attackBoats;

        protected override List<GameObject> GetGameObjects()
        {
			_shields = FindObjectsOfType<ShieldGenerator>();
			Assert.IsTrue(_shields.Length > 0);
            List<GameObject> gameObjects
                = _shields
                    .Select(shield => shield.GameObject)
                    .ToList();

			_attackBoats = FindObjectsOfType<AttackBoatController>();
			Assert.IsTrue(_attackBoats.Length > 0);
            List<GameObject> boatGameObjects
                = _attackBoats
                    .Select(boat => boat.GameObject)
                    .ToList();

            gameObjects.AddRange(boatGameObjects);
            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
			// Setup shields
			foreach (ShieldGenerator shield in _shields)
			{
                helper.InitialiseBuilding(shield, Faction.Blues);
				shield.StartConstruction();
			}

			// Setup attack boats
			foreach (AttackBoatController attackBoat in _attackBoats)
			{
                helper.InitialiseUnit(attackBoat, Faction.Reds, parentCruiserDirection: Direction.Left);
				attackBoat.StartConstruction();
			}
		}
	}
}
