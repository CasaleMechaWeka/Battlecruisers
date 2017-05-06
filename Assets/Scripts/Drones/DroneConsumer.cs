using System;
using System.Collections;
using System.Collections.Generic;

namespace BattleCruisers.Drones
{
	public enum DroneConsumerState
	{
		Active,		// Has the exact number of drones required
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

	public class DroneStateChangedEventArgs : EventArgs
	{
		public DroneConsumerState OldState { get; private set; }
		public DroneConsumerState NewState { get; private set; }

		public DroneStateChangedEventArgs(DroneConsumerState oldState, DroneConsumerState newState)
		{
			OldState = oldState;
			NewState = newState;
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
		event EventHandler<DroneStateChangedEventArgs> DroneStateChanged;
	}

	public class DroneConsumer : IDroneConsumer
	{
		private int _numOfDrones;
		public int NumOfDrones
		{
			get { return _numOfDrones; }
			set
			{
				if (value < 0
					|| (value != 0 && value < NumOfDronesRequired))
				{
					throw new ArgumentException();
				}

				if (value != _numOfDrones)
				{
					_numOfDrones = value;
					if (DroneNumChanged != null)
					{
						DroneNumChanged.Invoke(this, new DroneNumChangedEventArgs(_numOfDrones));
					}
					DroneConsumerState newState = FindDroneState(_numOfDrones, NumOfDronesRequired);
					if (newState != State)
					{
						if (DroneStateChanged != null)
						{
							DroneStateChanged.Invoke(this, new DroneStateChangedEventArgs(State, newState));
						}
						State = newState;
					}
				}
			}
		}

		public int NumOfDronesRequired { get; set; }
		public DroneConsumerState State { get; private set; }

		public event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;
		public event EventHandler<DroneStateChangedEventArgs> DroneStateChanged;

		public DroneConsumer(int numOfDronesRequired)
		{
			if (numOfDronesRequired < 0)
			{
				throw new ArgumentException();
			}

			NumOfDronesRequired = numOfDronesRequired;
			NumOfDrones = 0;
			State = DroneConsumerState.Idle;
		}

		private DroneConsumerState FindDroneState(int numOfDrones, int numOfDronesRequired)
		{
			if (numOfDrones > numOfDronesRequired)
			{
				return DroneConsumerState.Focused;
			}
			else if (numOfDrones == numOfDronesRequired)
			{
				return DroneConsumerState.Active;
			}
			else if (numOfDrones == 0)
			{
				return DroneConsumerState.Idle;
			}
			throw new InvalidProgramException();
		}
	}
}
