using System;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Cruisers
{
    // FELIX  Abstract and test???
    public class NumOfDronesControllerNEW : UIElement, IManagedDisposable, IButton, IPointerClickHandler
    {
        private IDroneManager _droneManager;
        private IDroneManagerMonitor _droneManagerMonitor;

        public Text numOfDronesText;

        public event EventHandler Clicked;

        private int NumOfDrones
        {
            set
            {
                numOfDronesText.text = value.ToString();
            }
        }

        public void Initialise(IDroneManager droneManager, IDroneManagerMonitor droneManagerMonitor)
        {
            base.Initialise();

            Helper.AssertIsNotNull(numOfDronesText, droneManager, droneManagerMonitor);

            _droneManager = droneManager;
            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;

            _droneManagerMonitor = droneManagerMonitor;
            // FELIX  Also need to know when no longer have idle drones :/
            //_droneManagerMonitor.IdleDrones

            NumOfDrones = _droneManager.NumOfDrones;
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            NumOfDrones = e.NewNumOfDrones;
        }

        public void DisposeManagedState()
        {
            _droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
