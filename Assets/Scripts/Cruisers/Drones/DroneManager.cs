using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones
{
    /// <summary>
    /// Only one drone consumer (DC) can be focused at a time (focused 
    /// meaning they have more than their required number of drones).
    /// 
    /// If a DC is focused they are the highest priority DC and any 
    /// newly available drones will go to them.
    /// </summary>
    public class DroneManager : IDroneManager
	{
		private readonly IList<IDroneConsumer> _droneConsumers;

        private const int MIN_NUM_OF_DRONES = 0;

		private int _numOfDrones;
		public int NumOfDrones
		{
			get { return _numOfDrones; }
			set
			{
				Logging.Log(Tags.DRONES, string.Format("NumOfDrones: {0} > {1}    NumOfDroneConsumers: {2}", _numOfDrones, value, _droneConsumers.Count));

                Assert.IsTrue(value >= MIN_NUM_OF_DRONES, string.Format("Invalid num of drones {0}.  Must be at least {1}", value, MIN_NUM_OF_DRONES));

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
        /// === High Priority Drone Consumer ===
        /// 
        /// Will be added as the lowest priority high priority consumer.  Ie, the consumer
        /// will be the last high priority consumer, but will be before the first
        /// low priority consumer.
        /// 
        /// Will only be assigned drones if:
        /// 1. There is a focused consumer with enough spare drones to cover the
        ///     new consumer.
        /// AND
        /// 2. All higher priority consumers have drones assigned.
        /// 
        /// 
        /// === Low Priority Drone Consumer ===
        /// Will be added as the lowest priority drone consumer.  Hence, will NOT
        /// be assigned any drones unless it is the only drone consumer.
		/// </summary>
		public void AddDroneConsumer(IDroneConsumer droneConsumer)
		{
			Logging.Log(Tags.DRONES, "AddDroneConsumer()  NumOfDroneConsumers: " + _droneConsumers.Count);

            Assert.IsTrue(CanSupportDroneConsumer(droneConsumer.NumOfDronesRequired), "Not enough drones to support drone consumer :/");
            Assert.IsFalse(_droneConsumers.Contains(droneConsumer), "Drone consumer has already been added.  Should not be added again!");

            if (droneConsumer.IsHighPriority)
            {
                AddHighPriorityDroneConsumer(droneConsumer);
            }
            else
            {
                AddLowPriorityDroneConsumer(droneConsumer);
            }
        }

        /// <summary>
        /// Add drone consumer as the lowest high priority consumer.  Ie, the consumer
        /// will be the last high priority consumer, but will be before the first
        /// low priority consumer.
        /// </summary>
        private void AddHighPriorityDroneConsumer(IDroneConsumer droneConsumer)
        {
            // FELIX  Fix it :D
			int numOfSpareDrones = ProvideRequiredDrones(droneConsumer);

            IDroneConsumer focusedConsumer = GetFocusedConsumer();
			if (focusedConsumer != null)
			{
				// Leave focused consumer as the highest priority consumer
				_droneConsumers.Insert(_droneConsumers.Count - 1, droneConsumer);
			}
			else
			{
				// Make the new consumer have the highest priority
				_droneConsumers.Add(droneConsumer);
			}
			
			if (numOfSpareDrones > 0)
			{
				AssignSpareDrones(numOfSpareDrones);
			}
        }

        private void AddLowPriorityDroneConsumer(IDroneConsumer droneConsumer)
        {
            // Make new consumer have the lowest priority
            _droneConsumers.Insert(0, droneConsumer);

            if (_droneConsumers.Count == 1)
            {
                droneConsumer.NumOfDrones = NumOfDrones;
            }
        }

		/// <summary>
		/// Remove the given consumer and reassign their drones (if they had any).
		/// </summary>
		public void RemoveDroneConsumer(IDroneConsumer droneConsumer)
		{
			Logging.Log(Tags.DRONES, "RemoveDroneConsumer()  NumOfDroneConsumers: " + _droneConsumers.Count);

			bool wasRemoved = _droneConsumers.Remove(droneConsumer);
            Assert.IsTrue(wasRemoved, "Tried to remove consumer that was not first added.");

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

            IDroneConsumer focusedConsumer = GetFocusedConsumer();
			if (focusedConsumer != null)
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
		/// <returns>
        /// Returns the number of spare drones, after the drone consumer's required
		/// number has been satisfied.
        /// </returns>
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
            int numOfFreedDrones = FindSpareDrones();

            if (numOfFreedDrones < numOfDesiredDrones)
			{
				// Remove drones from focused consuemr
                IDroneConsumer focusedConsumer = GetFocusedConsumer();

				if (focusedConsumer != null)
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

				// Remove drones from active consumers (from low => high priority)
                if (numOfFreedDrones < numOfDesiredDrones)
                {
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

            Logging.Log(Tags.DRONES, "DroneManager.FreeUpDrones() numOfDesiredDrones: " + numOfDesiredDrones + "  numOfFreedDrones: " + numOfFreedDrones);

			Assert.IsTrue(numOfFreedDrones >= numOfDesiredDrones);
			return numOfFreedDrones;
		}

        private int FindSpareDrones()
        {
            int numOfDronesUsed = _droneConsumers.Sum(droneConsumer => droneConsumer.NumOfDrones);
            return NumOfDrones - numOfDronesUsed;
        }

        private IDroneConsumer GetFocusedConsumer()
        {
            IDroneConsumer potentiallyFocusedConsumer = GetHighestPriorityConsumer();

            if (potentiallyFocusedConsumer != null 
                && potentiallyFocusedConsumer.State == DroneConsumerState.Focused)
            {
                return potentiallyFocusedConsumer;
            }

            return null;
        }

		private IDroneConsumer GetHighestPriorityConsumer()
		{
			return _droneConsumers.LastOrDefault();
		}

        public bool HasDroneConsumer(IDroneConsumer droneConsumer)
        {
            return _droneConsumers.Contains(droneConsumer);
        }
    }
}
