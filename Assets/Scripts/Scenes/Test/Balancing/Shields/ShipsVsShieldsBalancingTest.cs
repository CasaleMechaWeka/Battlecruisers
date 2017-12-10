using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Shields
{
    public abstract class ShipsVsShieldsBalancingTest : MonoBehaviour, ITestScenario
    {
        private IList<IBuildable> _ships, _aliveShields;
        private TimerController _timer;

        protected const int SHIELD_OFFSET_FROM_CENTRE_IN_M = 15;

        public int numOfShields;
        public int numOfShips;

        protected abstract IPrefabKey ShipKey { get; }
        protected virtual float ShipOffsetInM { get { return SHIELD_OFFSET_FROM_CENTRE_IN_M; } }

        public Camera Camera { get; private set; }

        public void Initialise(IPrefabFactory prefabFactory, TestUtils.Helper helper)
        {
            Helper.AssertIsNotNull(prefabFactory, helper);
            Assert.IsTrue(numOfShields > 0);
            Assert.IsTrue(numOfShips > 0);

            ShowScenarioDetails(ShipKey);

            // Create ships
            IBuildableSpawner shipSpawner = new UnitSpawner(prefabFactory, helper);
            Vector2 shipSpawnPosition = new Vector2(transform.position.x - ShipOffsetInM, transform.position.y);
            _ships = shipSpawner.SpawnBuildables(ShipKey, numOfShips, Faction.Blues, Direction.Right, shipSpawnPosition);

            // Create shields
            IBuildableSpawner shieldSpawner = new BuildingSpawner(prefabFactory, helper);
            Vector2 shieldSpawnPosition = new Vector2(transform.position.x + SHIELD_OFFSET_FROM_CENTRE_IN_M, transform.position.y);
            float spacingMultiplier = 5;  // So area of effect only damages one shield at a time
            _aliveShields = shieldSpawner.SpawnBuildables(StaticPrefabKeys.Buildings.ShieldGenerator, numOfShields, Faction.Reds, Direction.Left, shieldSpawnPosition, spacingMultiplier);

            foreach (IBuildable shield in _aliveShields)
            {
                shield.Destroyed += Shield_Destroyed;
            }

            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;

            // Start timer
            _timer = GetComponentInChildren<TimerController>();
            _timer.Initialise("Time Elapsed: ", "s");
            _timer.Begin();
        }

        private void ShowScenarioDetails(IPrefabKey shipKey)
        {
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text = "Shields: " + numOfShields + "  " + numOfShips + "x" + shipKey.PrefabPath.GetFileName();
        }

        private void Shield_Destroyed(object sender, DestroyedEventArgs e)
        {
            IBuildable destroyedShield = e.DestroyedTarget.Parse<IBuildable>();
            _aliveShields.Remove(destroyedShield);

            if (_aliveShields.Count == 0)
            {
                OnScenarioComplete();
            }
        }

        protected void OnScenarioComplete()
        {
            _timer.Stop();

            // Destroy all units (because behaviour is undefined if they have no more
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
