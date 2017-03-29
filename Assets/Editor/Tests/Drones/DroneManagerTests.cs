using BattleCruisers.Drones;
using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Drones
{
	[TestFixture]
	public class DroneManagerTests 
	{
		private IDroneManager _droneManager;

		private IDroneConsumer _droneConsumer1, _droneConsumer2, _droneConsumer3, _droneConsumer4;

		[SetUp]
		public void TestSetup()
		{
			_droneManager = new DroneManager();

			_droneConsumer1 = new DroneConsumer(1);
			_droneConsumer2 = new DroneConsumer(2);
			_droneConsumer3 = new DroneConsumer(2);
			_droneConsumer4 = new DroneConsumer(4);

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

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void NumOfDrones_Set_Invalid()
		{
			_droneManager.NumOfDrones = -1;
		}

		[Test]
		public void NumOfDrones_Set_IncreaseAssigns()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.NumOfDrones = 4;
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);
		}

		[Test]
		public void NumOfDrones_Set_DecreaseFreesUp()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.NumOfDrones = 0;
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
		}

		[Test]
		public void NumOfDrones_Set_DecreaseFreesUp2()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.NumOfDrones = 1;
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
		}

		[Test]
		public void NumOfDrones_Set_DecreaseFreesUp_SoNoneActive()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.NumOfDrones = 1;
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
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

		#region AddDroneConsumer()
		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void AddDroneConsumer_CannotSupport()
		{
			_droneManager.AddDroneConsumer(_droneConsumer1);
		}

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void AddDroneConsumer_DoubleAdd()
		{
			_droneManager.NumOfDrones = 1;
			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer1);
		}

		[Test]
		public void AddDroneConsumer_ExactNumOfDrones()
		{
			_droneManager.NumOfDrones = 1;
			_droneManager.AddDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
		}

		[Test]
		public void AddDroneConsumer_TooManyDrones()
		{
			_droneManager.NumOfDrones = 2;
			_droneManager.AddDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
		}

		[Test]
		public void AddDroneConsumer_TakeFromExisting_All()
		{
			_droneManager.NumOfDrones = 2;
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
		}

		[Test]
		public void AddDroneConsumer_TakeFromExisting_All_2()
		{
			_droneManager.NumOfDrones = 2;
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.AddDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
		}

		[Test]
		public void AddDroneConsumer_TakeFromExisting_Some()
		{
			_droneManager.NumOfDrones = 4;
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);

			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
		}

		[Test]
		public void AddDroneConsumer_TakeFromExisting_Some2()
		{
			_droneManager.NumOfDrones = 5;
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);

			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);
		}

		[Test]
		public void AddDroneConsumer_TakeFromExisting_Some3()
		{
			_droneManager.NumOfDrones = 6;
			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 4
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2

			_droneManager.AddDroneConsumer(_droneConsumer4);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 0
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 2
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer4.State);  // 4
		}

		[Test]
		public void AddDroneConsumer_TakeFromExisting_Some4()
		{
			_droneManager.NumOfDrones = 7;
			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 5
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2

			_droneManager.AddDroneConsumer(_droneConsumer4);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer4.State);  // 5
		}

		[Test]
		public void AddDroneConsumer_TakeFromMultipleExisting_All()
		{
			_droneManager.NumOfDrones = 4;
			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);

			_droneManager.AddDroneConsumer(_droneConsumer4);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer4.State);
		}

		[Test]
		public void AddDroneConsumer_TakeFromMultipleExisting_All2()
		{
			_droneManager.NumOfDrones = 5;
			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);

			_droneManager.AddDroneConsumer(_droneConsumer4);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer4.State);
		}
		#endregion AddDroneConsumer()

		#region RemoveDroneConsumer()
		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void RemoveDroneConsumer_WasNotAddedFirst()
		{
			_droneManager.RemoveDroneConsumer(_droneConsumer1);
		}

		[Test]
		public void RemoveDroneConsumer_LastConsumer()
		{
			_droneManager.NumOfDrones = 1;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
		}


		[Test]
		public void RemoveDroneConsumer_HadNoDrones()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
		}

		[Test]
		public void RemoveDroneConsumer_Reassigns_OtherConsumerIdle()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
		}

		[Test]
		public void RemoveDroneConsumer_Reassigns_OtherConsumersBothIdle()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
		}

		[Test]
		public void RemoveDroneConsumer_Reassigns_OtherConsumersBothActive()
		{
			_droneManager.NumOfDrones = 5;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
		}

		[Test]
		public void RemoveDroneConsumer_Reassigns_OtherConsumersIdleActive()
		{
			_droneManager.NumOfDrones = 3;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
		}
		#endregion RemoveDroneConsumer()

		#region ToggleDroneConsumerFocus
		#region Initial state idle
		[Test]
		public void ToggleDroneConsumerFocus_IdleToActive()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
		}

		[Test]
		public void ToggleDroneConsumerFocus_IdleToFocused()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
		}

		[Test]
		public void ToggleDroneConsumerFocus_IdleToIdle()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.NumOfDrones = 1;
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
		}
		#endregion Initial state idle

		#region Initial state active
		[Test]
		public void ToggleDroneConsumerFocus_ActiveToActive()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
		}

		[Test]
		public void ToggleDroneConsumerFocus_ActiveToFocused()
		{
			_droneManager.NumOfDrones = 4;

			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer3.State);
		}

		[Test]
		public void ToggleDroneConsumerFocus_ActiveToFocused2()
		{
			_droneManager.NumOfDrones = 5;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
		}
		#endregion Initial state active

		#region Initial state focused
		[Test]
		public void ToggleDroneConsumerFocus_FocusedToFocused()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
		}

		[Test]
		public void ToggleDroneConsumerFocus_FocusedToActive()
		{
			_droneManager.NumOfDrones = 3;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
		}
		#endregion Initial state focused
		#endregion ToggleDroneConsumerFocus
	
		#region Everything
		[Test]
		public void Add_Add_Toggle_Add_Remove()
		{
			_droneManager.NumOfDrones = 6;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);  // 4
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);  // 0
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 6

			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);  // 0
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 4
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2

			_droneManager.RemoveDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);  // 0
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 6
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0
		}

		// FELIX:  Show that newly added consumer has highest priority
		// FELIX:  Show that focused consumer (when NOT in focused state) has highest priority
		// => Show consumer does NOT have highest priority when in focused state
		// FELIX:  2 consumers focused (increase drone manager num of drones after a new consuemr has been added
		// FELIX:  Add consumer when have 1 or 2 consumers focused, and reducing them to active is enough
		// => Same for when reducing them to active is NOT enough, ie, need to reduce them to idle
		#endregion Everything
	}
}
