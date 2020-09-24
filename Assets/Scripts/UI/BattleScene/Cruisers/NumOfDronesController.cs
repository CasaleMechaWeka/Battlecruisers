using System;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Cruisers
{
    public class NumOfDronesController : Togglable, IManagedDisposable, IButton, IPointerClickHandler
    {
        private IDroneManager _droneManager;

        public Text numOfDronesText;

        public event EventHandler Clicked;

        private int NumOfDrones
        {
            set
            {
                numOfDronesText.text = value.ToString();
            }
        }

        public void Initialise(IDroneManager droneManager)
        {
            base.Initialise();

            Helper.AssertIsNotNull(numOfDronesText, droneManager);

            _droneManager = droneManager;
            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
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
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
