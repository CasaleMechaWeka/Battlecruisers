using BattleCruisers.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private const int MIN_NUM_OF_DRONES = 0;

        // Consumers are in descending order of priority.  Ie, the first consumer 
        // has the highest priority and the last consumer has the lowest priority.
        private readonly ObservableCollection<IDroneConsumer> _droneConsumers;
        public ReadOnlyObservableCollection<IDroneConsumer> DroneConsumers { get; }

        private int _numOfDrones;
        public int NumOfDrones
        {
            get { return _numOfDrones; }
            set
            {
                Logging.Log(Tags.DRONE_MANAGER, $"NumOfDrones: {_numOfDrones} > {value}    NumOfDroneConsumers: {_droneConsumers.Count}");
                Assert.IsTrue(value >= MIN_NUM_OF_DRONES, $"Invalid num of drones {value}.  Must be at least {MIN_NUM_OF_DRONES}");

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
            _droneConsumers = new ObservableCollection<IDroneConsumer>();
            DroneConsumers = new ReadOnlyObservableCollection<IDroneConsumer>(_droneConsumers);
            _numOfDrones = 0;
        }

        public bool CanSupportDroneConsumer(int numOfDronesRequired)
        {
            return NumOfDrones >= numOfDronesRequired;
        }

        /// <summary>
        /// Adds the new drone consumer as the lowest priority drone consumer.
        /// 
        /// Will only be assigned drones if:
        /// 1. It is the only drone consumer
        /// OR
        /// 2.  a) All existing consumers are not idle (ie, either active or focused).
	    ///     AND
        ///     b) There is a focused consumer with enough spare drones to cover the
        ///     new consumer.
        /// OR
        /// 3. All existing consumers are paused (ie, because we can no longer afford them).
        /// </summary>
        public void AddDroneConsumer(IDroneConsumer consumerToAdd)
        {
            Logging.Log(Tags.DRONE_MANAGER, "NumOfDroneConsumers: " + _droneConsumers.Count);

            Assert.IsTrue(CanSupportDroneConsumer(consumerToAdd.NumOfDronesRequired), "Not enough drones to support drone consumer :/");
            Assert.IsFalse(_droneConsumers.Contains(consumerToAdd), "Drone consumer has already been added.  Should not be added again!");

            bool wereAllConsumersNotIdle = AllConsumersAreNotIdle();
            IDroneConsumer focusedConsumer = GetFocusedConsumer();

            // Make new consumer have the lowest priority
            _droneConsumers.Add(consumerToAdd);

            if (_droneConsumers.Count == 1
                || (wereAllConsumersNotIdle
                    && focusedConsumer != null
                    && focusedConsumer.NumOfSpareDrones >= consumerToAdd.NumOfDronesRequired)
                || AllConsumersAreIdle())
            {
                Logging.Log(Tags.DRONE_MANAGER, "Immediately provide required drones :)");
                ProvideRequiredDrones(consumerToAdd);
            }
        }

        private bool AllConsumersAreNotIdle()
        {
            return
                _droneConsumers
                    .All(consumer => consumer.State != DroneConsumerState.Idle);
        }

        private bool AllConsumersAreIdle()
        {
            return
                _droneConsumers
                    .All(consumer => consumer.State == DroneConsumerState.Idle);
        }

		/// <summary>
		/// Remove the given consumer and reassign their drones (if they had any).
		/// </summary>
        public void RemoveDroneConsumer(IDroneConsumer consumerToRemove)
		{
			Logging.Log(Tags.DRONE_MANAGER, "NumOfDroneConsumers: " + _droneConsumers.Count);

			bool wasRemoved = _droneConsumers.Remove(consumerToRemove);
            Assert.IsTrue(wasRemoved, "Tried to remove consumer that was not first added.");

			if (consumerToRemove.NumOfDrones != 0)
			{
				if (_droneConsumers.Count != 0)
				{
					AssignSpareDrones(consumerToRemove.NumOfDrones);
				}
				consumerToRemove.NumOfDrones = 0;
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
			Logging.Log(Tags.DRONE_MANAGER, "NumOfDroneConsumers: " + _droneConsumers.Count);

			if (NumOfDrones < droneConsumer.NumOfDronesRequired)
			{
				return;
			}

			switch (droneConsumer.State)
			{
				// Idle => Active
				case DroneConsumerState.Idle:
					SetMaxPriority(droneConsumer);
					ProvideRequiredDrones(droneConsumer);
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
		/// non-idle consumer.
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

            numOfSpareDrones = MakeConsumersActive(numOfSpareDrones);

			if (numOfSpareDrones != 0)
			{
				AssignToHighestPriorityNonIdleConsumer(numOfSpareDrones);
            }
        }

        private void AssignToHighestPriorityNonIdleConsumer(int numOfSpareDrones)
        {
            // Consumer priority:  High => Low
            foreach (IDroneConsumer droneConsumer in _droneConsumers)
            {
                if (droneConsumer.State == DroneConsumerState.Idle)
                {
                    // We do not have enough drones to activate this consumer
                    Assert.IsTrue(numOfSpareDrones < droneConsumer.NumOfDronesRequired);
                    continue;
                }

                if (droneConsumer.State == DroneConsumerState.Active)
                {
                    // Adding drones will make this consumer focused, so it
                    // should have the highest priority.
                    SetMaxPriority(droneConsumer);
                }

                droneConsumer.NumOfDrones += numOfSpareDrones;
                break;
            }
        }

        /// <summary>
        /// Try to ensure all consumers active (have their required number of drones)
        /// </summary>
        private int MakeConsumersActive(int numOfSpareDrones)
        {
			// Consumer priority:  High => Low
            foreach (IDroneConsumer droneConsumer in _droneConsumers)
            {
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

            return numOfSpareDrones;
        }

		/// <summary>
		/// Ensures the given drone consumer has the highest priority, by placing
		/// them at the top of the list of drone consumers.
		/// </summary>
		private void SetMaxPriority(IDroneConsumer droneConsumer)
		{
            if (GetHighestPriorityConsumer() == droneConsumer)
            {
                // Drone consumer is already the highest priority
                return;
            }

            bool wasRemoved = _droneConsumers.Remove(droneConsumer);
            Assert.IsTrue(wasRemoved);

            _droneConsumers.Insert(0, droneConsumer);
		}

		/// <summary>
		/// Frees up the required number of drones for the given drone consumer
		/// and assigns them to the given drone consumer.
		/// 
        /// Assigns any spare drones.
        /// </summary>
		private void ProvideRequiredDrones(IDroneConsumer droneConsumer)
		{
			int numOfFreeDrones = FreeUpDrones(droneConsumer.NumOfDronesRequired);
			droneConsumer.NumOfDrones = droneConsumer.NumOfDronesRequired;
			
            int numOfSpareDrones = numOfFreeDrones - droneConsumer.NumOfDrones;
            AssignSpareDrones(numOfSpareDrones);
		}

		/// <summary>
		/// First removes drones from the focused consumer, if there is one.
        /// 
		/// If not enough drones have been freed, moves on to remove drones from
		/// active consumers, starting with those with the lowest priority.
		/// 
		/// Can end up providing more.  For example, if a consumer requires 2 drones
		/// and we only need 1, we will take 2 drones because the consumer cannot do
		/// anything with that single drone.
		/// 
		/// Surplus drones should be reassigned.
		/// </summary>
		/// <returns>
        /// The number of drones that have been freed.  Note: May be more
		/// than the specified numOfDesiredDrones!
        /// </returns>
		/// <param name="minDronesToFree">Minimum number of drones to free.</param>
        private int FreeUpDrones(int minDronesToFree)
		{
            int numOfFreedDrones = FindSpareDrones();

            if (numOfFreedDrones < minDronesToFree)
			{
				// Remove drones from focused consuemr
                IDroneConsumer focusedConsumer = GetFocusedConsumer();

				if (focusedConsumer != null)
				{
					if (focusedConsumer.NumOfDrones - minDronesToFree > focusedConsumer.NumOfDronesRequired)
					{
						// Focused consumer will remain focused even after giving up
						// the numOfDesiredDrones
						numOfFreedDrones = minDronesToFree;
						focusedConsumer.NumOfDrones -= minDronesToFree;
					}
					else
					{
						// Focused consumer will become active
						numOfFreedDrones = focusedConsumer.NumOfDrones - focusedConsumer.NumOfDronesRequired;
						focusedConsumer.NumOfDrones = focusedConsumer.NumOfDronesRequired;
					}
				}

				// Remove drones from active consumers (from low => high priority)
                if (numOfFreedDrones < minDronesToFree)
                {
                    for (int i = _droneConsumers.Count - 1; i >= 0; --i)
					{
						IDroneConsumer droneConsumer = _droneConsumers[i];

						numOfFreedDrones += droneConsumer.NumOfDrones;
						droneConsumer.NumOfDrones = 0;

						if (numOfFreedDrones >= minDronesToFree)
						{
							break;
						}
					}
				}
			}

            Logging.Log(Tags.DRONE_MANAGER, $"numOfDesiredDrones: {minDronesToFree}  numOfFreedDrones: {numOfFreedDrones}");

			Assert.IsTrue(numOfFreedDrones >= minDronesToFree);
			return numOfFreedDrones;
		}

        /// <summary>
        /// There should never be any spare drones unless:
        /// + There are no drone consumers.
        /// + We have a drone consumer we can no longer afford.  Ie:
        ///     1. Had 6 drones
        ///     2. Started consumer that needs 6 drones
        ///     3. Lost 2 drones
        ///     => Have 4 spare drones despite having a consumer!
        /// </summary>
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
            return _droneConsumers.FirstOrDefault();
		}

        public bool HasDroneConsumer(IDroneConsumer droneConsumer)
        {
            return _droneConsumers.Contains(droneConsumer);
        }
    }
}
