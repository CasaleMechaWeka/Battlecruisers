using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
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
        private readonly IDeferrer _deferrer;
        private readonly IDroneNumFeedbackFactory _feedbackFactory;
        private ICruiser _cruiser;
        private IDroneConsumerProvider _droneConsumerProvider;
        private IDictionary<IRepairable, IDroneNumFeedback> _repairableToDroneNum;

        private const int NUM_OF_DRONES_REQUIRED_FOR_REPAIR = 1;

        public RepairManager(IDeferrer deferrer, IDroneNumFeedbackFactory feedbackFactory)
        {
            Helper.AssertIsNotNull(deferrer, feedbackFactory);

            _deferrer = deferrer;
            _feedbackFactory = feedbackFactory;
        }

        // Not constructor because of circular dependency between Cruiser and RepairManager
        public void Initialise(ICruiser cruiser)
        {
            Helper.AssertIsNotNull(cruiser, cruiser.DroneConsumerProvider);

            _cruiser = cruiser;
            _droneConsumerProvider = _cruiser.DroneConsumerProvider;

            _repairableToDroneNum = new Dictionary<IRepairable, IDroneNumFeedback>();

            AddRepairable(_cruiser);

            _cruiser.BuildingStarted += _cruiser_BuildingStarted;
            _cruiser.Destroyed += _cruiser_Destroyed;
        }

        private void RepairCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            IRepairable repairable = sender.Parse<IRepairCommand>().Repairable;

            Logging.Log(Tags.REPAIR_MANAGER, $"{repairable}  repairable.RepairCommand.CanExecute: {repairable.RepairCommand.CanExecute}");

            Assert.IsTrue(_repairableToDroneNum.ContainsKey(repairable));
            IDroneConsumer droneConsumer = _repairableToDroneNum[repairable].DroneConsumer;

            if (repairable.RepairCommand.CanExecute)
            {
                _droneConsumerProvider.ActivateDroneConsumer(droneConsumer);
            }
            else
            {
                _droneConsumerProvider.ReleaseDroneConsumer(droneConsumer);
            }
        }

        private void _cruiser_BuildingStarted(object sender, StartedBuildingConstructionEventArgs e)
        {
            AddRepairable(e.Buildable);
        }

        private void _cruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            CleanUpCruiser();
        }

        public void Repair(float deltaTimeInS)
        {
            Logging.Verbose(Tags.REPAIR_MANAGER, "_repairableToDroneConsumer.Count:  " + _repairableToDroneNum.Count);

            foreach (KeyValuePair<IRepairable, IDroneNumFeedback> pair in _repairableToDroneNum)
            {
                IRepairable repairable = pair.Key;
                IDroneConsumer droneConsumer = pair.Value.DroneConsumer;

                if (droneConsumer != null
                    && droneConsumer.State != DroneConsumerState.Idle)
                {
                    Logging.Verbose(Tags.REPAIR_MANAGER, "About to repair: " + repairable);

                    Assert.IsTrue(repairable.RepairCommand.CanExecute);
                    float healthGained = deltaTimeInS * droneConsumer.NumOfDrones * repairable.HealthGainPerDroneS * BuildSpeedMultipliers.DEFAULT;

                    // Defer, as this may bring the repairable to full health, which 
                    // sets its DroneConsumer to null, which modifies this enumerable :)
                    _deferrer.Defer(() => repairable.RepairCommand.Execute(healthGained));
                }
            }
        }

        private void AddRepairable(IRepairable repairable)
        {
            Logging.Log(Tags.REPAIR_MANAGER, "repairable: " + repairable);

            Assert.IsFalse(_repairableToDroneNum.ContainsKey(repairable));

            IDroneConsumer droneConsumer = _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR);

            IDroneNumFeedback droneNumFeedback = _feedbackFactory.CreateFeedback(droneConsumer, repairable.NumOfRepairDronesText);
            _repairableToDroneNum.Add(repairable, droneNumFeedback);
			
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

            Assert.IsTrue(_repairableToDroneNum.ContainsKey(repairable));

            IDroneNumFeedback droneNumFeedback = _repairableToDroneNum[repairable];
            droneNumFeedback.DisposeManagedState();
            _droneConsumerProvider.ReleaseDroneConsumer(droneNumFeedback.DroneConsumer);

            _repairableToDroneNum.Remove(repairable);

            repairable.Destroyed -= Repairable_Destroyed;
            repairable.RepairCommand.CanExecuteChanged -= RepairCommand_CanExecuteChanged;
        }

        private void Repairable_Destroyed(object sender, DestroyedEventArgs e)
        {
            RemoveRepairable(e.DestroyedTarget);
        }

		public IDroneConsumer GetDroneConsumer(IRepairable repairable)
		{
			Assert.IsTrue(_repairableToDroneNum.ContainsKey(repairable));
            return _repairableToDroneNum[repairable].DroneConsumer;
		}

        public void DisposeManagedState()
        {
            CleanUpCruiser();
        }

        private void CleanUpCruiser()
        {
            IList<IRepairable> repairables = _repairableToDroneNum.Keys.ToList();

            foreach (IRepairable repairable in repairables)
            {
                RemoveRepairable(repairable);
            }

            _cruiser.Destroyed -= _cruiser_Destroyed;
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;
        }
    }
}
