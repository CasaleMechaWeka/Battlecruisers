using BattleCruisers.AI;
using BattleCruisers.AI.Providers;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.TaskProducers
{
    public class BasicTaskProducerTests
    {
        private BasicTaskProducer _taskProducer;
        private ITaskList _tasks;
        private ICruiserController _cruiser;
        private ISlotWrapper _slotWrapper;
        private IPrefabFactory _prefabFactory;
        private ITaskFactory _taskFactory;
        private IDynamicBuildOrder _buildOrder;
        private IBuildableWrapper<IBuilding> _platformSlotBuildingWrapper, _deckSlotBuildingWrapper;
        private IBuilding _platformSlotBuilding, _deckSlotBuilding;
        private IPrefabKey _platformBuildingKey, _deckBuildingKey;
        private ITask _platformBuildingTask, _deckBuildingTask;

        [SetUp]
        public void SetuUp()
        {
            _tasks = Substitute.For<ITaskList>();
            _tasks.IsEmpty.Returns(true);

            _slotWrapper = Substitute.For<ISlotWrapper>();
            _slotWrapper.IsSlotAvailable(SlotType.Platform).Returns(true);
            _slotWrapper.IsSlotAvailable(SlotType.Deck).Returns(false);

            _cruiser = Substitute.For<ICruiserController>();
            _cruiser.SlotWrapper.Returns(_slotWrapper);
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _taskFactory = Substitute.For<ITaskFactory>();

            _platformSlotBuilding = Substitute.For<IBuilding>();
            _platformSlotBuilding.SlotType.Returns(SlotType.Platform);
            _platformSlotBuildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _platformSlotBuildingWrapper.Buildable.Returns(_platformSlotBuilding);
            _platformBuildingKey = Substitute.For<IPrefabKey>();
            _prefabFactory.GetBuildingWrapperPrefab(_platformBuildingKey).Returns(_platformSlotBuildingWrapper);
            _platformBuildingTask = Substitute.For<ITask>();
            _taskFactory.CreateConstructBuildingTask(TaskPriority.Normal, _platformBuildingKey).Returns(_platformBuildingTask);

            _deckSlotBuilding = Substitute.For<IBuilding>();
            _deckSlotBuilding.SlotType.Returns(SlotType.Deck);
            _deckSlotBuildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _deckSlotBuildingWrapper.Buildable.Returns(_deckSlotBuilding);
            _deckBuildingKey = Substitute.For<IPrefabKey>();
            _prefabFactory.GetBuildingWrapperPrefab(_deckBuildingKey).Returns(_deckSlotBuildingWrapper);
            _deckBuildingTask = Substitute.For<ITask>();
            _taskFactory.CreateConstructBuildingTask(TaskPriority.Normal, _deckBuildingKey).Returns(_deckBuildingTask);

            _buildOrder = Substitute.For<IDynamicBuildOrder>();
            _buildOrder.MoveNext().Returns(true);
            _buildOrder.Current.Returns(_platformBuildingKey);

            _taskProducer = new BasicTaskProducer(_tasks, _cruiser, _prefabFactory, _taskFactory, _buildOrder);
        }

        [Test]
        public void Constructor_CreatesFirstTask()
        {
            _tasks.Received().Add(_platformBuildingTask);
        }

        [Test]
        public void IsEmptyChanged_IsNotEmpty_DoesNothing()
        {
            _buildOrder.ClearReceivedCalls();
            _tasks.IsEmpty.Returns(false);

            _tasks.IsEmptyChanged += Raise.Event();

            _buildOrder.DidNotReceive().MoveNext();
        }

        [Test]
        public void IsEmptyChanged_IsEmpty_CreatesTask()
        {
            _tasks.ClearReceivedCalls();
            _tasks.IsEmpty.Returns(true);

            _tasks.IsEmptyChanged += Raise.Event();

            _tasks.Received().Add(_platformBuildingTask);
        }

        [Test]
        public void NoSlotForCurrentBuilding_MovesToNextBuilding()
        {
            _tasks.ClearReceivedCalls();
            _tasks.IsEmpty.Returns(true);
            _slotWrapper.ClearReceivedCalls();

            _buildOrder.Current.Returns(_deckBuildingKey, _platformBuildingKey);

            _tasks.IsEmptyChanged += Raise.Event();

            _slotWrapper.Received().IsSlotAvailable(_deckSlotBuilding.SlotType);
            _slotWrapper.Received().IsSlotAvailable(_platformSlotBuilding.SlotType);
            _tasks.Received().Add(_platformBuildingTask);
        }

        [Test]
        public void NoMoreBuildingKeys_DoesNotCreateTask_AndUnsubscribesFromTaskList()
        {
            _tasks.ClearReceivedCalls();
            _buildOrder.ClearReceivedCalls();
            _buildOrder.MoveNext().Returns(false);

            _tasks.IsEmptyChanged += Raise.Event();
            _buildOrder.Received().MoveNext();
            _tasks.DidNotReceiveWithAnyArgs().Add(taskToAdd: null);

            _buildOrder.ClearReceivedCalls();
            _tasks.IsEmptyChanged += Raise.Event();
            _buildOrder.DidNotReceive().MoveNext();
        }

        [Test]
        public void Dispose_UnsubscribesFromTaskList()
        {
            _buildOrder.ClearReceivedCalls();

            _taskProducer.Dispose();

            _tasks.IsEmptyChanged += Raise.Event();

            _buildOrder.DidNotReceive().MoveNext();
        }
    }
}
