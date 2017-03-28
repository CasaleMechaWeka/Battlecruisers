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
		void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer);
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
				if (value < 0)
				{
					throw new ArgumentException();
				}

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

			int numOfSpareDrones = ProvideRequiredDrones(droneConsumer);
			
			_droneConsumers.Add(droneConsumer);

			if (numOfSpareDrones > 0)
			{
				AssignSpareDrones(numOfSpareDrones);
			}
		}

		/// <summary>
		/// Removes drones from the lowest priority consumers, to provide at least
		/// the specified number of drones.
		/// 
		/// First removes drones from focused consumers.
		/// 
		/// If not enough drones have been freed, moves on to remove drones from
		/// active consumers.
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
				// Remove drones from focused consuemrs
				// Consumer priority:  Low => High
				// FELIX


				// FELIX
				// Remove drones from active consumers
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
					numOfSpareDrones -= droneConsumer.NumOfDrones;

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
		/// DroneConsumerState
		/// Idle => Normal (Can remain idle, if there are less drones than required by the drone consumer.)
		/// Normal => Focused (Can remain Normal, if there are no more drones.)
		/// Focused => Normal (Can remain Focused if there are no other drone consumers.)
		/// </summary>
		public void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer)
		{
			if (NumOfDrones < droneConsumer.NumOfDronesRequired)
			{
				return;
			}

			int numOfFreedDrones;

			switch (droneConsumer.State)
			{
				// Idle => Active
				case DroneConsumerState.Idle:
					SetMaxPriority(droneConsumer);
					int numOfSpareDrones = ProvideRequiredDrones(droneConsumer);
					if (numOfSpareDrones > 0)
					{
						AssignSpareDrones(numOfSpareDrones);
					}
					break;

				// Active => Focused
				case DroneConsumerState.Active:
					SetMaxPriority(droneConsumer);
					if (droneConsumer.NumOfDronesRequired != NumOfDrones)
					{
						numOfFreedDrones = FreeUpDrones(NumOfDrones);
						Assert.AreEqual(numOfFreedDrones, NumOfDrones);
						
						droneConsumer.NumOfDrones = numOfFreedDrones;
					}
					break;

				// Focused => Active
				case DroneConsumerState.Focused:
					numOfFreedDrones = droneConsumer.NumOfDrones - droneConsumer.NumOfDronesRequired;
					droneConsumer.NumOfDrones = droneConsumer.NumOfDronesRequired;
					AssignSpareDrones(numOfFreedDrones);
					break;

				default:
					throw new InvalidProgramException();
			}
		}

		/// <summary>
		/// Ensures the given drone consumer has the highest priority, by placing
		/// them at the top of the list of drone consumers.
		/// </summary>
		private void SetMaxPriority(IDroneConsumer droneConsumer)
		{
			int index = _droneConsumers.IndexOf(droneConsumer);
			Assert.IsTrue(index != -1);

			if ((index + 1) != _droneConsumers.Count)
			{
				// Not highest priority yet, so increase priority!
				_droneConsumers.RemoveAt(index);
				_droneConsumers.Add(droneConsumer);
			}
		}

		/// <summary>
		/// Frees up the required number of drones for the given drone consumer
		/// and assigns them to the given drone consumer.
		/// 
		/// <returns>Returns the number of spare drones, after the drone consumer's required
		/// number has been satisfied.</returns>
		private int ProvideRequiredDrones(IDroneConsumer droneConsumer)
		{
			int numOfFreeDrones = FreeUpDrones(droneConsumer.NumOfDronesRequired);
			droneConsumer.NumOfDrones = droneConsumer.NumOfDronesRequired;
			return numOfFreeDrones - droneConsumer.NumOfDrones;
		}
	}
}