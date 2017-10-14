using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Repairables
{
    /// <summary>
    /// Keeps track of all repairables (cruiser, buildings).  
    /// 
    /// When they become repairable (ie, damaged), creates a drone consumer.
    /// 
    /// When they are no longer repairable (ie, fully repaired), releases their
    /// drone consumer.
    /// </summary>
    public class RepairManager : IRepairManager
    {
        private readonly IDeferrer _deferrer;
        private ICruiser _cruiser;
        private IDroneConsumerProvider _droneConsumerProvider;
        private IDictionary<IRepairable, IDroneConsumer> _repairableToDroneConsumer;

        private const int NUM_OF_DRONES_REQUIRED_FOR_REPAIR = 1;

        public RepairManager(IDeferrer deferrer)
        {
            Assert.IsNotNull(deferrer);
            _deferrer = deferrer;
        }

        // Not constructor because of circular dependency between Cruiser and RepairManager
        public void Initialise(ICruiser cruiser)
        {
            Helper.AssertIsNotNull(cruiser, cruiser.DroneConsumerProvider);

            _cruiser = cruiser;
            _droneConsumerProvider = _cruiser.DroneConsumerProvider;

            _repairableToDroneConsumer = new Dictionary<IRepairable, IDroneConsumer>();

            AddRepairable(_cruiser);

            _cruiser.StartedConstruction += _cruiser_StartedConstruction;
            _cruiser.Destroyed += _cruiser_Destroyed;
        }

        private void Repairable_Destroyed(object sender, DestroyedEventArgs e)
        {
            RemoveRepairable(e.DestroyedTarget);
        }

        private void RepairCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            IRepairable repairable = sender.Parse<IRepairCommand>().Repairable;

            Logging.Log(Tags.REPAIR_MANAGER, "RepairCommand_CanExecuteChanged() " + repairable);

            Assert.IsTrue(_repairableToDroneConsumer.ContainsKey(repairable));
            IDroneConsumer droneConsumer = _repairableToDroneConsumer[repairable];

            if (repairable.RepairCommand.CanExecute)
            {
                _droneConsumerProvider.ActivateDroneConsumer(droneConsumer);
            }
            else
            {
                _droneConsumerProvider.ReleaseDroneConsumer(droneConsumer);
            }
        }

        private void _cruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            AddRepairable(e.Buildable);
        }

        private void _cruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            CleanUpCruiser();
        }

        public void Repair(float deltaTimeInS)
        {
            Logging.Verbose(Tags.REPAIR_MANAGER, "Repair()  _repairableToDroneConsumer.Count:  " + _repairableToDroneConsumer.Count);

			foreach (KeyValuePair<IRepairable, IDroneConsumer> pair in _repairableToDroneConsumer)
            {
                IRepairable repairable = pair.Key;
                IDroneConsumer droneConsumer = pair.Value;

                if (droneConsumer != null
                    && droneConsumer.State != DroneConsumerState.Idle)
                {
                    Logging.Log(Tags.REPAIR_MANAGER, "Repair()  About to repair: " + repairable);

                    Assert.IsTrue(repairable.RepairCommand.CanExecute);
                    float healthGained = deltaTimeInS * droneConsumer.NumOfDrones * repairable.HealthGainPerDroneS;

                    // Defer, as this may bring the repairable to full health, which 
                    // sets its DroneConsumer to null, which modifies this enumerable :)
                    _deferrer.Defer(() => repairable.RepairCommand.Execute(healthGained));
                }
            }
        }

        public void Dispose()
        {
            CleanUpCruiser();
        }

        private void CleanUpCruiser()
        {
            IList<IRepairable> repairables = _repairableToDroneConsumer.Keys.ToList();

            foreach (IRepairable repairable in repairables)
			{
				RemoveRepairable(repairable);
			}

            _cruiser.Destroyed -= _cruiser_Destroyed;
            _cruiser.StartedConstruction -= _cruiser_StartedConstruction;
        }

        private void AddRepairable(IRepairable repairable)
        {
            Logging.Log(Tags.REPAIR_MANAGER, "AddRepairable(): repairable: " + repairable);

            Assert.IsFalse(_repairableToDroneConsumer.ContainsKey(repairable));

            IDroneConsumer droneConsumer = _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR, isHighPriority: false);

            if (repairable.RepairCommand.CanExecute)
            {
				_droneConsumerProvider.ActivateDroneConsumer(droneConsumer);
            }

            _repairableToDroneConsumer.Add(repairable, droneConsumer);

            repairable.Destroyed += Repairable_Destroyed;
            repairable.RepairCommand.CanExecuteChanged += RepairCommand_CanExecuteChanged;
        }

        private void RemoveRepairable(IRepairable repairable)
        {
            Logging.Log(Tags.REPAIR_MANAGER, "RemoveRepairable(): repairable: " + repairable);

            Assert.IsTrue(_repairableToDroneConsumer.ContainsKey(repairable));

            IDroneConsumer droneConsumer = _repairableToDroneConsumer[repairable];
            _droneConsumerProvider.ReleaseDroneConsumer(droneConsumer);

            _repairableToDroneConsumer.Remove(repairable);

            repairable.Destroyed -= Repairable_Destroyed;
            repairable.RepairCommand.CanExecuteChanged -= RepairCommand_CanExecuteChanged;
        }

		public IDroneConsumer GetDroneConsumer(IRepairable repairable)
		{
			Assert.IsTrue(_repairableToDroneConsumer.ContainsKey(repairable));
			return _repairableToDroneConsumer[repairable];
		}
    }
}
