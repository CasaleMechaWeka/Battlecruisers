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
    // FELIX  Avoid duplicate code with ShipsVsShields
    public abstract class OffensivesVsShieldsBalancingTest : MonoBehaviour, ITestScenario
    {
        private IList<IBuildable> _offensives, _aliveShields;
        private TimerController _timer;

        protected const int SHIELD_OFFSET_FROM_CENTRE_IN_M = 15;

        public int numOfShields;
        public int numOfOffensives;

        protected abstract IPrefabKey OffensiveKey { get; }
        protected virtual float OffensiveOffsetInM { get { return SHIELD_OFFSET_FROM_CENTRE_IN_M; } }

        public Camera Camera { get; private set; }

        public void Initialise(IPrefabFactory prefabFactory, TestUtils.Helper helper)
        {
            Helper.AssertIsNotNull(prefabFactory, helper);
            Assert.IsTrue(numOfShields > 0);
            Assert.IsTrue(numOfOffensives > 0);

            ShowScenarioDetails(OffensiveKey);

			IBuildableSpawner buildingSpawner = new BuildingSpawner(prefabFactory, helper);

            // Create offensives
            Vector2 offensivesSpawnPosition = new Vector2(transform.position.x - OffensiveOffsetInM, transform.position.y);
            _offensives = buildingSpawner.SpawnBuildables(OffensiveKey, numOfOffensives, Faction.Blues, Direction.Right, offensivesSpawnPosition);

            // Create shields
            Vector2 shieldSpawnPosition = new Vector2(transform.position.x + SHIELD_OFFSET_FROM_CENTRE_IN_M, transform.position.y);
            float spacingMultiplier = 5;  // So area of effect only damages one shield at a time
            _aliveShields = buildingSpawner.SpawnBuildables(StaticPrefabKeys.Buildings.ShieldGenerator, numOfShields, Faction.Reds, Direction.Left, shieldSpawnPosition, spacingMultiplier);

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

        private void ShowScenarioDetails(IPrefabKey offensiveKey)
        {
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text = "Shields: " + numOfShields + "  " + numOfOffensives + "x" + offensiveKey.PrefabPath.GetFileName();
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
            foreach (ITarget target in _offensives)
            {
                if (!target.IsDestroyed)
                {
                    target.Destroy();
                }
            }
        }
    }
}
