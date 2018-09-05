using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using System;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones
{
    public class DroneManagerMonitor : IDroneManagerMonitor, IManagedDisposable
    {
        private readonly IDroneManager _droneManager;
        private int _previousNumOfDrones;

        public event EventHandler DroneNumIncreased;
        public event EventHandler IdleDrones;

        public DroneManagerMonitor(IDroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);

            _droneManager = droneManager;
            _previousNumOfDrones = _droneManager.NumOfDrones;

            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            _droneManager.DroneConsumers.Changed += DroneConsumers_Changed;
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            if (e.NewNumOfDrones > _previousNumOfDrones
                && DroneNumIncreased != null)
            {
                DroneNumIncreased.Invoke(this, EventArgs.Empty);
            }

            CheckForIdleDrones();

            _previousNumOfDrones = e.NewNumOfDrones;
        }

        private void DroneConsumers_Changed(object sender, CollectionChangedEventArgs<IDroneConsumer> e)
        {
            if (e.Type == ChangeType.Remove)
            {
                CheckForIdleDrones();
            }
        }

        private void CheckForIdleDrones()
        {
            if (AreAllDroneConsumersIdle()
                && IdleDrones != null)
            {
                IdleDrones.Invoke(this, EventArgs.Empty);
            }
        }

        private bool AreAllDroneConsumersIdle()
        {
            return
                _droneManager.DroneConsumers.Items
                    .All(droneConsumer => droneConsumer.State == DroneConsumerState.Idle);
        }

        public void DisposeManagedState()
        {
            _droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
            _droneManager.DroneConsumers.Changed -= DroneConsumers_Changed;
        }
    }
}