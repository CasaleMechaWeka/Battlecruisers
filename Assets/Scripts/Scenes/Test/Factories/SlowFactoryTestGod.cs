using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class SlowFactoryTestGod : TestGodBase
    {
        private Factory _factory;
        public UnitWrapper unitPrefab;

        protected override async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            return await HelperFactory.CreateHelperAsync(buildSpeedMultiplier: 5, updaterProvider: updaterProvider);
        }

        protected override IList<GameObject> GetGameObjects()
        {
            _factory = FindObjectOfType<Factory>();

            return new List<GameObject>()
            {
                _factory.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            Assert.IsNotNull(_factory);
            helper.InitialiseBuilding(_factory);
            _factory.StartConstruction();
            _factory.CompletedBuildable += (sender, e) => _factory.StartBuildingUnit(unitPrefab);
        }
    }
}