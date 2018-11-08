using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Cruisers
{
    public class DronesPanelInitialiser : MonoBehaviour
    {
        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private DronesDisplayer _dronesDisplayer;
#pragma warning restore CS0414  // Variable is assigned but never used

        public void Initialise(IDroneManager droneManager, IDroneManagerMonitor droneManagerMonitor)
        {
            Helper.AssertIsNotNull(droneManager, droneManagerMonitor);

            Image highlight = transform.FindNamedComponent<Image>("Highlight");
            IGameObject highlightGameObject = new GameObjectBC(highlight.gameObject);

            NumOfDronesPanelInitialiser numOfDronesPanel = GetComponentInChildren<NumOfDronesPanelInitialiser>();
            INumberDisplay twoDigitDisplayer = numOfDronesPanel.CreateTwoDigitDisplay();

            _dronesDisplayer = new DronesDisplayer(droneManager, droneManagerMonitor, twoDigitDisplayer, highlightGameObject);
        }
    }
}