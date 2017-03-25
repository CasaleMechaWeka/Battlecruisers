using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using BattleCruisers.Drones;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Drones
{
	[TestFixture]
	public class DroneManagerTests 
	{
		private IDroneManager _droneManager;

		private IDroneConsumer _droneConsumer1, _droneConsumer2, _droneConsumer3;

		[SetUp]
		public void TestSetup()
		{
			_droneManager = new DroneManager();

			_droneConsumer1 = new DroneConsumer(1);
			_droneConsumer2 = new DroneConsumer(2);
			_droneConsumer3 = new DroneConsumer(4);

			UnityAsserts.Assert.raiseExceptions = true;
		}

		[Test]
		public void Initialisation() 
		{
			Assert.AreEqual(0, _droneManager.NumOfDrones);
		}

		#region NumOfDrones
		[Test]
		public void NumOfDrones_Set()
		{
			_droneManager.NumOfDrones = 12;
			Assert.AreEqual(12, _droneManager.NumOfDrones);
		}

		[ExpectedException(typeof(UnityAsserts.AssertionException))]
		[Test]
		public void NumOfDrones_Set_Invalid()
		{
			_droneManager.NumOfDrones = -1;
		}
		#endregion NumOfDrones

		#region CanSupportDroneConsumer()
		[Test]
		public void CanSupportDroneConsumer_True()
		{
			_droneManager.NumOfDrones = 1;
			Assert.IsTrue(_droneManager.CanSupportDroneConsumer(_droneConsumer1));
		}

		[Test]
		public void CanSupportDroneConsumer_False()
		{
			_droneManager.NumOfDrones = 1;
			Assert.IsFalse(_droneManager.CanSupportDroneConsumer(_droneConsumer2));
		}
		#endregion CanSupportDroneConsumer()
	}
}
