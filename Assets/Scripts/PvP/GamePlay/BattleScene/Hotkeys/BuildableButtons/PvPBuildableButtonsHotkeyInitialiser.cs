using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons
{
    public class PvPBuildableButtonsHotkeyInitialiser : MonoBehaviour
    {
        private PvPDummyBuildableButton _nullButton;

        // Keep references to avoid garbage collection
        private IPvPManagedDisposable _factoriesListener, _defensivesListener, _offensivesListener, _tacticalsListener, _ultrasListener, _aircraftListener, _shipsListener;

        [Header("Factories")]
        public PvPBuildingButtonController droneStationButton;
        public PvPBuildingButtonController airFactoryButton, navalFactoryButton, droneStation4Button, droneStation8Button;

        [Header("Defensives")]
        public PvPBuildingButtonController shipTurretButton;
        public PvPBuildingButtonController airTurretButton, mortarButton, samSiteButton, teslaCoilButton;

        [Header("Offensives")]
        public PvPBuildingButtonController artilleryButton;
        public PvPBuildingButtonController railgunButton, rocketLauncherButton, MLRSButton, gatlingMortarButton;

        [Header("Tacticals")]
        public PvPBuildingButtonController shieldButton;
        public PvPBuildingButtonController boosterButton, stealthGeneratorButton, spySatelliteButton, controlTowerButton;

        [Header("Ultras")]
        public PvPBuildingButtonController deathstarButton;
        public PvPBuildingButtonController nukeLauncherButton, ultraliskButton, kamikazeSignalButton, broadsidesButton;

        [Header("Aircraft")]
        public PvPUnitButtonController bomberButton;
        public PvPUnitButtonController gunshipButton, fighterButton, steamCopterButton;

        [Header("Ships")]
        public PvPUnitButtonController attackBoatButton;
        public PvPUnitButtonController frigateButton, destroyerButton, archonButton, attackRIBButton;

        private void Awake()
        {
            // Locked buttons may be destroyed, so null check before they are destroyed
            PvPHelper.AssertIsNotNull(droneStationButton, airFactoryButton, navalFactoryButton);
            PvPHelper.AssertIsNotNull(shipTurretButton, airTurretButton, mortarButton, samSiteButton, teslaCoilButton);
            PvPHelper.AssertIsNotNull(artilleryButton, railgunButton, rocketLauncherButton);
            PvPHelper.AssertIsNotNull(shieldButton, boosterButton, stealthGeneratorButton, spySatelliteButton, controlTowerButton);
            PvPHelper.AssertIsNotNull(deathstarButton, nukeLauncherButton, ultraliskButton, kamikazeSignalButton, broadsidesButton);
            PvPHelper.AssertIsNotNull(bomberButton, gunshipButton, fighterButton);
            PvPHelper.AssertIsNotNull(attackBoatButton, frigateButton, destroyerButton, archonButton);

            _nullButton = new PvPDummyBuildableButton();
        }

        public void Initialise(IPvPHotkeyDetector hotkeyDetector)
        {
            Assert.IsNotNull(hotkeyDetector);

            _factoriesListener = new PvPFactoryButtonsHotkeyListener(hotkeyDetector, droneStationButton, airFactoryButton, navalFactoryButton,
            UseNullButtonIfNeeded(droneStation4Button),
            UseNullButtonIfNeeded(droneStation8Button));
            _defensivesListener
                = new PvPDefensiveButtonsHotkeyListener(
                    hotkeyDetector,
                    shipTurretButton, // always unlocked
                    airTurretButton, // always unlocked
                    UseNullButtonIfNeeded(mortarButton),
                    UseNullButtonIfNeeded(samSiteButton),
                    UseNullButtonIfNeeded(teslaCoilButton));
            _offensivesListener
                = new PvPOffensiveButtonsHotkeyListener(
                    hotkeyDetector,
                    artilleryButton, // always unlocked
                    UseNullButtonIfNeeded(railgunButton),
                    UseNullButtonIfNeeded(rocketLauncherButton),
                    UseNullButtonIfNeeded(MLRSButton),
                    UseNullButtonIfNeeded(gatlingMortarButton));
            _tacticalsListener
                = new PvPTacticalButtonsHotkeyListener(
                    hotkeyDetector,
                    UseNullButtonIfNeeded(shieldButton),
                    UseNullButtonIfNeeded(boosterButton),
                    UseNullButtonIfNeeded(stealthGeneratorButton),
                    UseNullButtonIfNeeded(spySatelliteButton),
                    UseNullButtonIfNeeded(controlTowerButton));
            _ultrasListener
                = new PvPUltraButtonsHotkeyListener(
                    hotkeyDetector,
                    UseNullButtonIfNeeded(deathstarButton),
                    UseNullButtonIfNeeded(nukeLauncherButton),
                    UseNullButtonIfNeeded(ultraliskButton),
                    UseNullButtonIfNeeded(kamikazeSignalButton),
                    UseNullButtonIfNeeded(broadsidesButton));
            _aircraftListener
                = new PvPAircraftButtonsHotkeyListener(
                    hotkeyDetector,
                    bomberButton, // always unlocked
                    UseNullButtonIfNeeded(gunshipButton),
                    UseNullButtonIfNeeded(fighterButton),
                    UseNullButtonIfNeeded(steamCopterButton));
            _shipsListener
                = new PvPShipButtonsHotkeyListener(
                    hotkeyDetector,
                    attackBoatButton, // always unlocked
                    UseNullButtonIfNeeded(frigateButton),
                    UseNullButtonIfNeeded(destroyerButton),
                    UseNullButtonIfNeeded(archonButton),
                    UseNullButtonIfNeeded(attackRIBButton));
        }

        // Locked buttons may have been destroyed, so replace these with a dummy button
        private IPvPBuildableButton UseNullButtonIfNeeded(PvPBuildableButtonController realButton)
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