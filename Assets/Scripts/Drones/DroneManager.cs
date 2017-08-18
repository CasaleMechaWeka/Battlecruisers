using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Drones
{
	/// <summary>
	/// Only one drone consumer (DC) can be focused at a time (focused 
	/// meaning they have more than their required number of drones).
	/// 
	/// If a DC is focused they are the highest priority DC, and any 
	/// newly available drones will go to them.
	/// </summary>
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
				Logging.Log(Tags.DRONES, string.Format("NumOfDrones: {0} > {1}    NumOfDroneConsumers: {2}", _numOfDrones, value, _droneConsumers.Count));

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

					if (DroneNumChanged != null)
					{
                        DroneNumChanged.Invoke(this, new DroneNumChangedEventArgs(_numOfDrones));
					}
				}
			}
		}

		public event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;

		public DroneManager()
		{
			_droneConsumers = new List<IDroneConsumer>();
			_numOfDrones = 0;
		}

		public bool CanSupportDroneConsumer(int numOfDronesRequired)
		{
			return NumOfDrones >= numOfDronesRequired;
		}

		/// <summary>
		/// The newly added consumer will always be provided with at least
		/// its required number of drones.
		/// 
		/// Usually the newly added consumer is the highest priority consumer.
		/// 
		/// The exception to this is if there is a focused consumer, which has
		/// enough drones to spare for the newly added consumer AND still be
		/// focused.  In this case that focused consumer remains the highest
		/// priority consumer.
		/// </summary>
		public void AddDroneConsumer(IDroneConsumer droneConsumer)
		{
			Logging.Log(Tags.DRONES, "AddDroneConsumer()  NumOfDroneConsumers: " + _droneConsumers.Count);

			if (!CanSupportDroneConsumer(droneConsumer.NumOfDronesRequired)
			    || _droneConsumers.Contains(droneConsumer))
			{
				throw new ArgumentException();
			}

			int numOfSpareDrones = ProvideRequiredDrones(droneConsumer);

			IDroneConsumer focusedConsumer = GetHighestPriorityConsumer();
			if (focusedConsumer != null && focusedConsumer.State == DroneConsumerState.Focused)
			{
				// Leave focused consumer as the highest priority consumer
				_droneConsumers.Insert(_droneConsumers.Count - 1, droneConsumer);
			}
			else
			{
				// Make the new consumer be have the highest priority
				_droneConsumers.Add(droneConsumer);
			}

			if (numOfSpareDrones > 0)
			{
				AssignSpareDrones(numOfSpareDrones);
			}
		}

		/// <summary>
		/// Remove the given consumer and reassign their drones (if they had any).
		/// </summary>
		public void RemoveDroneConsumer(IDroneConsumer droneConsumer)
		{
			Logging.Log(Tags.DRONES, "RemoveDroneConsumer()  NumOfDroneConsumers: " + _droneConsumers.Count);

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
		/// Idle => Active (Can remain idle, if there are less drones than required by the drone consumer.)
		/// Active => Focused (Can remain Active, if there are no more drones.)
		/// Focused => 
		/// 	a) => More Focused, if not all drones are working on this consumer
		/// 	b) => Active (Can remain Focused if there are no other drone consumers.)
		/// </summary>
		public void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer)
		{
			Logging.Log(Tags.DRONES, "ToggleDroneConsumerFocus()  NumOfDroneConsumers: " + _droneConsumers.Count);

			if (NumOfDrones < droneConsumer.NumOfDronesRequired)
			{
				return;
			}

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
						AssignAllDronesToConsumer(droneConsumer);
					}
					break;

				case DroneConsumerState.Focused:
					if (droneConsumer.NumOfDrones < NumOfDrones)
					{
						// Focused => More Focused
						AssignAllDronesToConsumer(droneConsumer);
					}
					else
					{
						// Focused => Active
						int numOfFreedDrones = droneConsumer.NumOfDrones - droneConsumer.NumOfDronesRequired;
						droneConsumer.NumOfDrones = droneConsumer.NumOfDronesRequired;

						AssignSpareDrones(numOfFreedDrones);
					}
					break;

				default:
					throw new InvalidProgramException();
			}
		}

		private void AssignAllDronesToConsumer(IDroneConsumer droneConsumer)
		{
			int numOfFreedDrones = FreeUpDrones(NumOfDrones);
			Assert.AreEqual(numOfFreedDrones, NumOfDrones);
			droneConsumer.NumOfDrones = numOfFreedDrones;
		}

		/// <summary>
		/// If a focused consumer exists, assigns all drones to that consumer.
		/// 
		/// Otherwise, tries to provide the required number of drones to all consumers, 
		/// starting with the highest priority consumers.
		/// 
		/// If there are any spare drones after all consumers have their required
		/// number of drones, all spare drones are assigned to the highest priority
		/// consumer.
		/// 
		/// If there are not enough drones for the consumer with the lowest number
		/// of required drones, NO drones will be assignd to any consumers.
		/// 
		/// Note:  Should never be called if there are no consumers, because then
		/// there are no consumers to assign the drones to.
		/// </summary>
		private void AssignSpareDrones(int numOfSpareDrones)
		{
			Assert.IsTrue(_droneConsumers.Count != 0);

			IDroneConsumer focusedConsumer = GetHighestPriorityConsumer();
			if (focusedConsumer.State == DroneConsumerState.Focused)
			{
				focusedConsumer.NumOfDrones += numOfSpareDrones;
				return;
			}

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
			int numOfFreedDrones = 0;

			if (_droneConsumers.Count == 0)
			{
				numOfFreedDrones = NumOfDrones;
			}
			else
			{
				// Remove drones from focused consuemr
				IDroneConsumer focusedConsumer = GetHighestPriorityConsumer();

				if (focusedConsumer.State == DroneConsumerState.Focused)

				{
					if (focusedConsumer.NumOfDrones - numOfDesiredDrones > focusedConsumer.NumOfDronesRequired)
					{
						// Focused consumer will remain focused even after giving up
						// the numOfDesiredDrones
						numOfFreedDrones = numOfDesiredDrones;
						focusedConsumer.NumOfDrones -= numOfDesiredDrones;
					}
					else
					{
						// Focused consumer will become active
						numOfFreedDrones = focusedConsumer.NumOfDrones - focusedConsumer.NumOfDronesRequired;
						focusedConsumer.NumOfDrones = focusedConsumer.NumOfDronesRequired;
					}
				}

				if (numOfFreedDrones < numOfDesiredDrones)
				{
					// Remove drones from active consumers
					// Consumer priority:  Low => High
					for (int i = 0; i < _droneConsumers.Count; ++i)
					{
						IDroneConsumer droneConsumer = _droneConsumers[i];
						numOfFreedDrones += droneConsumer.NumOfDrones;
						droneConsumer.NumOfDrones = 0;

						if (numOfFreedDrones >= numOfDesiredDrones)
						{
							break;
						}
					}
				}
			}

			Assert.IsTrue(numOfFreedDrones >= numOfDesiredDrones);
			return numOfFreedDrones;
		}

		private IDroneConsumer GetHighestPriorityConsumer()
		{
			return _droneConsumers.LastOrDefault();
		}
	}
}
