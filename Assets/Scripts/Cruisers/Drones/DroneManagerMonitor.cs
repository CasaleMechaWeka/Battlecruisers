using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Threading;
using System;
using System.Linq;

namespace BattleCruisers.Cruisers.Drones
{
    public class DroneManagerMonitor : IDroneManagerMonitor, IManagedDisposable
    {
        private readonly IDroneManager _droneManager;
        private readonly IVariableDelayDeferrer _deferrer;
        private int _previousNumOfDrones;

        private const float IDLE_DRONE_CHECK_DEFERRAL_TIME_IN_S = 0.1f;

        public event EventHandler DroneNumIncreased;
        public event EventHandler IdleDrones;

        public DroneManagerMonitor(IDroneManager droneManager, IVariableDelayDeferrer deferrer)
        {
            Helper.AssertIsNotNull(droneManager, deferrer);

            _droneManager = droneManager;
            _deferrer = deferrer;
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

            DeferCheckForIdleDrones();

            _previousNumOfDrones = e.NewNumOfDrones;
        }

        private void DroneConsumers_Changed(object sender, CollectionChangedEventArgs<IDroneConsumer> e)
        {
            if (e.Type == ChangeType.Remove)
            {
                DeferCheckForIdleDrones();
            }
        }

        /// <summary>
        /// As the DroneManager (DM) cleans up a DroneConsumer (DC) all DCs may be briefly idle,
        /// before the DM has a chance to assign the freed up drones to other DCs.  Hence, wait
        /// briefly before checking for idle drones, giving the DM a chance to assign drones.
        /// </summary>
        private void DeferCheckForIdleDrones()
        {
            _deferrer.Defer(CheckForIdleDrones, IDLE_DRONE_CHECK_DEFERRAL_TIME_IN_S);
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