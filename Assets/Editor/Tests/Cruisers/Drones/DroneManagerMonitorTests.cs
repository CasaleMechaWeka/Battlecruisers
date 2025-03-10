using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Tests.Mock;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.Cruisers.Drones
{
    public class DroneManagerMonitorTests
    {
        private IDroneManagerMonitor _monitor;
        private IDroneManager _droneManager;
        private IDeferrer _deferrer;
        private ObservableCollection<IDroneConsumer> _droneConsumers;
        private IDroneConsumer _idleDroneConsumer, _activeDroneConsumer;
        private int _droneNumIncreasedEventCount, _idleDronesStartedEventCount, _idleDronesEndedEventCount;

        [SetUp]
        public void TestSetup()
        {
            _droneManager = Substitute.For<IDroneManager>();
            _droneManager.NumOfDrones.Returns(2);
            _droneConsumers = new ObservableCollection<IDroneConsumer>();
            ReadOnlyObservableCollection<IDroneConsumer> readonlyDroneConsumers = new ReadOnlyObservableCollection<IDroneConsumer>(_droneConsumers);
            _droneManager.DroneConsumers.Returns(readonlyDroneConsumers);

            _deferrer = SubstituteFactory.CreateDeferrer();

            _monitor = new DroneManagerMonitor(_droneManager, _deferrer);

            _idleDroneConsumer = Substitute.For<IDroneConsumer>();
            _idleDroneConsumer.State.Returns(DroneConsumerState.Idle);

            _activeDroneConsumer = Substitute.For<IDroneConsumer>();
            _activeDroneConsumer.State.Returns(DroneConsumerState.Active);

            _droneNumIncreasedEventCount = 0;
            _monitor.DroneNumIncreased += (sender, e) => _droneNumIncreasedEventCount++;

            _idleDronesStartedEventCount = 0;
            _monitor.IdleDronesStarted += (sender, e) => _idleDronesStartedEventCount++;

            _idleDronesEndedEventCount = 0;
            _monitor.IdleDronesEnded += (sender, e) => _idleDronesEndedEventCount++;
        }

        #region DroneNumChanged
        [Test]
        public void DroneNumIncreased_EmitsEvent()
        {
            _droneManager.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(_droneManager.NumOfDrones + 1));
            Assert.AreEqual(1, _droneNumIncreasedEventCount);
        }

        [Test]
        public void DroneNumDecreased_DoesNotEmitEvent()
        {
            _droneManager.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(_droneManager.NumOfDrones - 1));
            Assert.AreEqual(0, _droneNumIncreasedEventCount);
        }
        #endregion DroneNumChanged

        #region IdleDrones
        [Test]
        public void IdleDrones_DroneConsumersChanged_TriggersCheck()
        {
            _droneConsumers.Add(_idleDroneConsumer);
            _deferrer.ReceivedWithAnyArgs().Defer(null, default);
            Assert.AreEqual(1, _idleDronesStartedEventCount);         
        }

        [Test]
        public void IdleDronesStarted_NumerOfDronesChanged_TriggersCheck()
        {
            _droneManager.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(_droneManager.NumOfDrones - 1));
            _deferrer.ReceivedWithAnyArgs().Defer(null, default);
            Assert.AreEqual(1, _idleDronesStartedEventCount);
        }

        [Test]
        public void IdleDronesStarted_NoDroneConsumers_EmitsStartedEvent()
        {
            TriggerIdleDronesCheck();
            Assert.AreEqual(1, _idleDronesStartedEventCount);
        }

        [Test]
        public void IdleDronesStarted_AllDroneConsumersIdle_EmitsStartedEvent()
        {
            _droneConsumers.Add(_idleDroneConsumer);

            TriggerIdleDronesCheck();
            Assert.AreEqual(1, _idleDronesStartedEventCount);
        }

        [Test]
        public void IdleDronesStarted_SomeDroneConsumersIdle_DoesNotEmitStartedEvent()
        {
            _droneConsumers.Add(_idleDroneConsumer);
            _droneConsumers.Add(_activeDroneConsumer);
            _idleDronesStartedEventCount = 0;

            TriggerIdleDronesCheck();
            Assert.AreEqual(0, _idleDronesStartedEventCount);
        }

        [Test]
        public void IdleDronesStarted_InIdleState_DoesNotEmitEvent()
        {
            // First time emits event
            TriggerIdleDronesCheck();
            Assert.AreEqual(1, _idleDronesStartedEventCount);

            // Second time does not emit event again
            TriggerIdleDronesCheck();
            Assert.AreEqual(1, _idleDronesStartedEventCount);
        }

        [Test]
        public void IdleDronesEnded_InNonIdleState_BusyDrones_DoesNotEmitEvent()
        {
            _droneConsumers.Add(_activeDroneConsumer);
            _idleDronesEndedEventCount = 0;

            TriggerIdleDronesCheck();
            Assert.AreEqual(0, _idleDronesEndedEventCount);
        }

        [Test]
        public void IdleDronesEnded_InIdleState_BusyDrones_DoesNotEmitEvent()
        {
            // Enter idle state
            TriggerIdleDronesCheck();

            // Enter non-idle state
            _droneConsumers.Add(_activeDroneConsumer);
            TriggerIdleDronesCheck();
            Assert.AreEqual(1, _idleDronesEndedEventCount);
        }
        #endregion IdleDrones

        private void TriggerIdleDronesCheck()
        {
            _droneManager.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(_droneManager.NumOfDrones - 1));
        }
    }
}