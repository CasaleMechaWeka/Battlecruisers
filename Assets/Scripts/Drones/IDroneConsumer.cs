using System;

namespace BattleCruisers.Drones
{
	public enum DroneConsumerState
	{
		Active,     // Has the exact number of drones required
		Focused,    // Has more than the number of drones required
		Idle        // Has no drones
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
}
