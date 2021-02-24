using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Performance.ObjectPooling
{
    public class UnitRecyclingTestGod : TestGodBase
    {
        private Factory _factory;
        
        public UnitWrapper unitPrefab;
        public float timeToDieInS = 1;

        protected override List<GameObject> GetGameObjects()
        {
            _factory = FindObjectOfType<Factory>();
            Assert.IsNotNull(_factory);

            return new List<GameObject>()
            {
                _factory.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            Assert.IsNotNull(unitPrefab);
            unitPrefab.StaticInitialise(helper.CommonStrings);

            TimeScaleDeferrer deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(deferrer);

            helper.InitialiseBuilding(_factory);
            _factory.StartConstruction();
            _factory.CompletedBuildable += (sender, e) => _factory.StartBuildingUnit(unitPrefab);
            _factory.UnitCompleted += (sender, e) => deferrer.Defer(e.CompletedUnit.Destroy, delayInS: timeToDieInS);
        }
    }
}