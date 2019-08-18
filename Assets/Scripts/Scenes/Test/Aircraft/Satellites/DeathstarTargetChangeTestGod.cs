using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class DeathstarTargetChangeTestGod : TestGodBase
    {
        private Helper _helper;
        private IPrefabFactory _prefabFactory;

        public BuildingWrapper targetPrefab;

        protected override void Start()
        {
            base.Start();

            _helper = new Helper(buildSpeedMultiplier: BCUtils.BuildSpeedMultipliers.DEFAULT, updaterProvider: _updaterProvider);
            _prefabFactory = new PrefabFactory(new PrefabFetcher());

            // Setup target 1
            Vector2 target1Position = new Vector2(-3, 0);
            ConstructBuilding(target1Position);
            
            // Setup target 2
            Vector2 target2Position = new Vector2(3, 0);
            ConstructBuilding(target2Position);

            // Setup deathstar
            Vector2 parentCruiserPosition = new Vector2(-20, 0);
            Vector2 enemyCruiserPosition = new Vector2(0, 0);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, new BCUtils.RandomGenerator());

            DeathstarController deathstar = FindObjectOfType<DeathstarController>();
            _helper.InitialiseUnit(deathstar, Faction.Blues, aircraftProvider: aircraftProvider);
            deathstar.StartConstruction();
        }

        private void ConstructBuilding(Vector2 position)
        {
            IBuilding building = _prefabFactory.CreateBuilding(targetPrefab);
            building.Position = position;
            _helper.InitialiseBuilding(building, Faction.Reds);
            building.StartConstruction();

            // Rebuild building instantly :D
            building.Destroyed += (sender, e) => ConstructBuilding(position);
        }
    }
}
