using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI
{
    public class FactoryThreatMonitorTests
    {
        private IThreatMonitor _threatMonitor;
        private ICruiserController _cruiser;
        private IThreatEvaluator _threatEvaluator;
        private IFactory _threateningFactory, _threateningFactory2, _nonThreateningFactory;
        private IBuilding _nonFactoryBuilding;
        private int _numOfEventsEmitted;
        private ThreatLevel _initialThreatLevel;

        [SetUp]
        public void SetuUp()
        {
            _numOfEventsEmitted = 0;
            _initialThreatLevel = ThreatLevel.None;

            UnitCategory threatCategory = UnitCategory.Aircraft;
            UnitCategory nonThreatCategory = UnitCategory.Naval;

            _cruiser = Substitute.For<ICruiserController>();
            _threatEvaluator = Substitute.For<IThreatEvaluator>();
            _threatEvaluator.FindThreatLevel(value: 17).ReturnsForAnyArgs(_initialThreatLevel);
            _threatMonitor = new FactoryThreatMonitor(_cruiser, threatCategory, _threatEvaluator);
            _threatMonitor.ThreatLevelChanged += (sender, e) => _numOfEventsEmitted++;

            _threateningFactory = Substitute.For<IFactory>();
            _threateningFactory.NumOfDrones.Returns(2);
            _threateningFactory.UnitCategory.Returns(threatCategory);

            _threateningFactory2 = Substitute.For<IFactory>();
            _threateningFactory2.NumOfDrones.Returns(7);
            _threateningFactory2.UnitCategory.Returns(threatCategory);

            _nonThreateningFactory = Substitute.For<IFactory>();
            _nonThreateningFactory.UnitCategory.Returns(nonThreatCategory);

            _nonFactoryBuilding = Substitute.For<IBuilding>();
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(_initialThreatLevel, _threatMonitor.CurrentThreatLevel);
        }

        #region ICruiserController.StartedConstruction
        [Test]
        public void StartedConstruction_NonFactoryBuilding_DoesNotEvaluate()
        {
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_nonFactoryBuilding));
            _threatEvaluator.DidNotReceiveWithAnyArgs().FindThreatLevel(value: 72);
        }

        [Test]
        public void StartedConstruction_FactoryBuilding_WrongUnitCategory_DoesNotEvaluate()
        {
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_nonThreateningFactory));
            _threatEvaluator.DidNotReceiveWithAnyArgs().FindThreatLevel(value: 97);
        }

        [Test]
        public void StartedConstruction_FactoryBuilding_RightUnitCategory_Evaluates()
        {
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_threateningFactory));
            _threatEvaluator.Received().FindThreatLevel(_threateningFactory.NumOfDrones);
        }
        #endregion ICruiserController.StartedConstruction

        [Test]
        public void FactoryDestroyed_Evaluates()
        {
            StartedConstruction_FactoryBuilding_RightUnitCategory_Evaluates();

            _threateningFactory.Destroyed += Raise.EventWith(_threateningFactory, new DestroyedEventArgs(_threateningFactory));
            _threatEvaluator.Received().FindThreatLevel(value: 0);
        }

        [Test]
        public void FactoryNumOfDronesChanged_Evaluates()
        {
            StartedConstruction_FactoryBuilding_RightUnitCategory_Evaluates();

            _threateningFactory.NumOfDrones.Returns(0);

            _threateningFactory.DroneNumChanged += Raise.EventWith(_threateningFactory, new DroneNumChangedEventArgs(_threateningFactory.NumOfDrones));
            _threatEvaluator.Received().FindThreatLevel(_threateningFactory.NumOfDrones);
        }

        [Test]
        public void NumOfDrones_FromMultipleFactoriesIsConsidered()
        {
			// Factory 1
			_cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_threateningFactory));
			_threatEvaluator.Received().FindThreatLevel(_threateningFactory.NumOfDrones);

			// Factory 2
			_cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_threateningFactory2));
            _threatEvaluator.Received().FindThreatLevel(_threateningFactory.NumOfDrones + _threateningFactory2.NumOfDrones);
        }

        [Test]
        public void ThreatLevelChanged_EmitsEvent()
        {
			Assert.AreEqual(0, _numOfEventsEmitted);

            _threatEvaluator.FindThreatLevel(117).ReturnsForAnyArgs(ThreatLevel.High);
			_cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_threateningFactory));

            Assert.AreEqual(1, _numOfEventsEmitted);
        }

		[Test]
		public void ThreatLevelNotChanged_DoesNotEmitsEvent()
		{
			Assert.AreEqual(0, _numOfEventsEmitted);

            _threatEvaluator.FindThreatLevel(117).ReturnsForAnyArgs(_initialThreatLevel);
			_cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_threateningFactory));

			Assert.AreEqual(0, _numOfEventsEmitted);
		}
    }
}
