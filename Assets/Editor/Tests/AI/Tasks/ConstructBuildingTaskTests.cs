using System;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.Tasks
{
    // FELIX  Update tests
    public class ConstructBuildingTaskTests
    {
        private ITask _task;

		private IPrefabKey _key;
		private IPrefabFactory _prefabFactory;
		private ICruiserController _cruiser;
        private ISlotWrapper _slotWrapper;
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
			_slotWrapper = Substitute.For<ISlotWrapper>();
            _cruiser = Substitute.For<ICruiserController>();
            _cruiser.SlotWrapper.Returns(_slotWrapper);

            _task = new ConstructBuildingTask(_key, _prefabFactory, _cruiser);

            _task.Completed += _task_Completed;

			_building = Substitute.For<IBuilding>();
            _building.PreferCruiserFront.Returns(true);
            _prefab = Substitute.For<IBuildableWrapper<IBuilding>>();
			_prefab.Buildable.Returns(_building);
            _slot = Substitute.For<ISlot>();

			_numOfCompletedEvents = 0;
        }

        [Test]
        public void Start_StartsConstructingBuilding()
        {
            _prefabFactory.GetBuildingWrapperPrefab(_key).Returns(_prefab);
            _cruiser.SlotWrapper.IsSlotAvailable(_building.SlotType).Returns(true);
            _cruiser.SlotWrapper.GetFreeSlot(_building.SlotType, _building.PreferCruiserFront).Returns(_slot);
            _cruiser.ConstructBuilding(_prefab.UnityObject, _slot).Returns(_building);

            _task.Start();

            _cruiser.Received().ConstructBuilding(_prefab.UnityObject, _slot);
        }

        [Test]
        public void Start_CannotAffordBuilding_Throws()
        {
			_prefabFactory.GetBuildingWrapperPrefab(_key).Returns(_prefab);
            _building.NumOfDronesRequired.Returns(4);
            IDroneManager droneManager = Substitute.For<IDroneManager>();
            droneManager.NumOfDrones = 2;
            _cruiser.DroneManager.Returns(droneManager);

            Assert.Throws<UnityAsserts.AssertionException>(() => _task.Start());
		}

		[Test]
		public void Start_NoAvailabeSlots_EmitsCompletedEvent()
		{
			_prefabFactory.GetBuildingWrapperPrefab(_key).Returns(_prefab);
			_cruiser.SlotWrapper.IsSlotAvailable(_building.SlotType).Returns(false);
            // FELIX  Update tests
            //_deferrer
            //    .WhenForAnyArgs(deferrer => deferrer.Defer(null))
            //    .Do(callInfo =>
            //    {
            //     Assert.IsTrue(callInfo.Args().Length == 1);
            //     Action actionToDefer = callInfo.Args()[0] as Action;
            //        actionToDefer.Invoke();
            //    });

            _task.Start();

            _cruiser.DidNotReceiveWithAnyArgs().ConstructBuilding(null, null);
			Assert.AreEqual(1, _numOfCompletedEvents);
		}

        [Test]
        public void BuildableCompletedEvent_CausesTaskCompletedEvent()
        {
            Start_StartsConstructingBuilding();

            _building.CompletedBuildable += Raise.Event();
			Assert.AreEqual(1, _numOfCompletedEvents);
		}

        [Test]
        public void BuildableDestroyedEvent_CausesTaskCompletedEvent()
        {
            Start_StartsConstructingBuilding();

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
