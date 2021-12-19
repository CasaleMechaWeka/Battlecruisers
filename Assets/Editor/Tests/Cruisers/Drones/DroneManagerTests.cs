using BattleCruisers.Cruisers.Drones;
using NUnit.Framework;
using System.Collections.Specialized;
using UnityAsserts = UnityEngine.Assertions;

//TODO fix this to work for the new drone states (added AllFocused as a state)
namespace BattleCruisers.Tests.Cruisers.Drones
{
    public class DroneManagerTests 
	{
		private IDroneManager _droneManager;
        private IDroneConsumer _droneConsumer1, _droneConsumer2, _droneConsumer3, _droneConsumer4;
        private NotifyCollectionChangedEventArgs _lastDroneConsumersChangedEventArgs;

        [SetUp]
		public void TestSetup()
		{
			_droneManager = new DroneManager();
			_droneManager.NumOfDrones = 5;
			
            ((INotifyCollectionChanged)_droneManager.DroneConsumers).CollectionChanged += (sender, e) => _lastDroneConsumersChangedEventArgs = e;

			_droneConsumer1 = new DroneConsumer(1, _droneManager);
			_droneConsumer2 = new DroneConsumer(2, _droneManager);
			_droneConsumer3 = new DroneConsumer(2, _droneManager);
			_droneConsumer4 = new DroneConsumer(4, _droneManager);
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

		[Test]
		public void NumOfDrones_Set_Invalid_Throws()
		{
            Assert.Throws<UnityAsserts.AssertionException>(() => _droneManager.NumOfDrones = -1);
		}

		[Test]
		public void NumOfDrones_Set_IncreaseAssigns()
		{
			_droneManager.NumOfDrones = 2;

            _droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer1);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);

			_droneManager.NumOfDrones = 4;
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
		}

