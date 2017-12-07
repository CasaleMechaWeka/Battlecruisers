using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Shields
{
    public abstract class ShipsVsShieldsBalancingTest : MonoBehaviour, ITestScenario
    {
        private IList<ITarget> _ships;

        protected TestUtils.Helper _helper;
        protected IPrefabFactory _prefabFactory;

        private const int OFFSET_FROM_CENTRE_IN_M = 10;

        public int numOfShields;
        public int numOfShips;

        protected abstract IPrefabKey ShipKey { get; }

        public Camera Camera { get; private set; }

        public void Initialise(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            Assert.IsTrue(numOfShields > 0);
            Assert.IsTrue(numOfShips > 0);

            _prefabFactory = prefabFactory;
            _helper = new TestUtils.Helper();
            _ships = new List<ITarget>();

            ShowScenarioDetails(ShipKey);

            // FELIX  Avoid duplicate code with DefenceBuildingBalancingTest (CreateBuildings())
            // FELIX  Create ships

            // FELIX  Create shields
            IBuildableSpawner shieldSpawner = new BuildingSpawner(_prefabFactory, _helper);
            Vector2 shieldSpawnPosition = new Vector2(transform.position.x + OFFSET_FROM_CENTRE_IN_M, 0);
            shieldSpawner.SpawnBuildables(StaticPrefabKeys.Buildings.ShieldGenerator, numOfShips, Direction.Left, shieldSpawnPosition);

            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;
        }

        private void ShowScenarioDetails(IPrefabKey shipKey)
        {
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text = "Shields: " + numOfShields + "  " + numOfShips + "x" + shipKey.PrefabPath.GetFileName();
        }

        protected void OnScenarioComplete()
        {
            // Destroy all units (because behaviour is undefined they have no more
            // targets, means the game is won).
            foreach (ITarget target in _ships)
            {
                if (!target.IsDestroyed)
                {
                    target.Destroy();
                }
            }
        }
    }
}
