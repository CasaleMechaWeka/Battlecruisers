using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Tests.Mock;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.Cruisers.Drones
{
    public class DroneManagerMonitorTests
    {
        private IDroneManagerMonitor _monitor;
        private IDroneManager _droneManager;
        private IVariableDelayDeferrer _deferrer;
        private IList<IDroneConsumer> _droneConsumers;
        private IDroneConsumer _idleDroneConsumer, _activeDroneConsumer;
        private int _droneNumIncreasedEventCount, _idleDronesEventCount;

        [SetUp]
        public void TestSetup()
        {
            _droneManager = Substitute.For<IDroneManager>();
            _droneManager.NumOfDrones.Returns(2);
            _droneConsumers = new List<IDroneConsumer>();
            ReadOnlyCollection<IDroneConsumer> readonlyDroneConsumers = new ReadOnlyCollection<IDroneConsumer>(_droneConsumers);
            _droneManager.DroneConsumers.Items.Returns(readonlyDroneConsumers);

            _deferrer = SubstituteFactory.CreateVariableDelayDeferrer();

            _monitor = new DroneManagerMonitor(_droneManager, _deferrer);

            _idleDroneConsumer = Substitute.For<IDroneConsumer>();
            _idleDroneConsumer.State.Returns(DroneConsumerState.Idle);

            _activeDroneConsumer = Substitute.For<IDroneConsumer>();
            _activeDroneConsumer.State.Returns(DroneConsumerState.Active);

            _droneNumIncreasedEventCount = 0;
            _monitor.DroneNumIncreased += (sender, e) => _droneNumIncreasedEventCount++;

            _idleDronesEventCount = 0;
            _monitor.IdleDrones += (sender, e) => _idleDronesEventCount++;
        }

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

        [Test]
        public void IdleDrones_DroneConsumerRemoved_TriggersCheck()
        {
            _droneManager.DroneConsumers.Changed += Raise.EventWith(new CollectionChangedEventArgs<IDroneConsumer>(ChangeType.Remove, _idleDroneConsumer));
            _deferrer.ReceivedWithAnyArgs().Defer(null, default(float));
            Assert.AreEqual(1, _idleDronesEventCount);
        }

        [Test]
        public void IdleDrones_DroneConsumerAdded_DoesNotTriggersCheck()
        {
            _droneManager.DroneConsumers.Changed += Raise.EventWith(new CollectionChangedEventArgs<IDroneConsumer>(ChangeType.Add, _idleDroneConsumer));
            _deferrer.DidNotReceiveWithAnyArgs().Defer(null, default(float));
            Assert.AreEqual(0, _idleDronesEventCount);
        }

        [Test]
        public void IdleDrones_NumerOfDronesChanged_TriggersCheck()
        {
            _droneManager.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(_droneManager.NumOfDrones - 1));
            _deferrer.ReceivedWithAnyArgs().Defer(null, default(float));
            Assert.AreEqual(1, _idleDronesEventCount);
        }

        [Test]
        public void IdleDrones_NoDroneConsumers_EmitsEvent()
        {
            TriggerIdleDronesCheck();
            Assert.AreEqual(1, _idleDronesEventCount);
        }

        [Test]
        public void IdleDrones_AllDroneConsumersIdle_EmitsEvent()
        {
            _droneConsumers.Add(_idleDroneConsumer);

            TriggerIdleDronesCheck();
            Assert.AreEqual(1, _idleDronesEventCount);
        }

        [Test]
        public void IdleDrones_SomeDroneConsumersIdle_DoesNotEmitEvent()
        {
            _droneConsumers.Add(_idleDroneConsumer);
            _droneConsumers.Add(_activeDroneConsumer);

            TriggerIdleDronesCheck();
            Assert.AreEqual(0, _idleDronesEventCount);
        }

        private void TriggerIdleDronesCheck()
        {
            _droneManager.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(_droneManager.NumOfDrones - 1));
        }
    }
}