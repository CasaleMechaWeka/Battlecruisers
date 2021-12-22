using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Repairables
{
    /// <summary>
    /// Keeps track of all repairables (cruiser, buildings).  
    /// 
    /// Creates a drone consumer when the repairable is created and holds on to
    /// this consumer until the repairable is destroyed.
    /// 
    /// When the repairable become repairable (ie, damaged), activates the consumer 
    /// (adds the consumer to the drone manager).
    /// 
    /// When the repairable is no longer repairable (ie, fully repaired), releases 
    /// their consumer (removes the consumer from the drone manager).
    /// </summary>
    public class RepairManager : IRepairManager
    {
        private readonly IDroneFeedbackFactory _feedbackFactory;
        private readonly IDroneConsumerProvider _droneConsumerProvider;
        private readonly ICruiser _cruiser;
        private readonly IDictionary<IRepairable, IDroneFeedback> _repairableToFeedback;

        public const int NUM_OF_DRONES_REQUIRED_FOR_REPAIR = 1;

        // Code smell :D  ICruiser contains DroneConsumerProvider property, but this is not set
        // until cruiser has been initialised.  Hence directly pass drone consumer provider.
        public RepairManager(
            IDroneFeedbackFactory feedbackFactory,
            IDroneConsumerProvider droneConsumerProvider,
            ICruiser cruiser)
        {
            Helper.AssertIsNotNull(feedbackFactory, droneConsumerProvider, cruiser);

            _feedbackFactory = feedbackFactory;
            _droneConsumerProvider = droneConsumerProvider;
            _cruiser = cruiser;

            _repairableToFeedback = new Dictionary<IRepairable, IDroneFeedback>();

            AddRepairable(_cruiser);

            _cruiser.BuildingStarted += _cruiser_BuildingStarted;
            _cruiser.Destroyed += _cruiser_Destroyed;
        }

        private void RepairCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            IRepairable repairable = sender.Parse<IRepairCommand>().Repairable;

            Logging.Log(Tags.REPAIR_MANAGER, $"{repairable}  repairable.RepairCommand.CanExecute: {repairable.RepairCommand.CanExecute}");

            Assert.IsTrue(_repairableToFeedback.ContainsKey(repairable));
            IDroneConsumer droneConsumer = _repairableToFeedback[repairable].DroneConsumer;

            if (repairable.RepairCommand.CanExecute)
            {
                _droneConsumerProvider.ActivateDroneConsumer(droneConsumer);
            }
            else
            {
                _droneConsumerProvider.ReleaseDroneConsumer(droneConsumer);
            }
        }

        private void _cruiser_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            AddRepairable(e.StartedBuilding);
        }

        private void _cruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            CleanUpCruiser();
        }

        public void Repair(float deltaTimeInS)
        {
            Logging.Verbose(Tags.REPAIR_MANAGER, "_repairableToDroneConsumer.Count:  " + _repairableToFeedback.Count);

            foreach (KeyValuePair<IRepairable, IDroneFeedback> pair in _repairableToFeedback)
            {
                IRepairable repairable = pair.Key;
                IDroneConsumer droneConsumer = pair.Value.DroneConsumer;

                if (droneConsumer != null
                    && droneConsumer.State != DroneConsumerState.Idle)
                {
                    Logging.Verbose(Tags.REPAIR_MANAGER, "About to repair: " + repairable);

                    Assert.IsTrue(repairable.RepairCommand.CanExecute);
                    float healthGained = deltaTimeInS * droneConsumer.NumOfDrones * repairable.HealthGainPerDroneS * BuildSpeedMultipliers.DEFAULT;
                    repairable.RepairCommand.Execute(healthGained);
                }
            }
        }
        public void RemoveCruiser()
        {
            Assert.IsNotNull(_cruiser);
            RemoveRepairable(_cruiser);
        }

        private void AddRepairable(IRepairable repairable)
        {
            Logging.Log(Tags.REPAIR_MANAGER, "repairable: " + repairable);

            Assert.IsFalse(_repairableToFeedback.ContainsKey(repairable));

            IDroneConsumer droneConsumer = _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR);

            IDroneFeedback droneNumFeedback = _feedbackFactory.CreateFeedback(droneConsumer, repairable.DroneAreaPosition, repairable.DroneAreaSize);
            _repairableToFeedback.Add(repairable, droneNumFeedback);
			
            if (repairable.RepairCommand.CanExecute)
            {
                _droneConsumerProvider.ActivateDroneConsumer(droneConsumer);
            }

            repairable.Destroyed += Repairable_Destroyed;
            repairable.RepairCommand.CanExecuteChanged += RepairCommand_CanExecuteChanged;
        }

        private void RemoveRepairable(IRepairable repairable)
        {
            Logging.Log(Tags.REPAIR_MANAGER, "repairable: " + repairable);

            Assert.IsTrue(_repairableToFeedback.ContainsKey(repairable));

            IDroneFeedback droneNumFeedback = _repairableToFeedback[repairable];
            droneNumFeedback.DisposeManagedState();
            _droneConsumerProvider.ReleaseDroneConsumer(droneNumFeedback.DroneConsumer);

            _repairableToFeedback.Remove(repairable);

            repairable.Destroyed -= Repairable_Destroyed;
            repairable.RepairCommand.CanExecuteChanged -= RepairCommand_CanExecuteChanged;
        }

        private void Repairable_Destroyed(object sender, DestroyedEventArgs e)
        {
            RemoveRepairable(e.DestroyedTarget);
        }

		public IDroneConsumer GetDroneConsumer(IRepairable repairable)
		{
			Assert.IsTrue(_repairableToFeedback.ContainsKey(repairable));
            return _repairableToFeedback[repairable].DroneConsumer;
		}

        public void DisposeManagedState()
        {
            CleanUpCruiser();
        }

        private void CleanUpCruiser()
        {
            IList<IRepairable> repairables = _repairableToFeedback.Keys.ToList();

            foreach (IRepairable repairable in repairables)
            {
                RemoveRepairable(repairable);
            }

            _cruiser.Destroyed -= _cruiser_Destroyed;
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;
        }
    }
}
