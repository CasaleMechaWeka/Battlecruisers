using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Drones
{
	public enum DroneConsumerState
	{
		Normal,		// Has the exact number of drones required
		Focused,	// Has more than the number of drones required
		Idle		// Has no drones
	}

	public class DroneNumChangedEventArgs : EventArgs
	{
		public int NewNumOfDrones { get; private set; }
		public DroneNumChangedEventArgs(int newNumOfDrones)
		{
			NewNumOfDrones = newNumOfDrones;
		}
	}

	// Building, unit, cruiser.  Anything that needs to be repaired, in which case
	// NumOfDronesRequired should be 1.
	public interface IDroneConsumer
	{
		int NumOfDrones { get; set; }
		int NumOfDronesRequired { get; }
		DroneConsumerState State { get; }
		event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;
	}

	public class DroneConsumer : IDroneConsumer
	{
		private int _numOfDrones;
		public int NumOfDrones
		{
			get { return _numOfDrones; }
			set
			{
				if (value != _numOfDrones)
				{
					_numOfDrones = value;
					if (DroneNumChanged != null)
					{
						DroneNumChanged.Invoke(this, new DroneNumChangedEventArgs(_numOfDrones));
					}
					State = FindDroneState(_numOfDrones, NumOfDronesRequired);
				}
			}
		}

		public int NumOfDronesRequired { get; set; }
		public DroneConsumerState State { get; private set; }

		public event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;

		private DroneConsumerState FindDroneState(int numOfDrones, int numOfDronesRequired)
		{
			if (numOfDrones > numOfDronesRequired)
			{
				return DroneConsumerState.Focused;
			}
			else if (numOfDrones == numOfDronesRequired)
			{
				return DroneConsumerState.Normal;
			}
			else if (numOfDrones == 0)
			{
				return DroneConsumerState.Idle;
			}
			throw new InvalidProgramException();
		}
	}

	// FELIX  Allow building construction to be paused via explicit click?
	// Ie, not just via focusing on a different building?
	public interface IDroneManager
	{
		int NumOfDrones { get; set; }

		bool CanAddDroneConsumer(IDroneConsumer droneConsumer);
		void AddDroneConsumer(IDroneConsumer droneConsumer);
		void RemoveDroneConsumer(IDroneConsumer droneConsumer);

		/// <summary>
		/// DroneConsumerState
		/// Idle => Normal
		/// Normal => Focused
		/// Focused => Normal
		/// </summary>
		void FocusOnDroneConsumer(IDroneConsumer droneConsumer);
	}

	public class DroneManager : IDroneManager
	{
		private IList<IDroneConsumer> _droneConsumers;

		private int _numOfDrones;
		public int NumOfDrones
		{
			get
			{
				return _numOfDrones;
			}
			set
			{
				if (_numOfDrones != value)
				{
					// FELIX  Update drone allocations (if extra => assign, if less => remove)
					_numOfDrones = value;
				}
			}
		}

		public DroneManager()
		{
			_droneConsumers = new List<IDroneConsumer>();
			_numOfDrones = 0;
		}

		public bool CanAddDroneConsumer(IDroneConsumer droneConsumer)
		{
			return CanSupportDroneConsumer(droneConsumer, NumOfDrones);
		}

		private bool CanSupportDroneConsumer(IDroneConsumer droneConsumer, int numOfDrones)
		{
			return numOfDrones >= droneConsumer.NumOfDronesRequired;
		}

		public void AddDroneConsumer(IDroneConsumer droneConsumer)
		{
			Assert.IsTrue(CanAddDroneConsumer(droneConsumer));
			_droneConsumers.Add(droneConsumer);

			int numOfFreeDrones = FreeUpDrones(droneConsumer.NumOfDronesRequired);
			Assert.IsTrue(numOfFreeDrones >= droneConsumer.NumOfDronesRequired);

			droneConsumer.NumOfDrones = droneConsumer.NumOfDronesRequired;
			int numOfSpareDrones = numOfFreeDrones - droneConsumer.NumOfDrones;

			if (numOfSpareDrones > 0)
			{
				AssignSpareDrones(numOfSpareDrones);
			}

			CheckStateIsValid();
		}

		/// <summary>
		/// Removes drones from the lowest priority consumers, to provide at least
		/// the specified number of drones.
		/// 
		/// Can end up providing more.  For example, if a consumer requires 2 drones
		/// and we only need 1, we will take 2 drones because the consumer cannot do
		/// anything with that single drone.
		/// 
		/// Surplus drones should be reassigned.
		/// </summary>
		/// <returns>The number of drones that have been freed.  Note: May be more
		/// than the specified numOfDesiredDrones!</returns>
		/// <param name="numOfDesiredDrones">Number of desired drones.</param>
		private int FreeUpDrones(int numOfDesiredDrones)
		{
			return -1;
		}

		// FELIX NEXT Comments :)
		private void AssignSpareDrones(int numOfSpareDrones)
		{

		}

		private void CheckStateIsValid()
		{
//			int numOfDronesUsedByConsumers = 
		}

		// Can have unusable drones!  ONLY if we have no consumers!
		public void RemoveDroneConsumer(IDroneConsumer droneConsumer)
		{
			throw new NotImplementedException();
		}

		public void FocusOnDroneConsumer(IDroneConsumer droneConsumer)
		{
			throw new NotImplementedException();
		}
	}
}