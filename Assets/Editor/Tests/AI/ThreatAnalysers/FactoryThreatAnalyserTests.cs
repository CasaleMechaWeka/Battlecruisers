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
        private IFactory _threateningFactory, _nonThreateningFactory;
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
            _threatEvaluator.FindThreatLevel(numOfDrones: 17).ReturnsForAnyArgs(ThreatLevel.High);
            _threatAnalyzer = new FactoryThreatAnalyzer(_cruiser, threatCategory, _threatEvaluator);
            _threatAnalyzer.ThreatLevelChanged += (sender, e) => _numOfEventsEmitted++;

            _threateningFactory = Substitute.For<IFactory>();
            _threateningFactory.NumOfDrones.Returns(2);
            _threateningFactory.UnitCategory.Returns(threatCategory);

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
        public void StartedConstruction_NonFactoryBuilding_NoChange()
        {
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_nonFactoryBuilding));
			Assert.AreEqual(0, _numOfEventsEmitted);
            _threatEvaluator.DidNotReceiveWithAnyArgs().FindThreatLevel(numOfDrones: 72);
		}

		[Test]
		public void StartedConstruction_FactoryBuilding_WrongUnitCategory_NoChange()
		{
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_nonThreateningFactory));
			Assert.AreEqual(0, _numOfEventsEmitted);
			_threatEvaluator.DidNotReceiveWithAnyArgs().FindThreatLevel(numOfDrones: 97);
		}

		[Test]
		public void StartedConstruction_FactoryBuilding_RightUnitCategory_ChangesThreatLevel()
		{
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_threateningFactory));
			Assert.AreEqual(1, _numOfEventsEmitted);
            _threatEvaluator.Received().FindThreatLevel(_threateningFactory.NumOfDrones);
		}
		#endregion ICruiserController.StartedConstruction

		[Test]
		public void FactoryDestroyed_ChangesThreatLevel()
		{
            StartedConstruction_FactoryBuilding_RightUnitCategory_ChangesThreatLevel();

			// Need to change threat level for event to be emitted
			_threatEvaluator.FindThreatLevel(numOfDrones: 79).ReturnsForAnyArgs(ThreatLevel.None);

			_threateningFactory.Destroyed += Raise.EventWith(_threateningFactory, new DestroyedEventArgs(_threateningFactory));
			
            Assert.AreEqual(2, _numOfEventsEmitted);
            _threatEvaluator.Received().FindThreatLevel(numOfDrones: 0);
		}

        [Test]
        public void FatoryNumOfDronesChanged_ChangesThreatLevel()
        {
			StartedConstruction_FactoryBuilding_RightUnitCategory_ChangesThreatLevel();

            _threateningFactory.NumOfDrones.Returns(0);
			// Need to change threat level for event to be emitted
			_threatEvaluator.FindThreatLevel(numOfDrones: 79).ReturnsForAnyArgs(ThreatLevel.None);

			_threateningFactory.DroneNumChanged += Raise.EventWith(_threateningFactory, new DroneNumChangedEventArgs(newNumOfDrones: 0));

			Assert.AreEqual(2, _numOfEventsEmitted);
			_threatEvaluator.Received().FindThreatLevel(_threateningFactory.NumOfDrones);
		}

        // FELIX  Test drones are added from 2 factories
    }
}
