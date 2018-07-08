using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Tasks
{
    public class WaitForUnitConstructionTaskTests
    {
        private IInternalTask _task;
        private IFactory _factory;
        private int _numOfUnitsToBuild, _completedEventCount;
        private IBuildableWrapper<IUnit> _unitWrapper;

        private const int VALID_NUM_OF_DRONES = 1;
        private const int INVALID_NUM_OF_DRONES = 0;

        [SetUp]
        public void TestSetup()
        {
            _factory = Substitute.For<IFactory>();
            _numOfUnitsToBuild = 2;
            _completedEventCount = 0;

            _task = new WaitForUnitConstructionTask(_factory, _numOfUnitsToBuild);

            _task.Completed += (sender, e) => _completedEventCount++;

            _unitWrapper = Substitute.For<IBuildableWrapper<IUnit>>();

            _factory.IsDestroyed.Returns(false);
            _factory.BuildableState.Returns(BuildableState.Completed);
            _factory.UnitWrapper.Returns(_unitWrapper);
            _factory.NumOfDrones.Returns(VALID_NUM_OF_DRONES);
        }

        [Test]
        public void Start_FactoryCanProduceUnit()
        {
            _task.Start();
            Assert.AreEqual(0, _completedEventCount);
        }

        #region Factory cannot produce Unit
        [Test]
        public void Start_FactoryCannotProduceUnit_BecauseDestroyed_Completes()
        {
            _factory.IsDestroyed.Returns(true);
            _task.Start();
            Assert.AreEqual(1, _completedEventCount);
        }

        [Test]
        public void Start_FactoryCannotProduceUnit_BecauseNotCompleted_Completes()
        {
            _factory.BuildableState.Returns(BuildableState.InProgress);
            _task.Start();
            Assert.AreEqual(1, _completedEventCount);
        }

        [Test]
        public void Start_FactoryCannotProduceUnit_NoAssignedUnit_Completes()
        {
            _factory.UnitWrapper.Returns((IBuildableWrapper<IUnit>)null);
            _task.Start();
            Assert.AreEqual(1, _completedEventCount);
        }

        [Test]
        public void Start_FactoryCannotProduceUnit_NoAssignedDrones_Completes()
        {
            _factory.NumOfDrones.Returns(INVALID_NUM_OF_DRONES);
            _task.Start();
            Assert.AreEqual(1, _completedEventCount);
        }
        #endregion Factory cannot produce Unit

        [Test]
        public void Factory_Destroyed_Completes()
        {
            Start_FactoryCanProduceUnit();

            _factory.Destroyed += Raise.EventWith(new DestroyedEventArgs(_factory));
            Assert.AreEqual(1, _completedEventCount);
        }

        [Test]
        public void Factory_DroneStarved_Completes()
        {
            Start_FactoryCanProduceUnit();

            _factory.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(INVALID_NUM_OF_DRONES));
            Assert.AreEqual(1, _completedEventCount);
        }

        [Test]
        public void Factory_BuiltUnits_Completes()
        {
            Start_FactoryCanProduceUnit();

            ConstructUnits(_numOfUnitsToBuild);
            Assert.AreEqual(1, _completedEventCount);
        }

        [Test]
        public void Factory_DoubleBuiltUnits_DoesNotDoubleComplete()
        {
            Start_FactoryCanProduceUnit();

            // Create desired number of units to complete
            ConstructUnits(_numOfUnitsToBuild);
            Assert.AreEqual(1, _completedEventCount);

            // Create desired number again, should not complete again
            ConstructUnits(_numOfUnitsToBuild);
            Assert.AreEqual(1, _completedEventCount);
        }

        private void ConstructUnits(int numOfUnitsToBuild)
        {
            for (int i = 0; i < numOfUnitsToBuild; ++i)
            {
                _factory.CompletedBuildingUnit += Raise.EventWith(new CompletedConstructionEventArgs(buildable: null));
            }
        }
    }
}
