using System;
using BattleCruisers.Cruisers.Drones;
using NUnit.Framework;
using NSubstitute;

namespace BattleCruisers.Tests.Cruisers.Drones
{
    public class DroneConsumerTests 
	{
		private IDroneConsumer _droneConsumer;
		private IDroneManager _droneManager;
		private int _droneStateChangedEmittedCount;
		private DroneStateChangedEventArgs _expectedArgs;

		[SetUp]
		public void TestSetup()
		{
			_droneManager = Substitute.For<IDroneManager>();
			_droneConsumer = new DroneConsumer(2, _droneManager);

			_droneStateChangedEmittedCount = 0;
			_droneManager.NumOfDrones.Returns(4);
		}

		[Test]
		public void Initialisation()
		{
			Assert.AreEqual(2, _droneConsumer.NumOfDronesRequired);
			Assert.AreEqual(0, _droneConsumer.NumOfDrones);
		}

		[Test]
		public void InvalidConstructorArg() 
		{
			Assert.Throws<ArgumentException>(() => new DroneConsumer(-1, _droneManager));
		}

		[Test]
		public void States()
		{
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer.State);

			_droneConsumer.NumOfDrones = _droneConsumer.NumOfDronesRequired;
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer.State);

			_droneConsumer.NumOfDrones = _droneConsumer.NumOfDronesRequired + 1;
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer.State);

			_droneConsumer.NumOfDrones = _droneManager.NumOfDrones;
			Assert.AreEqual(DroneConsumerState.AllFocused, _droneConsumer.State);
		}

		[Test]
		public void NegativeNumOfDrones()
		{
			Assert.Throws<ArgumentException>(() => _droneConsumer.NumOfDrones = -1);
		}

		[Test]
		public void InvalidNumOfDrones()
		{
			Assert.Throws<ArgumentException>(() => _droneConsumer.NumOfDrones = _droneConsumer.NumOfDronesRequired - 1);
		}

		[Test]
		public void DroneNumChangedEvent()
		{
			bool wasEventCalled = false;
			int newNumOfDrones = 7;

			_droneConsumer.DroneNumChanged += (sender, e) => 
			{
				wasEventCalled = true;
				Assert.AreEqual(newNumOfDrones, e.NewNumOfDrones);
			};

			_droneConsumer.NumOfDrones = newNumOfDrones;
			Assert.IsTrue(wasEventCalled);
		}

		[Test]
		public void DroneStateChanged_Active()
		{
			_droneConsumer.DroneStateChanged += OnDroneStateChanged;

			_expectedArgs = new DroneStateChangedEventArgs(DroneConsumerState.Idle, DroneConsumerState.Active);

			_droneConsumer.NumOfDrones = _droneConsumer.NumOfDronesRequired;

			Assert.AreEqual(1, _droneStateChangedEmittedCount);
		}

		[Test]
		public void DroneStateChanged_Idle()
		{
			DroneStateChanged_Active();

			_expectedArgs = new DroneStateChangedEventArgs(DroneConsumerState.Active, DroneConsumerState.Idle);

			_droneConsumer.NumOfDrones = 0;

			Assert.AreEqual(2, _droneStateChangedEmittedCount);
		}

		[Test]
		public void DroneStateChanged_Focused()
		{
			_droneConsumer.DroneStateChanged += OnDroneStateChanged;

			_expectedArgs = new DroneStateChangedEventArgs(DroneConsumerState.Idle, DroneConsumerState.Focused);

			_droneConsumer.NumOfDrones = _droneConsumer.NumOfDronesRequired + 1;

			Assert.AreEqual(1, _droneStateChangedEmittedCount);
		}

        [Test]
        public void NumOfSpareDrones()
        {
            Assert.AreEqual(-_droneConsumer.NumOfDronesRequired, _droneConsumer.NumOfSpareDrones);

            _droneConsumer.NumOfDrones = _droneConsumer.NumOfDronesRequired;
            Assert.AreEqual(0, _droneConsumer.NumOfSpareDrones);

            _droneConsumer.NumOfDrones += 1;
            Assert.AreEqual(1, _droneConsumer.NumOfSpareDrones);
        }

		private void OnDroneStateChanged(object sender, DroneStateChangedEventArgs e)
		{
			_droneStateChangedEmittedCount++;
			Assert.AreEqual(_droneConsumer, sender);
			Assert.AreEqual(_expectedArgs.OldState, e.OldState);
			Assert.AreEqual(_expectedArgs.NewState, e.NewState);
		}
	}
}
