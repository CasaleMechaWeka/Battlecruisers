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
        private IManagedDisposable _factoriesListener, _defensivesListener, _offensivesListener, _tacticalsListener, _ultrasListener, _aircraftListener, _shipsListener;

        [Header("Factories")]
        public BuildingButtonController droneStationButton;
        public BuildingButtonController airFactoryButton, navalFactoryButton;
        
        [Header("Defensives")]
        public BuildingButtonController shipTurretButton;
        public BuildingButtonController airTurretButton, mortarButton, samSiteButton, teslaCoilButton;

        [Header("Offensives")]
        public BuildingButtonController artilleryButton;
        public BuildingButtonController railgunButton, rocketLauncherButton;

        [Header("Tacticals")]
        public BuildingButtonController shieldButton;
        public BuildingButtonController boosterButton, stealthGeneratorButton, spySatelliteButton, controlTowerButton;

        [Header("Ultras")]
        public BuildingButtonController deathstarButton;
        public BuildingButtonController nukeLauncherButton, ultraliskButton, kamikazeSignalButton, broadsidesButton;

        [Header("Aircraft")]
        public UnitButtonController bomberButton;
        public UnitButtonController gunshipButton, fighterButton;

        [Header("Ships")]
        public UnitButtonController attackBoatButton;
        public UnitButtonController frigateButton, destroyerButton, archonButton;

        public void Initialise(IHotkeyDetector hotkeyDetector)
        {
            Helper.AssertIsNotNull(droneStationButton, airFactoryButton, navalFactoryButton);
            Helper.AssertIsNotNull(shipTurretButton, airTurretButton, mortarButton, samSiteButton, teslaCoilButton);
            Helper.AssertIsNotNull(artilleryButton, railgunButton, rocketLauncherButton);
            Helper.AssertIsNotNull(shieldButton, boosterButton, stealthGeneratorButton, spySatelliteButton, controlTowerButton);
            Helper.AssertIsNotNull(deathstarButton, nukeLauncherButton, ultraliskButton, kamikazeSignalButton, broadsidesButton);
            Helper.AssertIsNotNull(bomberButton, gunshipButton, fighterButton);
            Helper.AssertIsNotNull(attackBoatButton, frigateButton, destroyerButton, archonButton);
            Assert.IsNotNull(hotkeyDetector);

            _factoriesListener = new FactoryButtonsHotkeyListener(hotkeyDetector, droneStationButton, airFactoryButton, navalFactoryButton);
            _defensivesListener = new DefensiveButtonsHotkeyListener(hotkeyDetector, shipTurretButton, airTurretButton, mortarButton, samSiteButton, teslaCoilButton);
            _offensivesListener = new OffensiveButtonsHotkeyListener(hotkeyDetector, artilleryButton, railgunButton, rocketLauncherButton);
            _tacticalsListener = new TacticalButtonsHotkeyListener(hotkeyDetector, shieldButton, boosterButton, stealthGeneratorButton, spySatelliteButton, controlTowerButton);
            _ultrasListener = new UltraButtonsHotkeyListener(hotkeyDetector, deathstarButton, nukeLauncherButton, ultraliskButton, kamikazeSignalButton, broadsidesButton);
            _aircraftListener = new AircraftButtonsHotkeyListener(hotkeyDetector, bomberButton, gunshipButton, fighterButton);
            _shipsListener = new ShipButtonsHotkeyListener(hotkeyDetector, attackBoatButton, frigateButton, destroyerButton, archonButton);
        }
    }
}