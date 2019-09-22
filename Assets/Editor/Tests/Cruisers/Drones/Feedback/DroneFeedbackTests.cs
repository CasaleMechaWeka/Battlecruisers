using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Utils.BattleScene.Pools;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Drones.Feedback
{
    public class DroneFeedbackTests
    {
        private IDroneFeedback _feedback;
        private IDroneConsumerInfo _droneConsumerInfo;
        private IPool<IDroneController, DroneActivationArgs> _dronePool;
        private ISpawnPositionFinder _spawnPositionFinder;
        private IDroneMonitor _droneMonitor;
        private IDroneController _drone1, _drone2;
        private IDroneConsumer _droneConsumer;
        private DroneActivationArgs _activationArgs1, _activationArgs2;
        private Vector2 _spawnPosition1, _spawnPosition2;

        [SetUp]
        public void TestSetup()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _droneConsumerInfo = Substitute.For<IDroneConsumerInfo>();
            _droneConsumer = Substitute.For<IDroneConsumer>();
            _droneConsumerInfo.DroneConsumer.Returns(_droneConsumer);
            _dronePool = Substitute.For<IPool<IDroneController, DroneActivationArgs>>();
            _spawnPositionFinder = Substitute.For<ISpawnPositionFinder>();

            _droneMonitor = Substitute.For<IDroneMonitor>();
            _droneMonitor.ShouldPlaySound(Faction.Reds).Returns(true, false);

            _feedback = new DroneFeedback(_droneConsumerInfo, _dronePool, _spawnPositionFinder, _droneMonitor, Faction.Reds);

            _spawnPosition1 = new Vector2(4, 3);
            _spawnPosition2 = new Vector2(6, 7);
            _spawnPositionFinder.FindSpawnPosition(_droneConsumerInfo).Returns(_spawnPosition1, _spawnPosition2);

            _activationArgs1 = new DroneActivationArgs(_spawnPosition1, true, Faction.Reds);
            _activationArgs2 = new DroneActivationArgs(_spawnPosition2, false, Faction.Reds);

            _drone1 = Substitute.For<IDroneController>();
            _drone2 = Substitute.For<IDroneController>();

            _dronePool.GetItem(_activationArgs1).Returns(_drone1);
            _dronePool.GetItem(_activationArgs2).Returns(_drone2);
        }

        [Test]
        public void InitialState()
        {
            _droneConsumer.NumOfDrones.Returns(2);

            _feedback = new DroneFeedback(_droneConsumerInfo, _dronePool, _spawnPositionFinder, _droneMonitor, Faction.Reds);

            // Create feedback for initial number of drones
            _spawnPositionFinder.Received(2).FindSpawnPosition(_droneConsumerInfo);
            _dronePool.Received().GetItem(_activationArgs1);
            _dronePool.Received().GetItem(_activationArgs2);

            Assert.AreSame(_droneConsumer, _feedback.DroneConsumer);
        }

        [Test]
        public void DroneNumChanged_InvalidNumOfDrones_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _droneConsumer.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(-1)));
        }

        [Test]
        public void DroneNumChanged_SameNumOfDrones_DoesNothing()
        {
            _droneConsumer.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(0));

            _spawnPositionFinder.DidNotReceiveWithAnyArgs().FindSpawnPosition(default);
            _droneMonitor.DidNotReceiveWithAnyArgs().ShouldPlaySound(default);
            _dronePool.DidNotReceiveWithAnyArgs().GetItem(default);
        }

        [Test]
        public void DroneNumChanged_MoreDrones_AddsFeedback()
        {
            _droneConsumer.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(2));

            _spawnPositionFinder.Received(2).FindSpawnPosition(_droneConsumerInfo);
            _droneMonitor.Received(2).ShouldPlaySound(Faction.Reds);
            _dronePool.Received().GetItem(_activationArgs1);
            _dronePool.Received().GetItem(_activationArgs2);
        }

        [Test]
        public void DroneNumChanged_LessDrones_RemovesFeedback()
        {
            // Add 2 drones
            _droneConsumer.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(2));

            // Remove 2 drones
            _droneConsumer.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(0));
            _drone1.Received().Deactivate();
            _drone2.Received().Deactivate();
        }

        [Test]
        public void DisposeManagedState_RemovesFeedback_And_Unsubscribes()
        {
            // Add 2 drones
            _droneConsumer.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(2));
            _spawnPositionFinder.ClearReceivedCalls();
            _droneMonitor.ClearReceivedCalls();
            _dronePool.ClearReceivedCalls();

            // Disose
            _feedback.DisposeManagedState();

            _drone1.Received().Deactivate();
            _drone2.Received().Deactivate();

            // Check unsubscribed
            _droneConsumer.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(2));
            _spawnPositionFinder.DidNotReceiveWithAnyArgs().FindSpawnPosition(default);
            _droneMonitor.DidNotReceiveWithAnyArgs().ShouldPlaySound(default);
            _dronePool.DidNotReceiveWithAnyArgs().GetItem(default);
        }
    }
}