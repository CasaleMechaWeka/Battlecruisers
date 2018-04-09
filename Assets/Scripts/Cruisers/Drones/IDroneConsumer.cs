using System;

namespace BattleCruisers.Cruisers.Drones
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

	/// <summary>
    /// Used by buildings, units and cruisers.  Anything that needs to be built or repaired.
    /// </summary>
	public interface IDroneConsumer
	{
		int NumOfDrones { get; set; }
		int NumOfDronesRequired { get; }
		DroneConsumerState State { get; }

        /// <summary>
        /// True for a buildable under construction.
        /// False for reparing a buildable or a cruiser.
        /// </summary>
        bool IsHighPriority { get; }

		event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;
		event EventHandler<DroneStateChangedEventArgs> DroneStateChanged;
	}
}
