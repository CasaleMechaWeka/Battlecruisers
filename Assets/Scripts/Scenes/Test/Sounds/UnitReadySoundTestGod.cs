using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Sounds
{
    public class UnitReadySoundTestGod : TestGodBase
    {
        private AirFactory _airFactory;
        public UnitWrapper aircraftPrefab;

        protected override async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            return await HelperFactory.CreateHelperAsync(buildSpeedMultiplier: BCUtils.BuildSpeedMultipliers.DEFAULT_TUTORIAL, updaterProvider: updaterProvider);
        }

        protected override List<GameObject> GetGameObjects()
        {
            _airFactory = FindObjectOfType<AirFactory>();
            Assert.IsNotNull(_airFactory);

            return new List<GameObject>()
            {
                _airFactory.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            helper.InitialiseBuilding(_airFactory);
            _airFactory.StartConstruction();
            _airFactory.CompletedBuildable += (sender, e) => _airFactory.StartBuildingUnit(aircraftPrefab);
        }
    }
}