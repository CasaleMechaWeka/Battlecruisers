using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class BuildableButtonsHotkeyInitialiser : MonoBehaviour
    {
        private DummyBuildableButton _nullButton;

        // Keep references to avoid garbage collection
        private IManagedDisposable _factoriesListener, _defensivesListener, _offensivesListener, _tacticalsListener, _ultrasListener, _aircraftListener, _shipsListener;

        [Header("Factories")]
        public BuildingButtonController droneStationButton;
        public BuildingButtonController airFactoryButton, navalFactoryButton, droneStation4Button, droneStation8Button;
        
        [Header("Defensives")]
        public BuildingButtonController shipTurretButton;
        public BuildingButtonController airTurretButton, mortarButton, samSiteButton, teslaCoilButton;

        [Header("Offensives")]
        public BuildingButtonController artilleryButton;
        public BuildingButtonController railgunButton, rocketLauncherButton, MLRSButton, gatlingMortarButton;

        [Header("Tacticals")]
        public BuildingButtonController shieldButton;
        public BuildingButtonController boosterButton, stealthGeneratorButton, spySatelliteButton, controlTowerButton;

        [Header("Ultras")]
        public BuildingButtonController deathstarButton;
        public BuildingButtonController nukeLauncherButton, ultraliskButton, kamikazeSignalButton, broadsidesButton;

        [Header("Aircraft")]
        public UnitButtonController bomberButton;
        public UnitButtonController gunshipButton, fighterButton, steamCopterButton;

        [Header("Ships")]
        public UnitButtonController attackBoatButton;
        public UnitButtonController frigateButton, destroyerButton, archonButton, attackRIBButton;

        private void Awake()
        {
            // Locked buttons may be destroyed, so null check before they are destroyed
            Helper.AssertIsNotNull(droneStationButton, airFactoryButton, navalFactoryButton);
            Helper.AssertIsNotNull(shipTurretButton, airTurretButton, mortarButton, samSiteButton, teslaCoilButton);
            Helper.AssertIsNotNull(artilleryButton, railgunButton, rocketLauncherButton);
            Helper.AssertIsNotNull(shieldButton, boosterButton, stealthGeneratorButton, spySatelliteButton, controlTowerButton);
            Helper.AssertIsNotNull(deathstarButton, nukeLauncherButton, ultraliskButton, kamikazeSignalButton, broadsidesButton);
            Helper.AssertIsNotNull(bomberButton, gunshipButton, fighterButton);
            Helper.AssertIsNotNull(attackBoatButton, frigateButton, destroyerButton, archonButton);

            _nullButton = new DummyBuildableButton();
        }

        public void Initialise(IHotkeyDetector hotkeyDetector)
        {
            Assert.IsNotNull(hotkeyDetector);

            _factoriesListener = new FactoryButtonsHotkeyListener(hotkeyDetector, droneStationButton, airFactoryButton, navalFactoryButton,
            UseNullButtonIfNeeded(droneStation4Button),
            UseNullButtonIfNeeded(droneStation8Button));
            _defensivesListener 
                = new DefensiveButtonsHotkeyListener(
                    hotkeyDetector, 
                    shipTurretButton, // always unlocked
                    airTurretButton, // always unlocked
                    UseNullButtonIfNeeded(mortarButton),
                    UseNullButtonIfNeeded(samSiteButton),
                    UseNullButtonIfNeeded(teslaCoilButton));
            _offensivesListener 
                = new OffensiveButtonsHotkeyListener(
                    hotkeyDetector, 
                    artilleryButton, // always unlocked
                    UseNullButtonIfNeeded(railgunButton),
                    UseNullButtonIfNeeded(rocketLauncherButton),
                    UseNullButtonIfNeeded(MLRSButton),
                    UseNullButtonIfNeeded(gatlingMortarButton));
            _tacticalsListener 
                = new TacticalButtonsHotkeyListener(
                    hotkeyDetector,
                    UseNullButtonIfNeeded(shieldButton), 
                    UseNullButtonIfNeeded(boosterButton), 
                    UseNullButtonIfNeeded(stealthGeneratorButton), 
                    UseNullButtonIfNeeded(spySatelliteButton),
                    UseNullButtonIfNeeded(controlTowerButton));
            _ultrasListener 
                = new UltraButtonsHotkeyListener(
                    hotkeyDetector,
                    UseNullButtonIfNeeded(deathstarButton), 
                    UseNullButtonIfNeeded(nukeLauncherButton), 
                    UseNullButtonIfNeeded(ultraliskButton), 
                    UseNullButtonIfNeeded(kamikazeSignalButton),
                    UseNullButtonIfNeeded(broadsidesButton));
            _aircraftListener 
                = new AircraftButtonsHotkeyListener(
                    hotkeyDetector, 
                    bomberButton, // always unlocked
                    UseNullButtonIfNeeded(gunshipButton),
                    UseNullButtonIfNeeded(fighterButton),
                    UseNullButtonIfNeeded(steamCopterButton));
            _shipsListener 
                = new ShipButtonsHotkeyListener(
                    hotkeyDetector, 
                    attackBoatButton, // always unlocked
                    UseNullButtonIfNeeded(frigateButton), 
                    UseNullButtonIfNeeded(destroyerButton), 
                    UseNullButtonIfNeeded(archonButton),
                    UseNullButtonIfNeeded(attackRIBButton));
        }

        // Locked buttons may have been destroyed, so replace these with a dummy button
        private IBuildableButton UseNullButtonIfNeeded(BuildableButtonController realButton)
        {
            // Destroyed Monobehaviour == null. Destroyed interface (eg: ), != null. => Need Monobehaviour (BuildableButtonController)
            if (realButton != null)
            {
                return realButton;
            }
            else
            {
                return _nullButton;
            }
        }
    }
}