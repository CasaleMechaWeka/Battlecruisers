using BattleCruisers.AI.ThreatMonitors;
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
            _threatMonitor = new BuildingThreatMonitor<IBuilding>(_cruiser, _threatEvaluator);
            _threatMonitor.ThreatLevelChanged += (sender, e) => _numOfEventsEmitted++;

            _matchingBuilding = Substitute.For<IFactory>();
            _matchingBuilding.BuildProgress.Returns(0.1f);
            _matchingBuilding2 = Substitute.For<IFactory>();
			_matchingBuilding2.BuildProgress.Returns(0.3f);
			_nonMatchingBuilding = Substitute.For<IBuilding>();
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(_initialThreatLevel, _threatMonitor.CurrentThreatLevel);
        }
    }
}
