using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes.Test.Balancing.Groups;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using UnityEngine;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class BuildableVsBuildableTest : MonoBehaviour, ITestScenario
    {
        private IBuildableGroup _leftGroup, _rightGroup;
        private TimerController _timer;

        protected const int DEFAULT_OFFSET_FROM_CENTRE_IN_M = 15;

        public int leftOffsetInM;
        public int rightOffsetInM;

        private bool IsScenarioOver { get { return !_timer.IsRunning; } }

        private float LeftOffsetInM { get { return leftOffsetInM != default(int) ? leftOffsetInM : DEFAULT_OFFSET_FROM_CENTRE_IN_M; } }
        private float RightOffsetInM { get { return rightOffsetInM != default(int) ? rightOffsetInM : DEFAULT_OFFSET_FROM_CENTRE_IN_M; } }

        public Camera Camera { get; private set; }

        public void Initialise(IPrefabFactory prefabFactory, TestUtils.Helper helper)
        {
            Helper.AssertIsNotNull(prefabFactory, helper);

            // Create left buildable group
            BuildableGroupController leftGroupController = transform.FindNamedComponent<BuildableGroupController>("LeftGroup");
            Vector2 leftSpawnPosition = new Vector2(transform.position.x - LeftOffsetInM, transform.position.y);
            TestUtils.BuildableInitialisationArgs leftGroupArgs = CreateInitialisationArgs(helper, Faction.Blues, Direction.Right);
            _leftGroup = leftGroupController.Initialise(prefabFactory, helper, leftGroupArgs, leftSpawnPosition);
            _leftGroup.BuildablesDestroyed += (sender, e) => OnScenarioComplete();

            // Create right buildable group
            BuildableGroupController rightGroupController = transform.FindNamedComponent<BuildableGroupController>("RightGroup");
            Vector2 rightSpawnPosition = new Vector2(transform.position.x + RightOffsetInM, transform.position.y);
            TestUtils.BuildableInitialisationArgs rightGroupArgs = CreateInitialisationArgs(helper, Faction.Reds, Direction.Left);
            _rightGroup = rightGroupController.Initialise(prefabFactory, helper, rightGroupArgs, rightSpawnPosition);
            _rightGroup.BuildablesDestroyed += (sender, e) => OnScenarioComplete();

            ShowScenarioDetails();

            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;

            // Start timer
            _timer = GetComponentInChildren<TimerController>();
            _timer.Initialise("Time Elapsed: ", "s");
            _timer.Begin();

            OnInitialised();
        }

        protected virtual void OnInitialised() { }

        protected virtual TestUtils.BuildableInitialisationArgs CreateInitialisationArgs(TestUtils.Helper helper, Faction faction, Direction facingDirection)
        {
            return new TestUtils.BuildableInitialisationArgs(helper, faction, parentCruiserDirection: facingDirection);
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
            if (IsScenarioOver)
            {
                return;
            }
         
            _timer.Stop();

            // Destroy all units (because behaviour is undefined if they have no more
            // targets, means the game is won).
            _leftGroup.DestroyAllBuildables();
            _rightGroup.DestroyAllBuildables();
        }
    }
}
