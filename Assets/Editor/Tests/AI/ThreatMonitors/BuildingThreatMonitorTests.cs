using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI
{
    public class BuildingThreatMonitorTests
    {
        private IThreatMonitor _threatMonitor;
        private ICruiserController _cruiser;
        private IThreatEvaluator _threatEvaluator;
        private IFactory _matchingBuilding, _matchingBuilding2;
        private IBuilding _nonMatchingBuilding;
        private int _numOfEventsEmitted;
        private ThreatLevel _initialThreatLevel;

        [SetUp]
        public void SetuUp()
        {
            _numOfEventsEmitted = 0;
            _initialThreatLevel = ThreatLevel.None;

            _cruiser = Substitute.For<ICruiserController>();
            _threatEvaluator = Substitute.For<IThreatEvaluator>();
            _threatEvaluator.FindThreatLevel(value: 17).ReturnsForAnyArgs(_initialThreatLevel);
            _threatMonitor = new BuildingThreatMonitor<IFactory>(_cruiser, _threatEvaluator);
            _threatMonitor.ThreatLevelChanged += (sender, e) => _numOfEventsEmitted++;

            _matchingBuilding = Substitute.For<IFactory>();
            _matchingBuilding2 = Substitute.For<IFactory>();
            _nonMatchingBuilding = Substitute.For<IBuilding>();
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(_initialThreatLevel, _threatMonitor.CurrentThreatLevel);
        }

        [Test]
        public void BuildingStarted_DoesNotEvaluate()
        {
            StartConstructingBuilding(_matchingBuilding);
            _threatEvaluator.DidNotReceiveWithAnyArgs().FindThreatLevel(864);
        }

        [Test]
        public void NonMatchingBuilding_DoesNotEvaluate()
        {
            StartConstructingBuilding(_nonMatchingBuilding);
            _nonMatchingBuilding.CompletedBuildable += Raise.Event();
			_threatEvaluator.DidNotReceiveWithAnyArgs().FindThreatLevel(864);
        }

        #region BuildableProgress
        [Test]
        public void BuildingBuildProgress_LessThanHalfway_DoesNotEvaluate()
        {
            StartConstructingBuilding(_matchingBuilding);
			_matchingBuilding.BuildProgress.Returns(0.1f);
            _matchingBuilding.BuildableProgress += Raise.EventWith(_matchingBuilding, new BuildProgressEventArgs(_matchingBuilding));
			_threatEvaluator.DidNotReceiveWithAnyArgs().FindThreatLevel(864);
		}

		[Test]
		public void BuildingBuildProgress_Halfway_Evaluates()
		{
            StartConstructingBuilding(_matchingBuilding);
			_matchingBuilding.BuildProgress.Returns(0.5f);
			_matchingBuilding.BuildableProgress += Raise.EventWith(_matchingBuilding, new BuildProgressEventArgs(_matchingBuilding));
            _threatEvaluator.Received().FindThreatLevel(_matchingBuilding.BuildProgress);
		}

		[Test]
		public void BuildingBuildProgress_MoreThanHalfway_Evaluates()
		{
            StartConstructingBuilding(_matchingBuilding);
			_matchingBuilding.BuildProgress.Returns(0.51f);
			_matchingBuilding.BuildableProgress += Raise.EventWith(_matchingBuilding, new BuildProgressEventArgs(_matchingBuilding));
			_threatEvaluator.Received().FindThreatLevel(_matchingBuilding.BuildProgress);
		}
		#endregion BuildableProgress

        [Test]
        public void CompletedBuilding_Evaluates()
        {
            StartConstructingBuilding(_matchingBuilding);
			_matchingBuilding.CompletedBuildable += Raise.Event();
            _threatEvaluator.Received().FindThreatLevel(_matchingBuilding.BuildProgress);
        }

        [Test]
        public void DestroyedBuilding_Evaluates()
        {
            StartConstructingBuilding(_matchingBuilding);
            _matchingBuilding.Destroyed += Raise.EventWith(_matchingBuilding, new DestroyedEventArgs(_matchingBuilding));
			_threatEvaluator.Received().FindThreatLevel(value: 0);
        }

        [Test]
        public void BuildProgress_FromMultipleBuildingsIsConsidered()
        {
            // Building 1
            StartConstructingBuilding(_matchingBuilding);
			_matchingBuilding.BuildProgress.Returns(0.1f);

            // Building 2
            StartConstructingBuilding(_matchingBuilding2);
			_matchingBuilding2.BuildProgress.Returns(0.3f);

            _matchingBuilding2.CompletedBuildable += Raise.Event();
            _threatEvaluator.Received().FindThreatLevel(_matchingBuilding.BuildProgress + _matchingBuilding2.BuildProgress);
        }

        private void StartConstructingBuilding(IBuilding building)
        {
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedBuildingConstructionEventArgs(building));
        }
    }
}
