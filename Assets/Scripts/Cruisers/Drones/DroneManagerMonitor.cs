using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace BattleCruisers.Cruisers.Drones
{
    public class DroneManagerMonitor : IDroneManagerMonitor, IManagedDisposable
    {
        private readonly IDroneManager _droneManager;
        private readonly IVariableDelayDeferrer _deferrer;
        private int _previousNumOfDrones;

        private const float IDLE_DRONE_CHECK_DEFERRAL_TIME_IN_S = 0.1f;

        private bool _areDronesIdle;
        private bool AreDronesIdle
        {
            get { return _areDronesIdle; }
            set
            {
                if (_areDronesIdle != value)
                {
                    _areDronesIdle = value;

                    if (_areDronesIdle)
                    {
                        IdleDronesStarted?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        IdleDronesEnded?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler DroneNumIncreased;
        public event EventHandler IdleDronesStarted;
        public event EventHandler IdleDronesEnded;

        public DroneManagerMonitor(IDroneManager droneManager, IVariableDelayDeferrer deferrer)
        {
            Helper.AssertIsNotNull(droneManager, deferrer);

            _droneManager = droneManager;
            _deferrer = deferrer;
            _previousNumOfDrones = _droneManager.NumOfDrones;
            _areDronesIdle = false;

            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            ((INotifyCollectionChanged)_droneManager.DroneConsumers).CollectionChanged += DroneManagerMonitor_CollectionChanged;
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


        private void DroneManagerMonitor_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DeferCheckForIdleDrones();
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
            AreDronesIdle = AreAllDroneConsumersIdle();
        }

        private bool AreAllDroneConsumersIdle()
        {
            return
                _droneManager.DroneConsumers
                    .All(droneConsumer => droneConsumer.State == DroneConsumerState.Idle);
        }

        public void DisposeManagedState()
        {
            _droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
            ((INotifyCollectionChanged)_droneManager.DroneConsumers).CollectionChanged -= DroneManagerMonitor_CollectionChanged;
        }
    }
}