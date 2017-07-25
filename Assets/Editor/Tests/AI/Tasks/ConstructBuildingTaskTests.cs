//using System;
//using BattleCruisers.AI.Tasks;
//using BattleCruisers.Buildables;
//using BattleCruisers.Buildables.Buildings;
//using BattleCruisers.Cruisers;
//using BattleCruisers.Data.PrefabKeys;
//using BattleCruisers.Fetchers;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.AI.Tasks
//{
//    public class ConstructBuildingTaskTests
//    {
//        private ITask _task;

//		private IPrefabKey _key;
//		private IPrefabFactory _prefabFactory;
//		private ICruiserController _cruiser;
//        private IBuildableWrapper<IBuilding> _prefab;
//        private IBuilding _building;
//        private ISlot _slot;

//        private int _numOfCompletedEvents;

//        [SetUp]
//        public void SetuUp()
//        {
//            _key = Substitute.For<IPrefabKey>();
//            _prefabFactory = Substitute.For<IPrefabFactory>();
//            _cruiser = Substitute.For<ICruiserController>();

//            _task = new ConstructBuildingTask(TaskPriority.High, _key, _prefabFactory, _cruiser);

//            _task.Completed += _task_Completed;

//			_building = Substitute.For<IBuilding>();
//            _prefab = Substitute.For<IBuildableWrapper<IBuilding>>();
//			_prefab.Buildable.Returns(_building);
//            _slot = Substitute.For<ISlot>();

//			_numOfCompletedEvents = 0;
//        }

//        [Test]
//        public void Start_StartsConstructingBuilding()
//        {
//            _prefabFactory.GetBuildingWrapperPrefab(_key).Returns(_prefab);
//            _cruiser.IsSlotAvailable(_building.SlotType).Returns(true);
//            _cruiser.GetFreeSlot(_building.SlotType).Returns(_slot);
//            _cruiser.ConstructBuilding(_prefab.UnityObject, _slot).Returns(_building);

//            _task.Start();

//            _cruiser.Received().ConstructBuilding(_prefab.UnityObject, _slot);
//        }

//		[Test]
//		public void Start_NoAvailabeSlots_EmitsCompletedEvent()
//		{
//			_prefabFactory.GetBuildingWrapperPrefab(_key).Returns(_prefab);
//			_cruiser.IsSlotAvailable(_building.SlotType).Returns(false);

//			_task.Start();

//            _cruiser.DidNotReceiveWithAnyArgs().ConstructBuilding(null, null);
//			Assert.AreEqual(1, _numOfCompletedEvents);
//		}

//		[Test]
//		public void Start_AlreadyCompleted_EmitsCompletedEvent()
//		{
//            BuildableCompleted_EmitsCompletedEvent();

//            ResetEventCounter();
//            _cruiser.ClearReceivedCalls();

//            _task.Start();

//            _cruiser.DidNotReceiveWithAnyArgs().ConstructBuilding(null, null);
//			Assert.AreEqual(1, _numOfCompletedEvents);
//		}


//		[Test]
//		public void Start_InProgressTask_DoesNothing()
//        {
//            Start_StartsConstructingBuilding();

//            _cruiser.ClearReceivedCalls();
//            _task.Start();
//            _cruiser.DidNotReceiveWithAnyArgs().ConstructBuilding(null, null);
//        }

//		[Test]
//        public void BuildableCompleted_EmitsCompletedEvent()
//        {
//            Start_StartsConstructingBuilding();

//            _building.CompletedBuildable += Raise.Event();
//            Assert.AreEqual(1, _numOfCompletedEvents);
//        }

//        private void _task_Completed(object sender, EventArgs e)
//        {
//            _numOfCompletedEvents++;
//        }

//        private void ResetEventCounter()
//        {
//            _numOfCompletedEvents = 0;
//        }
//    }
//}
