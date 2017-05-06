using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using BattleCruisers.Drones;

namespace BattleCruisers.Tests.Drones
{
	public class DroneConsumerTests 
	{
		private IDroneConsumer _droneConsumer;
		private int _droneStateChangedEmittedCount;
		private DroneStateChangedEventArgs _expectedArgs;

		[SetUp]
		public void TestSetup()
		{
			_droneConsumer = new DroneConsumer(2);
			_droneStateChangedEmittedCount = 0;
		}

		[Test]
		public void Initialisation()
		{
			Assert.AreEqual(2, _droneConsumer.NumOfDronesRequired);
			Assert.AreEqual(0, _droneConsumer.NumOfDrones);
		}

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void InvalidConstructorArg() 
		{
			new DroneConsumer(-1);
		}

		[Test]
		public void States()
		{
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer.State);

			_droneConsumer.NumOfDrones = _droneConsumer.NumOfDronesRequired;
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer.State);

			_droneConsumer.NumOfDrones = _droneConsumer.NumOfDronesRequired + 1;
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer.State);
		}

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void NegativeNumOfDrones()
		{
			_droneConsumer.NumOfDrones = -1;
		}

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void InvalidNumOfDrones()
		{
			_droneConsumer.NumOfDrones = _droneConsumer.NumOfDronesRequired - 1;
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

		private void OnDroneStateChanged(object sender, DroneStateChangedEventArgs e)
		{
			_droneStateChangedEmittedCount++;
			Assert.AreEqual(_droneConsumer, sender);
			Assert.AreEqual(_expectedArgs.OldState, e.OldState);
			Assert.AreEqual(_expectedArgs.NewState, e.NewState);
		}
	}
}
