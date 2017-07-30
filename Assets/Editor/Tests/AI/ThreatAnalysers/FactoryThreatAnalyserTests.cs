using BattleCruisers.AI.ThreatAnalysers;
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
    public class FactoryThreatAnalyserTests
    {
        private IThreatAnalyser _threatAnalyzer;
        private ICruiserController _cruiser;
        private IThreatEvaluator _threatEvaluator;
        private IFactory _threateningFactory, _threateningFactory2, _nonThreateningFactory;
        private IBuilding _nonFactoryBuilding;
        private int _numOfEventsEmitted;

        [SetUp]
        public void SetuUp()
        {
            _numOfEventsEmitted = 0;
            UnitCategory threatCategory = UnitCategory.Aircraft;
            UnitCategory nonThreatCategory = UnitCategory.Naval;

            _cruiser = Substitute.For<ICruiserController>();
            _threatEvaluator = Substitute.For<IThreatEvaluator>();
            _threatEvaluator.FindThreatLevel(numOfDrones: 17).ReturnsForAnyArgs(ThreatLevel.None);
            _threatAnalyzer = new FactoryThreatAnalyzer(_cruiser, threatCategory, _threatEvaluator);
            _threatAnalyzer.ThreatLevelChanged += (sender, e) => _numOfEventsEmitted++;

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
            Assert.AreEqual(ThreatLevel.None, _threatAnalyzer.CurrentThreatLevel);
            Assert.AreEqual(0, _numOfEventsEmitted);
        }

        #region ICruiserController.StartedConstruction
        [Test]
        public void StartedConstruction_NonFactoryBuilding_DoesNotEvaluate()
        {
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_nonFactoryBuilding));
            _threatEvaluator.DidNotReceiveWithAnyArgs().FindThreatLevel(numOfDrones: 72);
        }

        [Test]
        public void StartedConstruction_FactoryBuilding_WrongUnitCategory_DoesNotEvaluate()
        {
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_nonThreateningFactory));
            _threatEvaluator.DidNotReceiveWithAnyArgs().FindThreatLevel(numOfDrones: 97);
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
            _threatEvaluator.Received().FindThreatLevel(numOfDrones: 0);
        }

        [Test]
        public void FatoryNumOfDronesChanged_Evaluates()
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
        // FELIX  event emitted
    }
}
