using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.TaskProducers
{
    public class PostFactoryTaskProducerTests : TaskProducerTestsBase
    {
        private PostFactoryTaskProducer _taskProducer;
        private IPrioritisedTask _waitForUnitsTask;
        private IFactory _factory;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _factory = Substitute.For<IFactory>();
            _waitForUnitsTask = Substitute.For<IPrioritisedTask>();

            _taskFactory
                .CreateWaitForUnitConstructionTask(TaskPriority.Low, _factory)
                .Returns(_waitForUnitsTask);

            _taskProducer = new PostFactoryTaskProducer(_tasks, _cruiser, _taskFactory, _prefabFactory);
        }

        [Test]
        public void StartedConstruction_Factory_CreatesTask()
        {
            StartConstructingBuilding(_factory);

            _taskFactory.Received().CreateWaitForUnitConstructionTask(TaskPriority.Low, _factory);
            _tasks.Received().Add(_waitForUnitsTask);
        }

        [Test]
        public void StartedConstruction_NonFactory_DoesNotCreatesTask()
        {
            IBuilding nonFactoryBuilding = Substitute.For<IBuilding>();

            StartConstructingBuilding(nonFactoryBuilding);

            _taskFactory.DidNotReceiveWithAnyArgs().CreateWaitForUnitConstructionTask(default(TaskPriority), null);
            _tasks.DidNotReceiveWithAnyArgs().Add(null);
        }

        [Test]
        public void DisposeManagedState_Unsubscribes()
        {
            _taskProducer.DisposeManagedState();

            // Cruiser builds factory
            StartConstructingBuilding(_factory);

            // Task producer does no thing, as we no longer subscribe to the cruiser event
            _taskFactory.DidNotReceiveWithAnyArgs().CreateWaitForUnitConstructionTask(TaskPriority.Low, _factory);
            _tasks.DidNotReceiveWithAnyArgs().Add(_waitForUnitsTask);
        }

        private void StartConstructingBuilding(IBuilding building)
        {
            _cruiser.StartedConstruction += Raise.EventWith(new StartedConstructionEventArgs(building));
        }
    }
}
