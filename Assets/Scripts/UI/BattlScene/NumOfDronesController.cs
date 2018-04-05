using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene
{
    public class NumOfDronesController : MonoBehaviour, IManagedDisposable
    {
        private IDroneManager _droneManager;

        public Text numOfDronesText;

        private int NumOfDrones
        {
            set
            {
                numOfDronesText.text = value.ToString();
            }
        }

        public void Initialise(IDroneManager droneManager)
        {
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
    }
}
