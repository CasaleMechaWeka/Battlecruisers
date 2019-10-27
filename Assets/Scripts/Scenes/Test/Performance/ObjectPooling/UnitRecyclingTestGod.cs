using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Performance.ObjectPooling
{
    public class UnitRecyclingTestGod : TestGodBase
    {
        private Factory _factory;
        public UnitWrapper unitPrefab;

        protected override async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            return await HelperFactory.CreateHelperAsync(buildSpeedMultiplier: BCUtils.BuildSpeedMultipliers.FAST, updaterProvider: updaterProvider);
        }

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
            unitPrefab.StaticInitialise();

            TimeScaleDeferrer deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(deferrer);

            helper.InitialiseBuilding(_factory);
            _factory.StartConstruction();
            _factory.CompletedBuildable += (sender, e) => _factory.StartBuildingUnit(unitPrefab);
            _factory.UnitCompleted += (sender, e) => deferrer.Defer(e.CompletedUnit.Destroy, delayInS: 1);
        }
    }
}