using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Tutorial.Highlighting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Cruisers.Drones;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Cruisers
{
    public class PvPDronesPanelInitialiser : MonoBehaviour
    {
        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private PvPDronesDisplayer _dronesDisplayer;
#pragma warning restore CS0414  // Variable is assigned but never used

        public Image highlight;

        public IHighlightable Initialise(IDroneManager droneManager, IDroneManagerMonitor droneManagerMonitor)
        {
            PvPHelper.AssertIsNotNull(highlight, droneManager, droneManagerMonitor);

            IGameObject highlightGameObject = new GameObjectBC(highlight.gameObject);

            NumOfDronesPanelInitialiser numOfDronesPanel = GetComponentInChildren<NumOfDronesPanelInitialiser>();
            INumberDisplay twoDigitDisplayer = numOfDronesPanel.CreateTwoDigitDisplay();

            _dronesDisplayer = new PvPDronesDisplayer(droneManager, droneManagerMonitor, twoDigitDisplayer, highlightGameObject);

            Highlightable highlightable = GetComponent<Highlightable>();
            Assert.IsNotNull(highlightable);
            highlightable.Initialise();
            return highlightable;
        }


        public IHighlightable Initialise(PvPCruiser playerCruiser)
        {
            PvPHelper.AssertIsNotNull(highlight);

            IGameObject highlightGameObject = new GameObjectBC(highlight.gameObject);

            NumOfDronesPanelInitialiser numOfDronesPanel = GetComponentInChildren<NumOfDronesPanelInitialiser>();
            INumberDisplay twoDigitDisplayer = numOfDronesPanel.CreateTwoDigitDisplay();

            _dronesDisplayer = new PvPDronesDisplayer(playerCruiser, twoDigitDisplayer, highlightGameObject);

            Highlightable highlightable = GetComponent<Highlightable>();
            Assert.IsNotNull(highlightable);
            highlightable.Initialise();
            return highlightable;
        }
    }
}