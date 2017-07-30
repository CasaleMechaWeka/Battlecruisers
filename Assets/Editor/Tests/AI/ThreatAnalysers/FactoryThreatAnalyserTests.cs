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
            _threatAnalyzer = new FactoryThreatAnalyzer(_cruiser, threatCategory);
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

			Assert.AreEqual(ThreatLevel.None, _threatAnalyzer.CurrentThreatLevel);
			Assert.AreEqual(0, _numOfEventsEmitted);
		}

		[Test]
		public void StartedConstruction_FactoryBuilding_WrongUnitCategory_NoChange()
		{
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_nonThreateningFactory));

			Assert.AreEqual(ThreatLevel.None, _threatAnalyzer.CurrentThreatLevel);
			Assert.AreEqual(0, _numOfEventsEmitted);
		}

		[Test]
		public void StartedConstruction_FactoryBuilding_RightUnitCategory_ChangesThreatLevel()
		{
            _cruiser.StartedConstruction += Raise.EventWith(_cruiser, new StartedConstructionEventArgs(_threateningFactory));

            Assert.IsTrue(_threatAnalyzer.CurrentThreatLevel != ThreatLevel.None);
			Assert.AreEqual(1, _numOfEventsEmitted);
		}
		#endregion ICruiserController.StartedConstruction

		[Test]
		public void FactoryDestroyed_ChangesThreatLevel()
		{
            StartedConstruction_FactoryBuilding_RightUnitCategory_ChangesThreatLevel();

            _threateningFactory.Destroyed += Raise.EventWith(_threateningFactory, new DestroyedEventArgs(_threateningFactory));

            Assert.AreEqual(ThreatLevel.None, _threatAnalyzer.CurrentThreatLevel);
			Assert.AreEqual(2, _numOfEventsEmitted);
		}

        [Test]
        public void FatoryNumOfDronesChanged_ChangesThreatLevel()
        {
			StartedConstruction_FactoryBuilding_RightUnitCategory_ChangesThreatLevel();

            _threateningFactory.NumOfDrones.Returns(0);
            _threateningFactory.DroneNumChanged += Raise.EventWith(_threateningFactory, new DroneNumChangedEventArgs(0));

			Assert.AreEqual(ThreatLevel.None, _threatAnalyzer.CurrentThreatLevel);
			Assert.AreEqual(2, _numOfEventsEmitted);
		}

        // FELIX  Test drones are added from 2 factories
    }
}
