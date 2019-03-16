using System;

namespace BattleCruisers.Cruisers.Drones
{
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

					DroneNumChanged?.Invoke(this, new DroneNumChangedEventArgs(_numOfDrones));
					
                    DroneConsumerState newState = FindDroneState(_numOfDrones, NumOfDronesRequired);
					
                    if (newState != State)
					{
						DroneStateChanged?.Invoke(this, new DroneStateChangedEventArgs(State, newState));
						State = newState;
					}
				}
			}
		}

		public int NumOfDronesRequired { get; set; }
        public int NumOfSpareDrones { get { return NumOfDrones - NumOfDronesRequired; } }
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
