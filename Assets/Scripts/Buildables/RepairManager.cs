using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public class RepairManager : IRepairManager
    {
        private ICruiser _cruiser;
        private IDroneConsumerProvider _droneConsumerProvider;
        private IDictionary<IRepairable, IDroneConsumer> _repairableToDroneConsumer;

        private const int NUM_OF_DRONES_REQUIRED_FOR_REPAIR = 1;

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

        private void AddRepairable(IRepairable repairable)
        {
            Logging.Log(Tags.REPAIR_MANAGER, "AddRepairable(): repairable: " + repairable);

            Assert.IsFalse(_repairableToDroneConsumer.ContainsKey(repairable));

            IDroneConsumer droneConsumer = null;
            if (repairable.RepairCommand.CanExecute)
            {
                droneConsumer = _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR);
                _droneConsumerProvider.ActivateDroneConsumer(droneConsumer);
            }

            _repairableToDroneConsumer.Add(repairable, droneConsumer);

            repairable.Destroyed += Repairable_Destroyed;
            repairable.RepairCommand.CanExecuteChanged += RepairCommand_CanExecuteChanged;
        }

        private void Repairable_Destroyed(object sender, DestroyedEventArgs e)
        {
            RemoveRepairable(e.DestroyedTarget);
        }

        private void RepairCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            IRepairable repairable = sender.Parse<IRepairCommand>().Repairable;
            Assert.IsNotNull(repairable);

            Logging.Log(Tags.REPAIR_MANAGER, "RepairCommand_CanExecuteChanged() " + repairable);

            Assert.IsTrue(_repairableToDroneConsumer.ContainsKey(repairable));
            IDroneConsumer droneConsumer = _repairableToDroneConsumer[repairable];

            if (repairable.RepairCommand.CanExecute
                && droneConsumer == null)
            {
                droneConsumer = _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR);
                _droneConsumerProvider.ActivateDroneConsumer(droneConsumer);
            }
            else if (!repairable.RepairCommand.CanExecute
                && droneConsumer != null)
            {
                _droneConsumerProvider.ReleaseDroneConsumer(droneConsumer);
                droneConsumer = null;
            }

            _repairableToDroneConsumer[repairable] = droneConsumer;
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
                    repairable.RepairCommand.Execute(healthGained);
                }
            }
        }

        public void Dispose()
        {
            CleanUpCruiser();
        }

		private void RemoveRepairable(IRepairable repairable)
		{
			Logging.Log(Tags.REPAIR_MANAGER, "RemoveRepairable(): repairable: " + repairable);

			Assert.IsTrue(_repairableToDroneConsumer.ContainsKey(repairable));

			IDroneConsumer droneConsumer = _repairableToDroneConsumer[repairable];
			if (droneConsumer != null)
			{
				_droneConsumerProvider.ReleaseDroneConsumer(droneConsumer);
			}

			_repairableToDroneConsumer.Remove(repairable);

			repairable.Destroyed -= Repairable_Destroyed;
			repairable.RepairCommand.CanExecuteChanged -= RepairCommand_CanExecuteChanged;
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
    }
}
