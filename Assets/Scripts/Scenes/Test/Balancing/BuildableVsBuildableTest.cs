using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public abstract class BuildableVsBuildableTest : MonoBehaviour, ITestScenario
    {
        private IList<IBuildable> _leftBuildables, _rightBuildables;
        private TimerController _timer;

        protected const int DEFAULT_OFFSET_FROM_CENTRE_IN_M = 15;

        public int numOfLeftBuildable;
        public int numOfRightBuildable;

        private bool IsScenarioOver { get { return !_timer.IsRunning; } }

        protected virtual float LeftOffsetInM { get { return DEFAULT_OFFSET_FROM_CENTRE_IN_M; } }
        protected virtual float RightOffsetInM { get { return DEFAULT_OFFSET_FROM_CENTRE_IN_M; } }

        protected abstract IPrefabKey LeftBuildableKey { get; }
        protected abstract IPrefabKey RightBuildableKey { get; }

        public Camera Camera { get; private set; }

        public void Initialise(IPrefabFactory prefabFactory, TestUtils.Helper helper)
        {
            Helper.AssertIsNotNull(prefabFactory, helper);
            Assert.IsTrue(numOfLeftBuildable > 0);
            Assert.IsTrue(numOfRightBuildable > 0);

            ShowScenarioDetails(LeftBuildableKey, RightBuildableKey);

            // Create left hand buildables
            _leftBuildables = CreateLeftHandBuildables(LeftBuildableKey);
            foreach (IBuildable leftBuildable in _leftBuildables)
            {
                leftBuildable.Destroyed += (sender, e) => BuildableDestroyed(_leftBuildables, leftBuildable);
            }

            // Create right hand buildables
            _rightBuildables = CreateRightHandBuildables(RightBuildableKey);
            foreach (IBuildable rightBuildable in _rightBuildables)
            {
                rightBuildable.Destroyed += (sender, e) => BuildableDestroyed(_rightBuildables, rightBuildable);
            }

            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;

            // Start timer
            _timer = GetComponentInChildren<TimerController>();
            _timer.Initialise("Time Elapsed: ", "s");
            _timer.Begin();
        }

        protected abstract IList<IBuildable> CreateLeftHandBuildables(IPrefabKey buildableKey);
        protected abstract IList<IBuildable> CreateRightHandBuildables(IPrefabKey buildableKey);

        private void ShowScenarioDetails(IPrefabKey leftBuildableKey, IPrefabKey rightBuildableKey)
        {
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text = leftBuildableKey.PrefabPath.GetFileName() + ": " + numOfLeftBuildable + " vs " + rightBuildableKey.PrefabPath.GetFileName() + ": " + numOfRightBuildable;
        }

        private void BuildableDestroyed(IList<IBuildable> remainingBuildables, IBuildable destroyedBuildable)
        {
            Assert.IsTrue(remainingBuildables.Contains(destroyedBuildable));
            remainingBuildables.Remove(destroyedBuildable);

            if (remainingBuildables.Count == 0)
            {
                OnScenarioComplete();
            }
        }

        protected void OnScenarioComplete()
        {
            if (IsScenarioOver)
            {
                return;
            }
         
            _timer.Stop();

            // Destroy all units (because behaviour is undefined if they have no more
            // targets, means the game is won).
            foreach (ITarget target in _leftBuildables)
            {
                if (!target.IsDestroyed)
                {
                    target.Destroy();
                }
            }
        }
    }
}
