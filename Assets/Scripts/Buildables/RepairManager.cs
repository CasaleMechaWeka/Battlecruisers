using System;
using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public class RepairManager : IRepairManager
    {
        private readonly ICruiser _cruiser;
        private readonly IDroneConsumerProvider _droneConsumerProvider;
        private readonly IDictionary<IRepairable, IDroneConsumer> _repairableToDroneConsumer;

        private const int NUM_OF_DRONES_REQUIRED_FOR_REPAIR = 1;

        public RepairManager(ICruiser cruiser)
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
            Assert.IsFalse(_repairableToDroneConsumer.ContainsKey(repairable));

            IDroneConsumer droneConsumer 
                = repairable.RepairCommand.CanExecute ?
                    _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR) :
                    null;
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
            IRepairable repairable = sender.Parse<IRepairable>();

            Assert.IsTrue(_repairableToDroneConsumer.ContainsKey(repairable));
            IDroneConsumer droneConsumer = _repairableToDroneConsumer[repairable];

            if (repairable.RepairCommand.CanExecute
                && droneConsumer == null)
            {
                droneConsumer = _droneConsumerProvider.RequestDroneConsumer(NUM_OF_DRONES_REQUIRED_FOR_REPAIR);
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
            foreach (KeyValuePair<IRepairable, IDroneConsumer> pair in _repairableToDroneConsumer)
            {
                IRepairable repairable = pair.Key;
                IDroneConsumer droneConsumer = pair.Value;

                if (droneConsumer != null
                    && droneConsumer.State != DroneConsumerState.Idle)
                {
                    Assert.IsTrue(repairable.RepairCommand.CanExecute);
                    float healthGained = deltaTimeInS * droneConsumer.NumOfDrones * repairable.HealthGainPerDroneS;
                    repairable.RepairCommand.Execute(healthGained);
                }
            }
        }

        public void Dispose()
        {
            foreach (IRepairable repairable in _repairableToDroneConsumer.Keys)
            {
                RemoveRepairable(repairable);
            }

            CleanUpCruiser();
        }

		private void RemoveRepairable(IRepairable repairable)
		{
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
            _cruiser.Destroyed -= _cruiser_Destroyed;
            _cruiser.StartedConstruction -= _cruiser_StartedConstruction;
        }
    }
}
