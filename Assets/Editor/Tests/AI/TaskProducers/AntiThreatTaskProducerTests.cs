using System.Collections.Generic;
using BattleCruisers.AI;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.TaskProducers
{
    public class AntiThreatTaskProducerTests
	{
		private ITaskList _tasks;
		private ICruiserController _cruiser;
        private ISlotWrapper _slotWrapper;
		private IPrefabFactory _prefabFactory;
		private ITaskFactory _taskFactory;
        private IList<IPrefabKey> _buildOrder;
        private IThreatMonitor _threatMonitor;
        private ISlotNumCalculator _slotNumCalculator;
        private IPrefabKey _buildingKey;
        private ITask _task;

		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;
   
            _tasks = Substitute.For<ITaskList>();
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _taskFactory = Substitute.For<ITaskFactory>();
            _task = Substitute.For<ITask>();
			_slotNumCalculator = Substitute.For<ISlotNumCalculator>();

            _threatMonitor = Substitute.For<IThreatMonitor>();
            _threatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);

            _slotWrapper = Substitute.For<ISlotWrapper>();
            _slotWrapper.GetSlotCount(SlotType.Deck).Returns(5);
            _cruiser = Substitute.For<ICruiserController>();
            _cruiser.SlotWrapper.Returns(_slotWrapper);

            _buildingKey = Substitute.For<IPrefabKey>();
            _buildOrder = new List<IPrefabKey>();
            _buildOrder.Add(_buildingKey);

            new AntiThreatTaskProducer(_tasks, _cruiser, _prefabFactory, _taskFactory, _buildOrder, _threatMonitor, _slotNumCalculator);
		}

		#region ThreatLevelChanged
		[Test]
		public void ThreatLevelChanged_AlreadyHaveMetThreat_DoesNotCreateTask()
		{
            _slotNumCalculator.FindSlotNum(_threatMonitor.CurrentThreatLevel).Returns(-1);

            _threatMonitor.ThreatLevelChanged += Raise.Event();

            _slotNumCalculator.Received().FindSlotNum(_threatMonitor.CurrentThreatLevel);
            _taskFactory.DidNotReceiveWithAnyArgs().CreateConstructBuildingTask(default(TaskPriority), null);
		}

		[Test]
		public void ThreatLevelChanged_HaveNotMetThreat_CreatesTask()
		{
            CreateTask(numOfSlotsForThreat: 99);
		}

		[Test]
		public void ThreatLevelChanged_AlreadyHaveCurrentTask_DoesNotCreateTask()
		{
            CreateTask(numOfSlotsForThreat: 99);

            _taskFactory.ClearReceivedCalls();

			_threatMonitor.ThreatLevelChanged += Raise.Event();

			_taskFactory.DidNotReceiveWithAnyArgs().CreateConstructBuildingTask(default(TaskPriority), null);
		}
		#endregion ThreatLevelChanged

        [Test]
        public void TaskCompleted_HaveNotMetThreat_CreatesTask()
		{
            _buildOrder.Add(_buildingKey);
            Assert.IsTrue(_buildOrder.Count == 2);

			CreateTask(numOfSlotsForThreat: 2);  // Have 1/2 tasks

            _taskFactory.ClearReceivedCalls();

            _task.Completed += Raise.Event();

            _tasks.Received().Add(_task);
        }

		[Test]
		public void BuildOrderTooShort_Throws()
		{
            CreateTask(numOfSlotsForThreat: 2);  // Have 1/2 tasks
            Assert.IsTrue(_buildOrder.Count == 1);

            Assert.Throws<UnityAsserts.AssertionException>(() => _task.Completed += Raise.Event());
		}

        private void CreateTask(int numOfSlotsForThreat)
        {
            _slotNumCalculator.FindSlotNum(_threatMonitor.CurrentThreatLevel).Returns(numOfSlotsForThreat);
			_taskFactory.CreateConstructBuildingTask(TaskPriority.High, _buildingKey).Returns(_task);

			_threatMonitor.ThreatLevelChanged += Raise.Event();

			_tasks.Received().Add(_task);            
        }
	}
}
