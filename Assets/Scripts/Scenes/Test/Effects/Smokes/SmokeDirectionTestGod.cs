using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Smokes
{
    public class SmokeDirectionTestGod : TestGodBase
    {
        private DroneStation _droneStation;

        protected override List<GameObject> GetGameObjects()
        {
            _droneStation = FindObjectOfType<DroneStation>();
            Assert.IsNotNull(_droneStation);

            return new List<GameObject>()
            {
                _droneStation.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            helper.InitialiseBuilding(_droneStation);
            _droneStation.StartConstruction();
            _droneStation.CompletedBuildable += (sender, e) => _droneStation.TakeDamage(_droneStation.MaxHealth - 1, damageSource: null);
        }
    }
}