		[Test]
		public void NumOfDrones_Set_DecreaseFreesUp()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);

			_droneManager.NumOfDrones = 0;
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
		}

		[Test]
		public void NumOfDrones_Set_DecreaseFreesUp2()
		{
			_droneManager.NumOfDrones = 2;

            _droneManager.AddDroneConsumer(_droneConsumer2);
            _droneManager.AddDroneConsumer(_droneConsumer1);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);

			_droneManager.NumOfDrones = 1;
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
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

		#region DroneNumChanged
		[Test]
		public void DroneNumChanged()
		{
			bool wasEventCalled = false;
			int newNumOfDrones = 7;

			_droneManager.DroneNumChanged += (object sender, DroneNumChangedEventArgs e) => 
			{
				wasEventCalled = true;
				Assert.AreEqual(_droneManager, sender);
				Assert.AreEqual(newNumOfDrones, e.NewNumOfDrones);
			};

			_droneManager.NumOfDrones = newNumOfDrones;

			Assert.IsTrue(wasEventCalled);
		}
		#endregion DroneNumChanged

		#region CanSupportDroneConsumer()
		[Test]
		public void CanSupportDroneConsumer_True()
		{
			_droneManager.NumOfDrones = 1;
			Assert.IsTrue(_droneManager.CanSupportDroneConsumer(_droneConsumer1.NumOfDronesRequired));
		}

		[Test]
		public void CanSupportDroneConsumer_False()
		{
			_droneManager.NumOfDrones = 1;
			Assert.IsFalse(_droneManager.CanSupportDroneConsumer(_droneConsumer2.NumOfDronesRequired));
		}
		#endregion CanSupportDroneConsumer()

		#region AddDroneConsumer()
		[Test]
        public void AddDroneConsumer_CannotSupport_Throws()
		{
            Assert.Throws<UnityAsserts.AssertionException>(() => _droneManager.AddDroneConsumer(_droneConsumer1));
		}

		[Test]
        public void AddDroneConsumer_DoubleAdd_Throws()
		{
			_droneManager.NumOfDrones = 1;
			_droneManager.AddDroneConsumer(_droneConsumer1);
            Assert.Throws<UnityAsserts.AssertionException>(() => _droneManager.AddDroneConsumer(_droneConsumer1));
		}

		[Test]
		public void AddDroneConsumer_ExactNumOfDrones()
		{
			_droneManager.NumOfDrones = 1;

            _droneManager.AddDroneConsumer(_droneConsumer1);

            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
            Assert.IsTrue(_lastDroneConsumersChangedEventArgs.NewItems.Contains(_droneConsumer1));
            Assert.IsTrue(_droneManager.DroneConsumers.Contains(_droneConsumer1));
        }

        [Test]
		public void AddDroneConsumer_TooManyDrones()
		{
			_droneManager.NumOfDrones = 2;
			_droneManager.AddDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
		}

		[Test]
		public void AddDroneConsumer_NoSpareDrones()
		{
			_droneManager.NumOfDrones = 2;
			_droneManager.AddDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2

			_droneManager.AddDroneConsumer(_droneConsumer3);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
		}

        [Test]
        public void AddDroneConsumer_NotEnoughSpareDrones()
        {
            _droneManager.NumOfDrones = 3;
            _droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 3

            _droneManager.AddDroneConsumer(_droneConsumer3);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 3
        }

		[Test]
		public void AddDroneConsumer_EnoughSpareDrones()
		{
			_droneManager.NumOfDrones = 4;
			_droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 4

			_droneManager.AddDroneConsumer(_droneConsumer3);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
		}

        [Test]
        public void AddDroneConsumer_TooManySpareDrones()
        {
            _droneManager.NumOfDrones = 6;
            _droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 6

            _droneManager.AddDroneConsumer(_droneConsumer3);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 4
        }

        [Test]
        public void AddDroneConsumer_ExistingActiveAndActive_NoSpareDrones()
        {
            _droneManager.NumOfDrones = 6;
            _droneManager.AddDroneConsumer(_droneConsumer4);
            _droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer4.State);  // 4
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2

            _droneManager.AddDroneConsumer(_droneConsumer3);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer4.State);  // 4
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0
        }

        [Test]
        public void AddDroneConsumer_ExistingFocusedAndActive_NotEnoughSpareDrones()
        {
            _droneManager.NumOfDrones = 7;
            _droneManager.AddDroneConsumer(_droneConsumer4);
            _droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer4.State);  // 5
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2

            _droneManager.AddDroneConsumer(_droneConsumer3);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer4.State);  // 5
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0
        }

        [Test]
        public void AddDroneConsumer_ExistingFocusedAndActive_EnoughSpareDrones()
        {
            _droneManager.NumOfDrones = 8;
            _droneManager.AddDroneConsumer(_droneConsumer4);
			_droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer4.State);  // 6
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2

            _droneManager.AddDroneConsumer(_droneConsumer3);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer4.State);  // 4
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2
        }

        [Test]
        public void AddDroneConsumer_ExistingFocusedAndActive_TooManySpareDrones()
        {
            _droneManager.NumOfDrones = 10;
            _droneManager.AddDroneConsumer(_droneConsumer4);
            _droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer4.State);  // 8
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2

            _droneManager.AddDroneConsumer(_droneConsumer3);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer4.State);  // 6
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2
        }
		#endregion AddDroneConsumer()

		#region RemoveDroneConsumer()
		[Test]
        public void RemoveDroneConsumer_WasNotAddedFirst_Throws()
		{
            Assert.Throws<UnityAsserts.AssertionException>(() => _droneManager.RemoveDroneConsumer(_droneConsumer1));
		}

		[Test]
		public void RemoveDroneConsumer_LastConsumer()
		{
			_droneManager.NumOfDrones = 1;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);

            Assert.IsTrue(_lastDroneConsumersChangedEventArgs.OldItems.Contains(_droneConsumer1));
            Assert.IsFalse(_droneManager.DroneConsumers.Contains(_droneConsumer1));
        }

		[Test]
		public void RemoveDroneConsumer_HadNoDrones()
		{
			_droneManager.NumOfDrones = 2;

            _droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer1);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer1);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
		}

		[Test]
		public void RemoveDroneConsumer_Reassigns_OtherConsumerIdle()
		{
			_droneManager.NumOfDrones = 2;

            _droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer1);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
		}

		[Test]
		public void RemoveDroneConsumer_Reassigns_OtherConsumersBothIdle()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer3);
            _droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);
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

            _droneManager.AddDroneConsumer(_droneConsumer3);
			_droneManager.AddDroneConsumer(_droneConsumer1);
            _droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);

			_droneManager.RemoveDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
		}
		#endregion RemoveDroneConsumer()

		#region ToggleDroneConsumerFocus()
		#region Initial state idle
		[Test]
		public void ToggleDroneConsumerFocus_IdleToActive()
		{
			_droneManager.NumOfDrones = 2;

            _droneManager.AddDroneConsumer(_droneConsumer3);
			_droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
		}

		[Test]
		public void ToggleDroneConsumerFocus_IdleToFocused()
		{
			_droneManager.NumOfDrones = 2;

            _droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer1);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer1);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
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

		[Test]
		public void ToggleDroneConsumerFocus_ActiveToFocused3()
		{
			_droneManager.NumOfDrones = 9;

			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 7
			Assert.AreEqual(7, _droneConsumer2.NumOfDrones);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);  // 0
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer3.State);  // 9
			Assert.AreEqual(9, _droneConsumer3.NumOfDrones);
		}
		#endregion Initial state active

		#region Initial state focused
		[Test]
		public void ToggleDroneConsumerFocus_FocusedToFocused()
		{
			_droneManager.NumOfDrones = 2;

			_droneManager.AddDroneConsumer(_droneConsumer1);
            _droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer1);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);
		}

		[Test]
		public void ToggleDroneConsumerFocus_FocusedToFocused2()
		{
			_droneManager.NumOfDrones = 9;

			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 7
			Assert.AreEqual(7, _droneConsumer2.NumOfDrones);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 9
			Assert.AreEqual(9, _droneConsumer2.NumOfDrones);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 7
			Assert.AreEqual(7, _droneConsumer2.NumOfDrones);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);  // 0
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer3.State);  // 9
			Assert.AreEqual(9, _droneConsumer3.NumOfDrones);
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

		[Test]
		public void ToggleDroneConsumerFocus_ToActive_SetsMaxPriority()
		{
			_droneManager.NumOfDrones = 2;

            _droneManager.AddDroneConsumer(_droneConsumer3);
			_droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

			_droneManager.NumOfDrones = 5;
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);
		}

		[Test]
		public void ToggleDroneConsumerFocus_ToFocused_SetsMaxPriority()
		{
			_droneManager.NumOfDrones = 4;

			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 4
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0

			_droneManager.NumOfDrones = 5;
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 5
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0
		}
        #endregion ToggleDroneConsumerFocus()

        #region HasDroneConsumer
        [Test]
        public void HasDroneConsumer_ConsumerAdded_ReturnsTrue()
        {
            _droneManager.NumOfDrones = 12;
            _droneManager.AddDroneConsumer(_droneConsumer1);

            Assert.IsTrue(_droneManager.HasDroneConsumer(_droneConsumer1));
        }

        [Test]
        public void HasDroneConsumer_ConsumerNeverAdded_False()
        {
            Assert.IsFalse(_droneManager.HasDroneConsumer(_droneConsumer1));
        }

        [Test]
        public void HasDroneConsumer_ConsumerRemoved_False()
        {
            _droneManager.NumOfDrones = 12;
            _droneManager.AddDroneConsumer(_droneConsumer1);
            _droneManager.RemoveDroneConsumer(_droneConsumer1);

            Assert.IsFalse(_droneManager.HasDroneConsumer(_droneConsumer1));
        }
        #endregion HasDroneConsumer

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
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 6
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);  // 0

			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 6
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);  // 0
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0

			_droneManager.RemoveDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);  // 4
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);  // 0
		}

		[Test]
		public void Add_Add_ExtraDrones_FirstAddedHasHighestPriority()
		{
			_droneManager.NumOfDrones = 4;

			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2

			_droneManager.NumOfDrones = 6;
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 4
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2
		}

		[Test]
		public void Add_Add_Toggle_ExtraDrones_FocusedHasHighestPriority()
		{
			_droneManager.NumOfDrones = 5;

            _droneManager.AddDroneConsumer(_droneConsumer2);
            _droneManager.AddDroneConsumer(_droneConsumer3);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 3
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2

            _droneManager.ToggleDroneConsumerFocus(_droneConsumer3);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);  // 0
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer3.State);  // 5

			_droneManager.NumOfDrones = 6;
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);  // 0
			Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer3.State);  // 6
            Assert.AreEqual(6, _droneConsumer3.NumOfDrones);
		}

		[Test]
		public void Add_Add_Add_Add_Toggle_ExtraDrones()
		{
			_droneManager.NumOfDrones = 9;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			_droneManager.AddDroneConsumer(_droneConsumer4);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);  // 1
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer4.State);  // 4

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 9
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);  // 0
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer4.State);  // 0

			_droneManager.NumOfDrones = 11;
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 11
			Assert.AreEqual(11, _droneConsumer2.NumOfDrones);
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);  // 0
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer4.State);  // 0
		}

		[Test]
		public void Add_Add_Add_Toggle_Remove()
		{
			_droneManager.NumOfDrones = 5;

			_droneManager.AddDroneConsumer(_droneConsumer1);
			_droneManager.AddDroneConsumer(_droneConsumer2);
			_droneManager.AddDroneConsumer(_droneConsumer3);
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer1.State);  // 1
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);  // 2
			Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2

			_droneManager.ToggleDroneConsumerFocus(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer2.State);  // 5
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer1.State);  // 0
			Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer3.State);  // 0

			_droneManager.RemoveDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);  // 3
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer3.State);  // 2
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer2.State);  // 0
		}

        /// <summary>
        /// Checks a case that was a bug:
        /// 1. Start constructing building costing 6 (have 6 drones)
        /// 2. Lose drone station => have 4 drones, building paused
        /// 3. Try to start building a drone station (costing 4 drones) 
        ///     => Error!  No freed drones, as previously freed drones
        ///     when we paused the building were "lost"  (ie, could only
        ///     be freed once :) ).
        /// </summary>
        [Test]
        public void Add_LoseDrones_CurrentConsumerPaused_AddNewBecomesFocused_GainDrones()
        {
            _droneManager.NumOfDrones = 4;

            _droneManager.AddDroneConsumer(_droneConsumer4);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer4.State);

            // Simulate losing 2 drones
            _droneManager.NumOfDrones = 2;
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer4.State);

            _droneManager.AddDroneConsumer(_droneConsumer1);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);

            // Focused drone should be highest priority and should get all new drones
            _droneManager.NumOfDrones = 10;
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer4.State);
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer1.State);
            Assert.AreEqual(10, _droneConsumer1.NumOfDrones);
        }

        [Test]
        public void Add_LoseDrones_CurrentConsumerPaused_AddNewBecomesActive_GainDrones()
        {
            _droneManager.NumOfDrones = 4;

            _droneManager.AddDroneConsumer(_droneConsumer4);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer4.State);

            // Simulate losing 2 drones
            _droneManager.NumOfDrones = 2;
            Assert.AreEqual(DroneConsumerState.Idle, _droneConsumer4.State);

            _droneManager.AddDroneConsumer(_droneConsumer2);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);

            // Added drone is active, so should NOT have become highest priority consumer.
            _droneManager.NumOfDrones = 10;
            Assert.AreEqual(DroneConsumerState.Focused, _droneConsumer4.State);
			Assert.AreEqual(8, _droneConsumer4.NumOfDrones);
            Assert.AreEqual(DroneConsumerState.Active, _droneConsumer2.State);
        }
		#endregion Everything
	}
}
