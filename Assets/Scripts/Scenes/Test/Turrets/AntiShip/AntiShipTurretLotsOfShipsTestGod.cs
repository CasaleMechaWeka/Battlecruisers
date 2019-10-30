using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AntiShipTurretLotsOfShipsTestGod : TestGodBase 
	{
        private Factory _factory;
        private TurretController[] _turrets;

        public UnitWrapper unitPrefab;

        protected override List<GameObject> GetGameObjects()
        {
            _factory = FindObjectOfType<Factory>();
            _turrets = FindObjectsOfType<TurretController>();

            List<GameObject> gameObjects
                = _turrets
                    .Select(turret => turret.GameObject)
                    .ToList();
            gameObjects.Add(_factory.GameObject);
            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            unitPrefab.StaticInitialise();

            // Factory
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);
            helper.InitialiseBuilding(_factory, Faction.Blues, parentCruiserDirection: Direction.Right, parentCruiser: blueCruiser);
            _factory.CompletedBuildable += Factory_CompletedBuildable;
            _factory.StartConstruction();
            Helper.SetupFactoryForUnitMonitor(_factory, blueCruiser);

            // Turrets
            foreach (TurretController turret in _turrets)
            {
                helper.InitialiseBuilding(turret, Faction.Reds);
			    turret.StartConstruction();
            }
		}

        private void Factory_CompletedBuildable(object sender, EventArgs e)
        {
            ((Factory)sender).StartBuildingUnit(unitPrefab);
        }
    }
}
