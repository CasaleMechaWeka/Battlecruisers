using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class DeathstarTargetChangeTestGod : TestGodBase
    {
        private DeathstarController _deathstar;
        private Helper _helper;

        public BuildingWrapper targetPrefab;

        protected override List<GameObject> GetGameObjects()
        {
            _deathstar = FindObjectOfType<DeathstarController>();
            return new List<GameObject>()
            {
                _deathstar.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            _helper = helper;

            // Setup target 1
            Vector2 target1Position = new Vector2(-3, 0);
            ConstructBuilding(target1Position);
            
            // Setup target 2
            Vector2 target2Position = new Vector2(3, 0);
            ConstructBuilding(target2Position);

            // Setup deathstar
            Vector2 parentCruiserPosition = new Vector2(-20, 0);
            Vector2 enemyCruiserPosition = new Vector2(0, 0);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, BCUtils.RandomGenerator.Instance);

            _helper.InitialiseUnit(_deathstar, Faction.Blues, aircraftProvider: aircraftProvider);
            _deathstar.StartConstruction();
        }

        private void ConstructBuilding(Vector2 position)
        {
            BuildableWrapper<IBuilding> buildingWrapper = Instantiate(targetPrefab);
            buildingWrapper.gameObject.SetActive(true);
            buildingWrapper.StaticInitialise(_helper.CommonStrings);

            IBuilding building = buildingWrapper.Buildable;
            building.Position = position;
            _helper.InitialiseBuilding(building, Faction.Reds);
            building.StartConstruction();

            // Rebuild building instantly :D
            building.Destroyed += (sender, e) => ConstructBuilding(position);
        }

        protected async override Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            return await HelperFactory.CreateHelperAsync(buildSpeedMultiplier: BCUtils.BuildSpeedMultipliers.DEFAULT, updaterProvider: _updaterProvider);
        }
    }
}
