using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Scenes.Test.Balancing.Groups;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using UnityEngine;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class BuildableVsBuildableTest : MonoBehaviour, ITestScenario
    {
		private TimerController _timer;

        protected IBuildableGroup _leftGroup, _rightGroup;
        protected TestUtils.Helper _helper;

        protected const int DEFAULT_OFFSET_FROM_CENTRE_IN_M = 15;

        public int leftOffsetInM;
        public int rightOffsetInM;

        private bool _isScenarioOver;

        protected float LeftOffsetInM => leftOffsetInM != default ? leftOffsetInM : DEFAULT_OFFSET_FROM_CENTRE_IN_M;
        protected float RightOffsetInM => rightOffsetInM != default ? rightOffsetInM : DEFAULT_OFFSET_FROM_CENTRE_IN_M;

        // Lazily initialise, so camera can be accessed even if scenario 
        // initialisation is delayed (kamikaze balancing tests)
        private Camera _camera;
        public Camera Camera 
        { 
            get
            {
                if (_camera == null)
                {
                    _camera = GetComponentInChildren<Camera>();
                    _camera.enabled = false;
                }

                return _camera;
            }
        }

        public void Initialise(IPrefabFactory prefabFactory, TestUtils.Helper helper, IUpdaterProvider updaterProvider)
        {
            Helper.AssertIsNotNull(prefabFactory, helper, updaterProvider);

            _helper = helper;
            _isScenarioOver = false;

            // Create left buildable group
            BuildableGroupController leftGroupController = transform.FindNamedComponent<BuildableGroupController>("LeftGroup");
            Vector2 leftSpawnPosition = new Vector2(transform.position.x - LeftOffsetInM, transform.position.y);
            TestUtils.BuildableInitialisationArgs leftGroupArgs = CreateLeftGroupArgs(_helper, leftSpawnPosition, updaterProvider);
            _leftGroup = leftGroupController.Initialise(prefabFactory, _helper, leftGroupArgs, leftSpawnPosition);
            _leftGroup.BuildablesDestroyed += (sender, e) => OnScenarioComplete();

            // Create right buildable group
            BuildableGroupController rightGroupController = transform.FindNamedComponent<BuildableGroupController>("RightGroup");
            Vector2 rightSpawnPosition = new Vector2(transform.position.x + RightOffsetInM, transform.position.y);
            TestUtils.BuildableInitialisationArgs rightGroupArgs = CreateRightGroupArgs(_helper, rightSpawnPosition, updaterProvider);
            _rightGroup = rightGroupController.Initialise(prefabFactory, _helper, rightGroupArgs, rightSpawnPosition);
            _rightGroup.BuildablesDestroyed += (sender, e) => OnScenarioComplete();

            ShowScenarioDetails();

            // Start timer
            _timer = GetComponentInChildren<TimerController>();
            if (_timer != null)
            {
                _timer.Initialise("Time Elapsed: ", "s");
                _timer.Begin();
			}

            OnInitialised();
        }

        protected virtual void OnInitialised() { }

        protected virtual TestUtils.BuildableInitialisationArgs CreateLeftGroupArgs(
            TestUtils.Helper helper, 
            Vector2 spawnPosition,
            IUpdaterProvider updaterProvider)
        {
            return new TestUtils.BuildableInitialisationArgs(helper, Faction.Blues, parentCruiserDirection: Direction.Right, updaterProvider: updaterProvider);
        }

        protected virtual TestUtils.BuildableInitialisationArgs CreateRightGroupArgs(
            TestUtils.Helper helper, 
            Vector2 spawnPosition,
            IUpdaterProvider updaterProvider)
        {
            return new TestUtils.BuildableInitialisationArgs(helper, Faction.Reds, parentCruiserDirection: Direction.Left, updaterProvider: updaterProvider);
        }

        private void ShowScenarioDetails()
        {
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text 
                = _leftGroup.BuildableKey.PrefabPath.GetFileName() + ": " + _leftGroup.NumOfBuildables 
                + " vs " + _rightGroup.BuildableKey.PrefabPath.GetFileName() + ": " + _rightGroup.NumOfBuildables;
        }

        protected void OnScenarioComplete()
        {
            if (_isScenarioOver)
            {
                return;
            }

            _isScenarioOver = true;

            if (_timer != null)
            {
				_timer.Stop();
            }

            // Destroy all units (because behaviour is undefined if they have no more
            // targets, means the game is won).
            _leftGroup.DestroyAllBuildables();
            _rightGroup.DestroyAllBuildables();
        }
    }
}
