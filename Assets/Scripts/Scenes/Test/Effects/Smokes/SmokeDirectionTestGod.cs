using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Smokes
{
    public class SmokeDirectionTestGod : TestGodBase
    {
        private DroneStation[] _droneStations;

        protected override List<GameObject> GetGameObjects()
        {
            _droneStations = FindObjectsOfType<DroneStation>();
            Assert.IsTrue(_droneStations.Length != 0);

            return 
                _droneStations
                    .Select(station => station.GameObject)
                    .ToList();
        }

        protected override void Setup(Helper helper)
        {
            foreach (DroneStation station in _droneStations)
            {
                helper.InitialiseBuilding(station);
                station.StartConstruction();
                station.CompletedBuildable += (sender, e) => station.TakeDamage(station.MaxHealth - 1, damageSource: null);
            }
        }
    }
}