using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Cruisers
{
    public class PvPDronesPanelInitialiser : MonoBehaviour
    {
        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private PvPDronesDisplayer _dronesDisplayer;
#pragma warning restore CS0414  // Variable is assigned but never used

        public Image highlight;

        public IPvPHighlightable Initialise(IPvPDroneManager droneManager, IPvPDroneManagerMonitor droneManagerMonitor)
        {
            PvPHelper.AssertIsNotNull(highlight, droneManager, droneManagerMonitor);

            IPvPGameObject highlightGameObject = new PvPGameObjectBC(highlight.gameObject);

            PvPNumOfDronesPanelInitialiser numOfDronesPanel = GetComponentInChildren<PvPNumOfDronesPanelInitialiser>();
            IPvPNumberDisplay twoDigitDisplayer = numOfDronesPanel.CreateTwoDigitDisplay();

            _dronesDisplayer = new PvPDronesDisplayer(droneManager, droneManagerMonitor, twoDigitDisplayer, highlightGameObject);

            PvPHighlightable highlightable = GetComponent<PvPHighlightable>();
            Assert.IsNotNull(highlightable);
            highlightable.Initialise();
            return highlightable;
        }


        public IPvPHighlightable Initialise()
        {
            PvPHelper.AssertIsNotNull(highlight);

            IPvPGameObject highlightGameObject = new PvPGameObjectBC(highlight.gameObject);

            PvPNumOfDronesPanelInitialiser numOfDronesPanel = GetComponentInChildren<PvPNumOfDronesPanelInitialiser>();
            IPvPNumberDisplay twoDigitDisplayer = numOfDronesPanel.CreateTwoDigitDisplay();

            _dronesDisplayer = new PvPDronesDisplayer(twoDigitDisplayer, highlightGameObject);

            PvPHighlightable highlightable = GetComponent<PvPHighlightable>();
            Assert.IsNotNull(highlightable);
            highlightable.Initialise();
            return highlightable;
        }
    }
}