using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Scenes.Test.Balancing.Groups;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.Timers;
using NSubstitute;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class BuildableVsBuildableTest : MonoBehaviour, ITestScenario
    {
		private TimerController _timer;

        protected IBuildableGroup _leftGroup, _rightGroup;
        protected TestUtils.Helper _helper;
        protected IDeferrer _deferrer;

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

        public void Initialise(TestUtils.Helper baseHelper)
        {
            Assert.IsNotNull(baseHelper);

            _helper = baseHelper;
            _isScenarioOver = false;

            _deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(_deferrer);

            ICruiser blueCruiser = _helper.CreateCruiser(Direction.Right, Faction.Blues);
            ICruiser redCruiser = _helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Create left buildable group
            BuildableGroupController leftGroupController = transform.FindNamedComponent<BuildableGroupController>("LeftGroup");
            Vector2 leftSpawnPosition = new Vector2(transform.position.x - LeftOffsetInM, transform.position.y);
            TestUtils.BuildableInitialisationArgs leftGroupArgs = CreateLeftGroupArgs(_helper, leftSpawnPosition, _helper.UpdaterProvider, blueCruiser, redCruiser);
            _leftGroup = leftGroupController.Initialise(_helper.PrefabFactory, _helper, leftGroupArgs, leftSpawnPosition);
            _leftGroup.BuildablesDestroyed += (sender, e) => OnScenarioComplete();

            foreach (IBuildable buildable in _leftGroup.Buildables)
            {
                if (buildable is IUnit unit)
                {
                    TestUtils.Helper.SetupUnitForUnitMonitor(unit, unit.ParentCruiser);
                }
            }

            // Create right buildable group
            BuildableGroupController rightGroupController = transform.FindNamedComponent<BuildableGroupController>("RightGroup");
            Vector2 rightSpawnPosition = new Vector2(transform.position.x + RightOffsetInM, transform.position.y);
            TestUtils.BuildableInitialisationArgs rightGroupArgs = CreateRightGroupArgs(_helper, rightSpawnPosition, _helper.UpdaterProvider, redCruiser, blueCruiser);
            _rightGroup = rightGroupController.Initialise(_helper.PrefabFactory, _helper, rightGroupArgs, rightSpawnPosition);
            _rightGroup.BuildablesDestroyed += (sender, e) => OnScenarioComplete();

            foreach (IBuildable buildable in _rightGroup.Buildables)
            {
                // For the GlobalTargetFinder
                redCruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs((IBuilding)buildable));
            }

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
            IUpdaterProvider updaterProvider,
            ICruiser parentCruiser,
            ICruiser enemyCruiser)
        {
            return 
                new TestUtils.BuildableInitialisationArgs(
                    helper, 
                    Faction.Blues, 
                    parentCruiserDirection: Direction.Right, 
                    updaterProvider: updaterProvider,
                    parentCruiser: parentCruiser,
                    enemyCruiser: enemyCruiser,
                    deferrer: _deferrer);
        }

        protected virtual TestUtils.BuildableInitialisationArgs CreateRightGroupArgs(
            TestUtils.Helper helper, 
            Vector2 spawnPosition,
            IUpdaterProvider updaterProvider,
            ICruiser parentCruiser,
            ICruiser enemyCruiser)
        {
            return
                new TestUtils.BuildableInitialisationArgs(
                    helper,
                    Faction.Reds,
                    parentCruiserDirection: Direction.Left,
                    updaterProvider: updaterProvider,
                    parentCruiser: parentCruiser,
                    enemyCruiser: enemyCruiser,
                    deferrer: _deferrer);
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
