using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiAir
{
    public class AntiAirTurretIgnoresSatellitesTestGod : TestGodBase
    {
        private AircraftController[] _aircraftList;
        private TurretController _turret;

        protected override List<GameObject> GetGameObjects()
        {
            _aircraftList = FindObjectsOfType<AircraftController>();
            _turret = FindObjectOfType<TurretController>();

            List<GameObject> gameObjects
                = _aircraftList
                    .Select(aircraft => aircraft.GameObject)
                    .ToList();
            gameObjects.Add(_turret.GameObject);
            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            // Aircraft
            foreach (AircraftController aircraft in _aircraftList)
            {
                helper.InitialiseUnit(aircraft, Faction.Blues);
                aircraft.StartConstruction();
            }

            // Turret
            helper.InitialiseBuilding(_turret, Faction.Reds);
            _turret.StartConstruction();
        }
    }
}