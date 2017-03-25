using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Drones
{
	// FELIX  Allow building construction to be paused via explicit click?
	// Ie, not just via focusing on a different building?
	public interface IDroneManager
	{
		int NumOfDrones { get; set; }

		bool CanSupportDroneConsumer(IDroneConsumer droneConsumer);
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
				Assert.IsTrue(value >= 0);

				if (_numOfDrones != value)
				{
					if (_droneConsumers.Count != 0)
					{
						if (value > _numOfDrones)
						{
							int newDrones = value - _numOfDrones;
							AssignSpareDrones(newDrones);
						}
						else
						{
							int numOfLostDrones = _numOfDrones - value;
							int numOfFreedDrones = FreeUpDrones(numOfLostDrones);
							int numOfSpareDrones = numOfFreedDrones - numOfLostDrones;

							if (numOfSpareDrones > 0)
							{
								AssignSpareDrones(numOfSpareDrones);
							}
						}
					}

					_numOfDrones = value;
				}
			}
		}

		public DroneManager()
		{
			_droneConsumers = new List<IDroneConsumer>();
			_numOfDrones = 0;
		}

		public bool CanSupportDroneConsumer(IDroneConsumer droneConsumer)
		{
			return NumOfDrones >= droneConsumer.NumOfDronesRequired;
		}

		public void AddDroneConsumer(IDroneConsumer droneConsumer)
		{
			if (!CanSupportDroneConsumer(droneConsumer)
			    || _droneConsumers.Contains(droneConsumer))
			{
				throw new ArgumentException();
			}

			_droneConsumers.Add(droneConsumer);

			int numOfFreeDrones = FreeUpDrones(droneConsumer.NumOfDronesRequired);

			droneConsumer.NumOfDrones = droneConsumer.NumOfDronesRequired;
			int numOfSpareDrones = numOfFreeDrones - droneConsumer.NumOfDrones;

			if (numOfSpareDrones > 0)
			{
				AssignSpareDrones(numOfSpareDrones);
			}
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
			int freedUpDrones = 0;

			if (_droneConsumers.Count == 0)
			{
				freedUpDrones = NumOfDrones;
			}
			else
			{
				// Consumer priority:  Low => High
				for (int i = 0; i < _droneConsumers.Count; ++i)
				{
					IDroneConsumer droneConsumer = _droneConsumers[i];
					freedUpDrones += droneConsumer.NumOfDrones;
					droneConsumer.NumOfDrones = 0;

					if (freedUpDrones >= numOfDesiredDrones)
					{
						break;
					}
				}
			}

			Assert.IsTrue(freedUpDrones >= numOfDesiredDrones);
			return freedUpDrones;
		}

		/// <summary>
		/// Tries to provide the required number of drones to all consumers, starting
		/// with the highest prioirty consumers.
		/// 
		/// If there are any spare drones after all consumers have their required
		/// number of drones, all spare drones are assigned to the highest priority
		/// consumer.
		/// 
		/// If there are not enough drones for the consumer with the lowest number
		/// of required drones, NO drones will be assignd to any consumers.
		/// 
		/// Note:  Should never be called if there are no consumers, because then
		/// there are no consumers to assign them to.
		/// </summary>
		private void AssignSpareDrones(int numOfSpareDrones)
		{
			Assert.IsTrue(_droneConsumers.Count != 0);

			// Try to ensure all consumers have their required number of drones
			// Consumer priority:  High => Low
			for (int i = _droneConsumers.Count - 1; i >= 0; --i)
			{
				IDroneConsumer droneConsumer = _droneConsumers[i];

				if (droneConsumer.State == DroneConsumerState.Idle 
					&& droneConsumer.NumOfDronesRequired <= numOfSpareDrones)
				{
					droneConsumer.NumOfDrones = droneConsumer.NumOfDronesRequired;
					numOfSpareDrones -= numOfSpareDrones;

					if (numOfSpareDrones == 0)
					{
						break;
					}
				}
			}

			// Assign remaining spares to highest priority consumer
			if (numOfSpareDrones != 0)
			{
				// Consumer priority:  High => Low
				for (int i = _droneConsumers.Count - 1; i >= 0; --i)
				{
					IDroneConsumer droneConsumer = _droneConsumers[i];

					if (droneConsumer.State == DroneConsumerState.Active
					    || droneConsumer.State == DroneConsumerState.Focused)
					{
						droneConsumer.NumOfDrones += numOfSpareDrones;
						numOfSpareDrones = 0;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Remove the given consumer and reassign their drones (if they had any).
		/// </summary>
		public void RemoveDroneConsumer(IDroneConsumer droneConsumer)
		{
			bool wasRemoved = _droneConsumers.Remove(droneConsumer);

			if (!wasRemoved)
			{
				throw new ArgumentException("Tried to remove consumer that was not first added.");
			}

			if (droneConsumer.NumOfDrones != 0)
			{
				if (_droneConsumers.Count != 0)
				{
					AssignSpareDrones(droneConsumer.NumOfDrones);
				}
				droneConsumer.NumOfDrones = 0;
			}
		}

		/// <summary>
		/// Remove drones from ALL drone consumers and assign them all to
		/// this drone consumer.
		/// 
		/// Note:  The DroneManager must have at least the number of required
		/// drones for this drone consumer to be able to focus on them.  Ie,
		/// if 
		/// </summary>
		public void FocusOnDroneConsumer(IDroneConsumer droneConsumer)
		{
			if (NumOfDrones < droneConsumer.NumOfDronesRequired
				|| NumOfDrones == droneConsumer.NumOfDrones)
			{
				return;
			}

			int freedDrones = FreeUpDrones(NumOfDrones);
			Assert.AreEqual(freedDrones, NumOfDrones);

			droneConsumer.NumOfDrones = freedDrones;
		}
	}
}