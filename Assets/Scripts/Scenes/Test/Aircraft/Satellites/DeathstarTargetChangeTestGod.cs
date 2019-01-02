using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class DeathstarTargetChangeTestGod : MonoBehaviour
    {
        private Helper _helper;
        private IPrefabFactory _prefabFactory;

        public BuildingWrapper targetPrefab;

        void Start()
        {
            _helper = new Helper();
            _prefabFactory = new PrefabFactory(new PrefabFetcher());

            // Setup target 1
            Vector2 target1Position = new Vector2(-3, 0);
            ConstructBuilding(target1Position);
            
            // Setup target 2
            Vector2 target2Position = new Vector2(3, 0);
            ConstructBuilding(target2Position);

            // Setup deathstar
            Vector2 parentCruiserPosition = new Vector2(-20, 0);
            Vector2 enemyCruisrePosition = new Vector2(0, 0);
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruisrePosition, new BcUtils.RandomGenerator());

            DeathstarController deathstar = FindObjectOfType<DeathstarController>();
            _helper.InitialiseUnit(deathstar, Faction.Blues, aircraftProvider: aircraftProvider);
            deathstar.StartConstruction();
        }

        private void ConstructBuilding(Vector2 position)
        {
            IBuilding building = _prefabFactory.CreateBuilding(targetPrefab);
            _helper.InitialiseBuilding(building, Faction.Reds);
            building.Position = position;

            // Rebuild building instantly :D
            building.Destroyed += (sender, e) => ConstructBuilding(position);
        }
    }
}
