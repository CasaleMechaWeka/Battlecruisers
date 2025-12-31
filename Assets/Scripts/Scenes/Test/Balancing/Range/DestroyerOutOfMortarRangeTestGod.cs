using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Balancing.Range
{
    public class DestroyerOutOfMortarRangeTestGod : TestGodBase
    {
        private IBuilding _mortar;
        private IUnit[] _destroyers;

        protected override List<GameObject> GetGameObjects()
        {
            _mortar = FindObjectOfType<TurretController>();
            _destroyers = FindObjectsOfType<ShipController>();

            List<GameObject> gameObjects
                = _destroyers
                    .Select(destroyer => destroyer.GameObject)
                    .ToList();
            gameObjects.Add(_mortar.GameObject);
            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            // Initialise mortar
            helper.InitialiseBuilding(_mortar, Faction.Reds, parentCruiserDirection: Direction.Left);
            _mortar.StartConstruction();

            // Initialise destroyers
            foreach (IUnit destroyer in _destroyers)
            {
                helper.InitialiseUnit(destroyer, Faction.Blues, parentCruiserDirection: Direction.Right, parentCruiser: blueCruiser);
                destroyer.StartConstruction();
                Helper.SetupUnitForUnitMonitor(destroyer, blueCruiser);
            }
        }
    }
}
