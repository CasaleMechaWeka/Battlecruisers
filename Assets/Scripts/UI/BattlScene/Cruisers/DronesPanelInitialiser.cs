using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Cruisers
{
    public class DronesPanelInitialiser : MonoBehaviour
    {
        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private DronesDisplayer _dronesDisplayer;
#pragma warning restore CS0414  // Variable is assigned but never used

        public Image highlight;

        public IHighlightable Initialise(IDroneManager droneManager, IDroneManagerMonitor droneManagerMonitor)
        {
            Helper.AssertIsNotNull(highlight, droneManager, droneManagerMonitor);

            IGameObject highlightGameObject = new GameObjectBC(highlight.gameObject);

            NumOfDronesPanelInitialiser numOfDronesPanel = GetComponentInChildren<NumOfDronesPanelInitialiser>();
            INumberDisplay twoDigitDisplayer = numOfDronesPanel.CreateTwoDigitDisplay();

            _dronesDisplayer = new DronesDisplayer(droneManager, droneManagerMonitor, twoDigitDisplayer, highlightGameObject);

            Highlightable highlightable = GetComponent<Highlightable>();
            Assert.IsNotNull(highlightable);
            highlightable.Initialise();
            return highlightable;
        }
    }
}