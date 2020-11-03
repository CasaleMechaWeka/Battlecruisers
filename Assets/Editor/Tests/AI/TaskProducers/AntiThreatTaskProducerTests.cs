using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.TaskProducers
{
    public class AntiThreatTaskProducerTests : TaskProducerTestsBase
    {
        private ISlotAccessor _slotAccessor;
        private IDynamicBuildOrder _buildOrder;
        private IThreatMonitor _threatMonitor;
        private ISlotNumCalculator _slotNumCalculator;
        private BuildingKey _buildingKey;
        private IPrioritisedTask _task;

		[SetUp]
		public override void SetuUp()
		{
            base.SetuUp();

            _task = Substitute.For<IPrioritisedTask>();
			_slotNumCalculator = Substitute.For<ISlotNumCalculator>();

            _threatMonitor = Substitute.For<IThreatMonitor>();
            _threatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);

            _slotAccessor = Substitute.For<ISlotAccessor>();
            _slotAccessor.GetSlotCount(SlotType.Deck).Returns(5);
            _cruiser.SlotAccessor.Returns(_slotAccessor);

            _buildingKey = new BuildingKey(BuildingCategory.Tactical, "Zwackelmann");
            _buildOrder = Substitute.For<IDynamicBuildOrder>();
            _buildOrder.Current.Returns(_buildingKey);
            _buildOrder.MoveNext().Returns(true);

            new AntiThreatTaskProducer(_tasks, _cruiser, _prefabFactory, _taskFactory, _buildOrder, _threatMonitor, _slotNumCalculator);
		}

		#region ThreatLevelChanged
		[Test]
		public void ThreatLevelChanged_AlreadyHaveMetThreat_DoesNotCreateTask()
		{
            _slotNumCalculator.FindSlotNum(_threatMonitor.CurrentThreatLevel).Returns(-1);

            _threatMonitor.ThreatLevelChanged += Raise.Event();

            _slotNumCalculator.Received().FindSlotNum(_threatMonitor.CurrentThreatLevel);
            _taskFactory.DidNotReceiveWithAnyArgs().CreateConstructBuildingTask(default, null);
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

			_taskFactory.DidNotReceiveWithAnyArgs().CreateConstructBuildingTask(default, null);
		}
		#endregion ThreatLevelChanged

        [Test]
        public void TaskCompleted_HaveNotMetThreat_CreatesTask()
		{
			CreateTask(numOfSlotsForThreat: 2);  // Have 1/2 tasks

            _taskFactory.ClearReceivedCalls();

            _task.Completed += Raise.Event();

            _tasks.Received().Add(_task);
        }

		[Test]
		public void BuildOrderEmpty_DoesNotCreateTask()
		{
            _buildOrder.MoveNext().Returns(false);

			_threatMonitor.ThreatLevelChanged += Raise.Event();

            _tasks.DidNotReceiveWithAnyArgs().Add(taskToAdd: null);
		}

        private void CreateTask(int numOfSlotsForThreat)
        {
            _slotNumCalculator.FindSlotNum(_threatMonitor.CurrentThreatLevel).Returns(numOfSlotsForThreat);
			_taskFactory.CreateConstructBuildingTask(TaskPriority.Normal, _buildingKey).Returns(_task);

			_threatMonitor.ThreatLevelChanged += Raise.Event();

			_tasks.Received().Add(_task);            
        }
	}
}
