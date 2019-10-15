using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BuildProgressTestsGod : TestGodBase
    {
        private Building[] _buildings;

        public GameObject dummyEnemyCruiser;

        protected override IList<GameObject> GetGameObjects()
        {
            _buildings = FindObjectsOfType<Building>();
            return 
                _buildings
                    .Select(building => building.GameObject)
                    .ToList();
        }

        protected async override Task<Helper> CreateHelper(IUpdaterProvider updaterProvider)
        {
            return await HelperFactory.CreateHelperAsync(buildSpeedMultiplier: 5, updaterProvider: updaterProvider);
        }

        protected override void Setup(Helper helper)
        {
            ICruiser enemyCruiser = helper.CreateCruiser(dummyEnemyCruiser);

            foreach (Building building in _buildings)
            {
                helper.InitialiseBuilding(building, enemyCruiser: enemyCruiser);
                building.CompletedBuildable += (sender, e) => building.InitiateDelete();
                building.StartConstruction();
            }
        }
    }
}