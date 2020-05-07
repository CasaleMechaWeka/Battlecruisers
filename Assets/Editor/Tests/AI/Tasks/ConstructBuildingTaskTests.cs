using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Tasks
{
    public class ConstructBuildingTaskTests
    {
        private ITask _task;

		private IPrefabKey _key;
		private IPrefabFactory _prefabFactory;
		private ICruiserController _cruiser;
        private ISlotAccessor _slotAccessor;
        private IBuildableWrapper<IBuilding> _prefab;
        private IBuilding _building;
        private ISlot _slot;

        private int _numOfCompletedEvents;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _key = Substitute.For<IPrefabKey>();
            _prefabFactory = Substitute.For<IPrefabFactory>();
			_slotAccessor = Substitute.For<ISlotAccessor>();
            _cruiser = Substitute.For<ICruiserController>();
            _cruiser.IsAlive.Returns(true);
            _cruiser.SlotAccessor.Returns(_slotAccessor);

            _task = new ConstructBuildingTask(_key, _prefabFactory, _cruiser);

            _task.Completed += _task_Completed;

			_building = Substitute.For<IBuilding>();
            SlotSpecification slotSpecification = new SlotSpecification(SlotType.Platform, default, default);
            _building.SlotSpecification.Returns(slotSpecification);
            _prefab = Substitute.For<IBuildableWrapper<IBuilding>>();
			_prefab.Buildable.Returns(_building);
            _slot = Substitute.For<ISlot>();

			_numOfCompletedEvents = 0;
        }

        [Test]
        public void Start_StartsConstructingBuilding_ReturnsTrue()
        {
            _prefabFactory.GetBuildingWrapperPrefab(_key).Returns(_prefab);
            _cruiser.SlotAccessor.IsSlotAvailable(_building.SlotSpecification).Returns(true);
            _cruiser.SlotAccessor.GetFreeSlot(_building.SlotSpecification).Returns(_slot);
            _cruiser.ConstructBuilding(_prefab.UnityObject, _slot).Returns(_building);

            bool haveStarted = _task.Start();

            Assert.IsTrue(haveStarted);
            _cruiser.Received().ConstructBuilding(_prefab.UnityObject, _slot);
        }

        [Test]
        public void Start_CannotAffordBuilding_ReturnsFalse()
        {
            _cruiser.SlotAccessor.IsSlotAvailable(_building.SlotSpecification).Returns(true);
			_prefabFactory.GetBuildingWrapperPrefab(_key).Returns(_prefab);
            _building.NumOfDronesRequired.Returns(4);
            IDroneManager droneManager = Substitute.For<IDroneManager>();
            droneManager.NumOfDrones = 2;
            _cruiser.DroneManager.Returns(droneManager);

            bool haveStarted = _task.Start();

            Assert.IsFalse(haveStarted);
            _cruiser.DidNotReceiveWithAnyArgs().ConstructBuilding(null, null);
            Assert.AreEqual(0, _numOfCompletedEvents);
        }

        [Test]
        public void Start_CruiserIsNotAlive_ReturnsFalse()
        {
            _prefabFactory.GetBuildingWrapperPrefab(_key).Returns(_prefab);
            _cruiser.IsAlive.Returns(false);

            bool haveStarted = _task.Start();

            Assert.IsFalse(haveStarted);
            _cruiser.DidNotReceiveWithAnyArgs().ConstructBuilding(null, null);
            Assert.AreEqual(0, _numOfCompletedEvents);
        }

        [Test]
		public void Start_NoAvailabeSlots_ReturnsFalse()
		{
			_prefabFactory.GetBuildingWrapperPrefab(_key).Returns(_prefab);
			_cruiser.SlotAccessor.IsSlotAvailable(_building.SlotSpecification).Returns(false);

            bool haveStarted = _task.Start();

            Assert.IsFalse(haveStarted);
            _cruiser.DidNotReceiveWithAnyArgs().ConstructBuilding(null, null);
			Assert.AreEqual(0, _numOfCompletedEvents);
		}

        [Test]
        public void BuildableCompletedEvent_CausesTaskCompletedEvent()
        {
            Start_StartsConstructingBuilding_ReturnsTrue();

            _building.CompletedBuildable += Raise.Event();
			Assert.AreEqual(1, _numOfCompletedEvents);
		}

        [Test]
        public void BuildableDestroyedEvent_CausesTaskCompletedEvent()
        {
            Start_StartsConstructingBuilding_ReturnsTrue();

            _building.Destroyed += Raise.EventWith(new DestroyedEventArgs(_building));
			Assert.AreEqual(1, _numOfCompletedEvents);
        }

        private void _task_Completed(object sender, EventArgs e)
        {
            _numOfCompletedEvents++;
        }

        private void ResetEventCounter()
        {
            _numOfCompletedEvents = 0;
        }
    }
}
