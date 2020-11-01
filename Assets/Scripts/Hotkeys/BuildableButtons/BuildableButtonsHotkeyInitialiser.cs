using BattleCruisers.Hotkeys;
using BattleCruisers.Hotkeys.BuildableButtons;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Hotkeys.BuildableButtons
{
    public class BuildableButtonsHotkeyInitialiser : MonoBehaviour
    {
        // Keep references to avoid garbage collection
        private IManagedDisposable _shipButtonsHotkeyListener, _factoryButtonsHotkeyListener, _defensiveButtonsHotkeyListener;

        [Header("Factories")]
        public BuildingButtonController droneStationButton;
        public BuildingButtonController airFactoryButton, navalFactoryButton;
        [Header("Defensives")]
        public BuildingButtonController shipTurretButton;
        public BuildingButtonController airTurretButton, mortarButton, samSiteButton, teslaCoilButton;
        [Header("Ships")]
        public UnitButtonController attackBoatButton;
        public UnitButtonController frigateButton, destroyerButton, archonButton;

        public void Initialise(IHotkeyDetector hotkeyDetector)
        {
            Helper.AssertIsNotNull(attackBoatButton, frigateButton, destroyerButton, archonButton);
            Helper.AssertIsNotNull(droneStationButton, airFactoryButton, navalFactoryButton);
            Helper.AssertIsNotNull(shipTurretButton, airTurretButton, mortarButton, samSiteButton, teslaCoilButton);
            Assert.IsNotNull(hotkeyDetector);

            _factoryButtonsHotkeyListener = new FactoryButtonsHotkeyListener(hotkeyDetector, droneStationButton, airFactoryButton, navalFactoryButton);
            _defensiveButtonsHotkeyListener = new DefensiveButtonsHotkeyListener(hotkeyDetector, shipTurretButton, airTurretButton, mortarButton, samSiteButton, teslaCoilButton);
            _shipButtonsHotkeyListener = new ShipButtonsHotkeyListener(hotkeyDetector, attackBoatButton, frigateButton, destroyerButton, archonButton);
        }
    }
}