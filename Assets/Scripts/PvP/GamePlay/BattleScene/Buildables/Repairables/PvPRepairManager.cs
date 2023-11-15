using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables
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
    public class PvPRepairManager : IPvPRepairManager
    {
        private readonly IPvPDroneFeedbackFactory _feedbackFactory;
        private readonly IPvPDroneConsumerProvider _droneConsumerProvider;
        private readonly IPvPCruiser _cruiser;
        private readonly IDictionary<IPvPRepairable, IPvPDroneFeedback> _repairableToFeedback;

        public const int NUM_OF_DRONES_REQUIRED_FOR_REPAIR = 1;

        // Code smell :D  ICruiser contains DroneConsumerProvider property, but this is not set
        // until cruiser has been initialised.  Hence directly pass drone consumer provider.
        public PvPRepairManager(
            IPvPDroneFeedbackFactory feedbackFactory,
            IPvPDroneConsumerProvider droneConsumerProvider,
            IPvPCruiser cruiser)
        {
            PvPHelper.AssertIsNotNull(feedbackFactory, droneConsumerProvider, cruiser);

            _feedbackFactory = feedbackFactory;
            _droneConsumerProvider = droneConsumerProvider;
            _cruiser = cruiser;

            _repairableToFeedback = new Dictionary<IPvPRepairable, IPvPDroneFeedback>();

            AddRepairable(_cruiser);

            _cruiser.BuildingStarted += _cruiser_BuildingStarted;
            _cruiser.Destroyed += _cruiser_Destroyed;
        }

        private void RepairCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            IPvPRepairable repairable = sender.Parse<IPvPRepairCommand>().Repairable;

            // Logging.Log(Tags.REPAIR_MANAGER, $"{repairable}  repairable.RepairCommand.CanExecute: {repairable.RepairCommand.CanExecute}");

            Assert.IsTrue(_repairableToFeedback.ContainsKey(repairable));
            IPvPDroneConsumer droneConsumer = _repairableToFeedback[repairable].DroneConsumer;

            if (repairable.RepairCommand.CanExecute)
            {
                _droneConsumerProvider.ActivateDroneConsumer(droneConsumer);
            }
            else
            {
                _droneConsumerProvider.ReleaseDroneConsumer(droneConsumer);
            }
        }

        private void _cruiser_BuildingStarted(object sender, PvPBuildingStartedEventArgs e)
        {
            AddRepairable(e.StartedBuilding);
        }

        private void _cruiser_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            CleanUpCruiser();
        }

        public void Repair(float deltaTimeInS)
        {
            // Logging.Verbose(Tags.REPAIR_MANAGER, "_repairableToDroneConsumer.Count:  " + _repairableToFeedback.Count);

            foreach (KeyValuePair<IPvPRepairable, IPvPDroneFeedback> pair in _repairableToFeedback)
            {
                IPvPRepairable repairable = pair.Key;
                IPvPDroneConsumer droneConsumer = pair.Value.DroneConsumer;

                if (droneConsumer != null
                    && droneConsumer.State != PvPDroneConsumerState.Idle)
                {
                    // Logging.Verbose(Tags.REPAIR_MANAGER, "About to repair: " + repairable);

                    if (!repairable.RepairCommand.CanExecute)
                    {
                        return;
                    }
                    float healthGained = deltaTimeInS * droneConsumer.NumOfDrones * repairable.HealthGainPerDroneS * PvPBuildSpeedMultipliers.DEFAULT;
                    repairable.RepairCommand.Execute(healthGained);
                }
            }
        }
        public void RemoveCruiser()
        {
            Assert.IsNotNull(_cruiser);
            RemoveRepairable(_cruiser);
        }

        private void AddRepairable(IPvPRepairable repairable)
        {
            // Logging.Log(Tags.REPAIR_MANAGER, "repairable: " + repairable);

            Assert.IsFalse(_repairableToFeedback.ContainsKey(repairable));

            IPvPDroneConsumer droneConsumer = _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR);

            IPvPDroneFeedback droneNumFeedback = _feedbackFactory.CreateFeedback(droneConsumer, repairable.DroneAreaPosition, repairable.DroneAreaSize);
            _repairableToFeedback.Add(repairable, droneNumFeedback);

            if (repairable.RepairCommand.CanExecute)
            {
                _droneConsumerProvider.ActivateDroneConsumer(droneConsumer);
            }

            repairable.Destroyed += Repairable_Destroyed;
            repairable.RepairCommand.CanExecuteChanged += RepairCommand_CanExecuteChanged;
        }

        private void RemoveRepairable(IPvPRepairable repairable)
        {
            // Logging.Log(Tags.REPAIR_MANAGER, "repairable: " + repairable);

            Assert.IsTrue(_repairableToFeedback.ContainsKey(repairable));

            IPvPDroneFeedback droneNumFeedback = _repairableToFeedback[repairable];
            droneNumFeedback.DisposeManagedState();
            _droneConsumerProvider.ReleaseDroneConsumer(droneNumFeedback.DroneConsumer);

            _repairableToFeedback.Remove(repairable);

            repairable.Destroyed -= Repairable_Destroyed;
            repairable.RepairCommand.CanExecuteChanged -= RepairCommand_CanExecuteChanged;
        }

        private void Repairable_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            RemoveRepairable(e.DestroyedTarget);
        }

        public IPvPDroneConsumer GetDroneConsumer(IPvPRepairable repairable)
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
            IList<IPvPRepairable> repairables = _repairableToFeedback.Keys.ToList();

            foreach (IPvPRepairable repairable in repairables)
            {
                RemoveRepairable(repairable);
            }

            _cruiser.Destroyed -= _cruiser_Destroyed;
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;
        }
    }
}
