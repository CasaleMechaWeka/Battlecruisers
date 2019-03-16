using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.TaskProducers
{
    public class BasicTaskProducerTests : TaskProducerTestsBase
    {
        private BasicTaskProducer _taskProducer;
        private ISlotAccessor _slotAccessor;
        private IDynamicBuildOrder _buildOrder;
        private IBuildableWrapper<IBuilding> _platformSlotBuildingWrapper, _deckSlotBuildingWrapper;
        private IBuilding _platformSlotBuilding, _deckSlotBuilding;
        private BuildingKey _platformBuildingKey, _deckBuildingKey;
        private IPrioritisedTask _platformBuildingTask, _deckBuildingTask;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _tasks.IsEmpty.Returns(true);

            _platformSlotBuilding = Substitute.For<IBuilding>();
            SlotSpecification platformSlotSpecification = new SlotSpecification(SlotType.Platform, default, default);
            _platformSlotBuilding.SlotSpecification.Returns(platformSlotSpecification);
            _platformSlotBuildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _platformSlotBuildingWrapper.Buildable.Returns(_platformSlotBuilding);
            _platformBuildingKey = new BuildingKey(BuildingCategory.Ultra, "Kaffeemuehle");
            _prefabFactory.GetBuildingWrapperPrefab(_platformBuildingKey).Returns(_platformSlotBuildingWrapper);
            _platformBuildingTask = Substitute.For<IPrioritisedTask>();
            _taskFactory.CreateConstructBuildingTask(TaskPriority.Low, _platformBuildingKey).Returns(_platformBuildingTask);

            _deckSlotBuilding = Substitute.For<IBuilding>();
            SlotSpecification deckSlotSpecification = new SlotSpecification(SlotType.Deck, default, default);
            _deckSlotBuilding.SlotSpecification.Returns(deckSlotSpecification);
            _deckSlotBuildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _deckSlotBuildingWrapper.Buildable.Returns(_deckSlotBuilding);
            _deckBuildingKey = new BuildingKey(BuildingCategory.Tactical, "Hirsch");
            _prefabFactory.GetBuildingWrapperPrefab(_deckBuildingKey).Returns(_deckSlotBuildingWrapper);
            _deckBuildingTask = Substitute.For<IPrioritisedTask>();
            _taskFactory.CreateConstructBuildingTask(TaskPriority.Low, _deckBuildingKey).Returns(_deckBuildingTask);

            _slotAccessor = Substitute.For<ISlotAccessor>();
            _slotAccessor.IsSlotAvailable(_platformSlotBuilding.SlotSpecification).Returns(true);
            _slotAccessor.IsSlotAvailable(_deckSlotBuilding.SlotSpecification).Returns(false);

            _cruiser.SlotAccessor.Returns(_slotAccessor);

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
            _slotAccessor.ClearReceivedCalls();

            _buildOrder.Current.Returns(_deckBuildingKey, _platformBuildingKey);

            _tasks.IsEmptyChanged += Raise.Event();

            _slotAccessor.Received().IsSlotAvailable(_deckSlotBuilding.SlotSpecification);
            _slotAccessor.Received().IsSlotAvailable(_platformSlotBuilding.SlotSpecification);
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

            _taskProducer.DisposeManagedState();

            _tasks.IsEmptyChanged += Raise.Event();

            _buildOrder.DidNotReceive().MoveNext();
        }
    }
}
