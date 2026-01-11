using BattleCruisers.Data.Models;
using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class HotkeyRowEnabledEventArgs : EventArgs
    {
        public HotkeyRow RowEnabled { get; }

        public HotkeyRowEnabledEventArgs(HotkeyRow rowEnabled)
        {
            Assert.IsNotNull(rowEnabled);
            RowEnabled = rowEnabled;
        }
    }

    public class HotkeysPanel : Panel
    {
        private HotkeysModel _hotkeysModel;

        [Header("Navigation")]
        public HotkeyRow playerCruiserRow;
        public HotkeyRow overviewRow, enemyCruiserRow;

        [Header("Game speed")]
        public HotkeyRow slowMotionRow;
        public HotkeyRow normalSpeedRow, fastForwardRow, toggleSpeedRow;

        [Header("Building categories")]
        public HotkeyRow factoriesRow;
        public HotkeyRow defensivesRow, offensivesRow, tacticalsRow, ultrasRow;

        [Header("Buildable Slot")]
        public HotkeyRow Slot1;
        public HotkeyRow Slot2, Slot3, Slot4, Slot5;
        /*
        [Header("Factories")]
        public HotkeyRow droneStationRow;
        public HotkeyRow airFactoryRow, navalFactoryRow, droneStation4Row, droneStation8Row;
        
        [Header("Defensives")]
        public HotkeyRow shipTurretRow;
        public HotkeyRow airTurretRow, mortarRow, samSiteRow, teslaCoilRow;

        [Header("Offensives")]
        public HotkeyRow artilleryRow;
        public HotkeyRow railgunRow, rocketLauncherRow, MLRSRow, gatlingMortarRow;

        [Header("Tacticals")]
        public HotkeyRow shieldRow;
        public HotkeyRow boosterRow, stealthGeneratorRow, spySatelliteRow, controlTowerRow;

        [Header("Ultras")]
        public HotkeyRow deathstarRow;
        public HotkeyRow nukeLauncherRow, ultraliskRow, kamikazeSignalRow, broadsidesRow;

        [Header("Aircraft")]
        public HotkeyRow bomberRow;
        public HotkeyRow gunshipRow, fighterRow, steamCopterRow, broadswordRow;

        [Header("Ships")]
        public HotkeyRow attackBoatRow;
        public HotkeyRow frigateRow, destroyerRow, archonRow, attackRIBRow;*/

        private ISettableBroadcastingProperty<bool> _isDirty;
        public IBroadcastingProperty<bool> IsDirty { get; private set; }

        public event EventHandler<HotkeyRowEnabledEventArgs> RowEnabled;

        public void Initialise(HotkeysModel hotkeysModel)
        {
            /*Helper.AssertIsNotNull(playerCruiserRow, overviewRow, enemyCruiserRow);
            Helper.AssertIsNotNull(slowMotionRow, normalSpeedRow, fastForwardRow);
            Helper.AssertIsNotNull(droneStationRow, airFactoryRow, navalFactoryRow);
            Helper.AssertIsNotNull(shipTurretRow, airTurretRow, mortarRow, samSiteRow, teslaCoilRow);
            Helper.AssertIsNotNull(artilleryRow, broadsidesRow, rocketLauncherRow);
            Helper.AssertIsNotNull(shieldRow, boosterRow, stealthGeneratorRow, spySatelliteRow, controlTowerRow);
            Helper.AssertIsNotNull(deathstarRow, nukeLauncherRow, ultraliskRow, kamikazeSignalRow, broadsidesRow);
            Helper.AssertIsNotNull(bomberRow, gunshipRow, fighterRow);
            Helper.AssertIsNotNull(attackBoatRow, frigateRow, destroyerRow, archonRow);*/
            Assert.IsNotNull(hotkeysModel);

            _hotkeysModel = hotkeysModel;

            _isDirty = new SettableBroadcastingProperty<bool>(initialValue: false);
            IsDirty = new BroadcastingProperty<bool>(_isDirty);

            IList<HotkeyRow> rows = new List<HotkeyRow>();

            SetupNavigationRows(rows);
            SetupGameSpeedRows(rows);
            SetupBulidingCategoryRows(rows);
            SetupBuildableSlotRows(rows);
            /*SetupFactoryRows(rows);
            SetupDefensiveRows(rows);
            SetupOffensivesRows(rows);
            SetupTacticalsRows(rows);
            SetupUltrasRows(rows);
            SetupAircraftRows(rows);
            SetupShipRows(rows);*/

            foreach (HotkeyRow row in rows)
            {
                row.Enabled += Row_Enabled;
                row.Value.Key.ValueChanged += Key_ValueChanged;
            }
        }

        private void SetupNavigationRows(IList<HotkeyRow> rows)
        {
            rows.Add(playerCruiserRow);
            playerCruiserRow.Initialise(InputBC.Instance, _hotkeysModel.PlayerCruiser, this);

            rows.Add(overviewRow);
            overviewRow.Initialise(InputBC.Instance, _hotkeysModel.Overview, this);

            rows.Add(enemyCruiserRow);
            enemyCruiserRow.Initialise(InputBC.Instance, _hotkeysModel.EnemyCruiser, this);
        }

        private void SetupGameSpeedRows(IList<HotkeyRow> rows)
        {
            //rows.Add(pauseSpeedRow);
            // pauseSpeedRow.Initialise(InputBC.Instance, _hotkeysModel.PauseSpeed, this);

            rows.Add(slowMotionRow);
            slowMotionRow.Initialise(InputBC.Instance, _hotkeysModel.SlowMotion, this);

            rows.Add(normalSpeedRow);
            normalSpeedRow.Initialise(InputBC.Instance, _hotkeysModel.NormalSpeed, this);

            rows.Add(fastForwardRow);
            fastForwardRow.Initialise(InputBC.Instance, _hotkeysModel.FastForward, this);

            rows.Add(toggleSpeedRow);
            toggleSpeedRow.Initialise(InputBC.Instance, _hotkeysModel.ToggleSpeed, this);
        }

        private void SetupBulidingCategoryRows(IList<HotkeyRow> rows)
        {
            rows.Add(factoriesRow);
            factoriesRow.Initialise(InputBC.Instance, _hotkeysModel.Factories, this);

            rows.Add(defensivesRow);
            defensivesRow.Initialise(InputBC.Instance, _hotkeysModel.Defensives, this);

            rows.Add(offensivesRow);
            offensivesRow.Initialise(InputBC.Instance, _hotkeysModel.Offensives, this);

            rows.Add(tacticalsRow);
            tacticalsRow.Initialise(InputBC.Instance, _hotkeysModel.Tacticals, this);

            rows.Add(ultrasRow);
            ultrasRow.Initialise(InputBC.Instance, _hotkeysModel.Ultras, this);
        }

        private void SetupBuildableSlotRows(IList<HotkeyRow> rows)
        {
            rows.Add(Slot1);
            Slot1.Initialise(InputBC.Instance, _hotkeysModel.DroneStation, this);

            rows.Add(Slot2);
            Slot2.Initialise(InputBC.Instance, _hotkeysModel.AirFactory, this);

            rows.Add(Slot3);
            Slot3.Initialise(InputBC.Instance, _hotkeysModel.NavalFactory, this);

            rows.Add(Slot4);
            Slot4.Initialise(InputBC.Instance, _hotkeysModel.DroneStation4, this);

            rows.Add(Slot5);
            Slot5.Initialise(InputBC.Instance, _hotkeysModel.DroneStation8, this);
        }

        /*private void SetupFactoryRows(IList<HotkeyRow> rows)
        {
            rows.Add(droneStationRow);
            droneStationRow.Initialise(InputBC.Instance, _hotkeysModel.DroneStation, this);

            rows.Add(airFactoryRow);
            airFactoryRow.Initialise(InputBC.Instance, _hotkeysModel.AirFactory, this);

            rows.Add(navalFactoryRow);
            navalFactoryRow.Initialise(InputBC.Instance, _hotkeysModel.NavalFactory, this);

            rows.Add(droneStation4Row);
            droneStation4Row.Initialise(InputBC.Instance, _hotkeysModel.DroneStation4, this);

            rows.Add(droneStation8Row);
            droneStation8Row.Initialise(InputBC.Instance, _hotkeysModel.DroneStation8, this);
        }

        private void SetupDefensiveRows(IList<HotkeyRow> rows)
        {
            rows.Add(shipTurretRow);
            shipTurretRow.Initialise(InputBC.Instance, _hotkeysModel.ShipTurret, this);

            rows.Add(airTurretRow);
            airTurretRow.Initialise(InputBC.Instance, _hotkeysModel.AirTurret, this);

            rows.Add(mortarRow);
            mortarRow.Initialise(InputBC.Instance, _hotkeysModel.Mortar, this);

            rows.Add(samSiteRow);
            samSiteRow.Initialise(InputBC.Instance, _hotkeysModel.SamSite, this);

            rows.Add(teslaCoilRow);
            teslaCoilRow.Initialise(InputBC.Instance, _hotkeysModel.TeslaCoil, this);
        }

        private void SetupOffensivesRows(IList<HotkeyRow> rows)
        {
            rows.Add(artilleryRow);
            artilleryRow.Initialise(InputBC.Instance, _hotkeysModel.Artillery, this);

            rows.Add(railgunRow);
            railgunRow.Initialise(InputBC.Instance, _hotkeysModel.Railgun, this);

            rows.Add(rocketLauncherRow);
            rocketLauncherRow.Initialise(InputBC.Instance, _hotkeysModel.RocketLauncher, this);

            rows.Add(MLRSRow);
            MLRSRow.Initialise(InputBC.Instance, _hotkeysModel.MLRS, this);

            rows.Add(gatlingMortarRow);
            gatlingMortarRow.Initialise(InputBC.Instance, _hotkeysModel.GatlingMortar, this);
        }

        private void SetupTacticalsRows(IList<HotkeyRow> rows)
        {
            rows.Add(shieldRow);
            shieldRow.Initialise(InputBC.Instance, _hotkeysModel.Shield, this);

            rows.Add(boosterRow);
            boosterRow.Initialise(InputBC.Instance, _hotkeysModel.Booster, this);

            rows.Add(stealthGeneratorRow);
            stealthGeneratorRow.Initialise(InputBC.Instance, _hotkeysModel.StealthGenerator, this);

            rows.Add(spySatelliteRow);
            spySatelliteRow.Initialise(InputBC.Instance, _hotkeysModel.SpySatellite, this);

            rows.Add(controlTowerRow);
            controlTowerRow.Initialise(InputBC.Instance, _hotkeysModel.ControlTower, this);
        }

        private void SetupUltrasRows(IList<HotkeyRow> rows)
        {
            rows.Add(deathstarRow);
            deathstarRow.Initialise(InputBC.Instance, _hotkeysModel.Deathstar, this);

            rows.Add(nukeLauncherRow);
            nukeLauncherRow.Initialise(InputBC.Instance, _hotkeysModel.NukeLauncher, this);

            rows.Add(ultraliskRow);
            ultraliskRow.Initialise(InputBC.Instance, _hotkeysModel.Ultralisk, this);

            rows.Add(kamikazeSignalRow);
            kamikazeSignalRow.Initialise(InputBC.Instance, _hotkeysModel.KamikazeSignal, this);

            rows.Add(broadsidesRow);
            broadsidesRow.Initialise(InputBC.Instance, _hotkeysModel.Broadsides, this);
        }

        private void SetupAircraftRows(IList<HotkeyRow> rows)
        {
            rows.Add(bomberRow);
            bomberRow.Initialise(InputBC.Instance, _hotkeysModel.Bomber, this);

            rows.Add(gunshipRow);
            gunshipRow.Initialise(InputBC.Instance, _hotkeysModel.Gunship, this);

            rows.Add(fighterRow);
            fighterRow.Initialise(InputBC.Instance, _hotkeysModel.Fighter, this);

            rows.Add(steamCopterRow);
            steamCopterRow.Initialise(InputBC.Instance, _hotkeysModel.SteamCopter, this);

            rows.Add(broadswordRow);
            broadswordRow.Initialise(InputBC.Instance, _hotkeysModel.Broadsword, this);
        }

        private void SetupShipRows(IList<HotkeyRow> rows)
        {
            rows.Add(attackBoatRow);
            attackBoatRow.Initialise(InputBC.Instance, _hotkeysModel.AttackBoat, this);

            rows.Add(frigateRow);
            frigateRow.Initialise(InputBC.Instance, _hotkeysModel.Frigate, this);

            rows.Add(destroyerRow);
            destroyerRow.Initialise(InputBC.Instance, _hotkeysModel.Destroyer, this);

            rows.Add(archonRow);
            archonRow.Initialise(InputBC.Instance, _hotkeysModel.Archon, this);

            rows.Add(attackRIBRow);
            attackRIBRow.Initialise(InputBC.Instance, _hotkeysModel.AttackRIB, this);
        }*/

        private void Row_Enabled(object sender, EventArgs e)
        {
            HotkeyRow row = sender.Parse<HotkeyRow>();
            RowEnabled?.Invoke(this, new HotkeyRowEnabledEventArgs(row));
        }

        private void Key_ValueChanged(object sender, EventArgs e)
        {
            _isDirty.Value = FindIsDirty();
        }

        private bool FindIsDirty()
        {
            bool isDirty =
                // Navigation
                playerCruiserRow.Value.Key.Value != _hotkeysModel.PlayerCruiser
                || overviewRow.Value.Key.Value != _hotkeysModel.Overview
                || enemyCruiserRow.Value.Key.Value != _hotkeysModel.EnemyCruiser
                // Game speed
                //|| pauseSpeedRow.Value.Key.Value != _hotkeysModel.PauseSpeed
                || slowMotionRow.Value.Key.Value != _hotkeysModel.SlowMotion
                || normalSpeedRow.Value.Key.Value != _hotkeysModel.NormalSpeed
                || fastForwardRow.Value.Key.Value != _hotkeysModel.FastForward
                || toggleSpeedRow.Value.Key.Value != _hotkeysModel.ToggleSpeed
                // Building categories
                || factoriesRow.Value.Key.Value != _hotkeysModel.Factories
                || defensivesRow.Value.Key.Value != _hotkeysModel.Defensives
                || offensivesRow.Value.Key.Value != _hotkeysModel.Offensives
                || tacticalsRow.Value.Key.Value != _hotkeysModel.Tacticals
                || ultrasRow.Value.Key.Value != _hotkeysModel.Ultras;

            // Buildable slot hotkeys are intended to represent the 5 visible "build buttons" (Q/W/E/R/T)
            // regardless of which category is currently active in the HUD.
            // Therefore, the settings screen exposes ONLY Slot1-Slot5, and we keep all per-buildable
            // hotkeys in sync with these five keys.
            isDirty = isDirty
                // Slot1
                || Slot1.Value.Key.Value != _hotkeysModel.DroneStation
                || Slot1.Value.Key.Value != _hotkeysModel.ShipTurret
                || Slot1.Value.Key.Value != _hotkeysModel.Artillery
                || Slot1.Value.Key.Value != _hotkeysModel.Shield
                || Slot1.Value.Key.Value != _hotkeysModel.Deathstar
                || Slot1.Value.Key.Value != _hotkeysModel.Bomber
                || Slot1.Value.Key.Value != _hotkeysModel.AttackBoat
                // Slot2
                || Slot2.Value.Key.Value != _hotkeysModel.AirFactory
                || Slot2.Value.Key.Value != _hotkeysModel.AirTurret
                || Slot2.Value.Key.Value != _hotkeysModel.Railgun
                || Slot2.Value.Key.Value != _hotkeysModel.Booster
                || Slot2.Value.Key.Value != _hotkeysModel.NukeLauncher
                || Slot2.Value.Key.Value != _hotkeysModel.Gunship
                || Slot2.Value.Key.Value != _hotkeysModel.Frigate
                // Slot3
                || Slot3.Value.Key.Value != _hotkeysModel.NavalFactory
                || Slot3.Value.Key.Value != _hotkeysModel.Mortar
                || Slot3.Value.Key.Value != _hotkeysModel.RocketLauncher
                || Slot3.Value.Key.Value != _hotkeysModel.StealthGenerator
                || Slot3.Value.Key.Value != _hotkeysModel.Ultralisk
                || Slot3.Value.Key.Value != _hotkeysModel.Fighter
                || Slot3.Value.Key.Value != _hotkeysModel.Destroyer
                // Slot4
                || Slot4.Value.Key.Value != _hotkeysModel.DroneStation4
                || Slot4.Value.Key.Value != _hotkeysModel.SamSite
                || Slot4.Value.Key.Value != _hotkeysModel.MLRS
                || Slot4.Value.Key.Value != _hotkeysModel.SpySatellite
                || Slot4.Value.Key.Value != _hotkeysModel.KamikazeSignal
                || Slot4.Value.Key.Value != _hotkeysModel.SteamCopter
                || Slot4.Value.Key.Value != _hotkeysModel.Archon
                // Slot5
                || Slot5.Value.Key.Value != _hotkeysModel.DroneStation8
                || Slot5.Value.Key.Value != _hotkeysModel.TeslaCoil
                || Slot5.Value.Key.Value != _hotkeysModel.GatlingMortar
                || Slot5.Value.Key.Value != _hotkeysModel.ControlTower
                || Slot5.Value.Key.Value != _hotkeysModel.Broadsides
                || Slot5.Value.Key.Value != _hotkeysModel.Broadsword
                || Slot5.Value.Key.Value != _hotkeysModel.AttackRIB;

            return isDirty;
        }

        public void UpdateHokeysModel()
        {
            // Navigation
            _hotkeysModel.PlayerCruiser = playerCruiserRow.Value.Key.Value;
            _hotkeysModel.Overview = overviewRow.Value.Key.Value;
            _hotkeysModel.EnemyCruiser = enemyCruiserRow.Value.Key.Value;

            // Game speed
            //_hotkeysModel.PauseSpeed = pauseSpeedRow.Value.Key.Value;
            _hotkeysModel.SlowMotion = slowMotionRow.Value.Key.Value;
            _hotkeysModel.NormalSpeed = normalSpeedRow.Value.Key.Value;
            _hotkeysModel.FastForward = fastForwardRow.Value.Key.Value;
            _hotkeysModel.ToggleSpeed = toggleSpeedRow.Value.Key.Value;

            // Building categories
            _hotkeysModel.Factories = factoriesRow.Value.Key.Value;
            _hotkeysModel.Defensives = defensivesRow.Value.Key.Value;
            _hotkeysModel.Offensives = offensivesRow.Value.Key.Value;
            _hotkeysModel.Tacticals = tacticalsRow.Value.Key.Value;
            _hotkeysModel.Ultras = ultrasRow.Value.Key.Value;

            // Slot1-Slot5 represent the 5 visible build buttons (Q/W/E/R/T) irrespective of category.
            // Keep all per-buildable hotkeys in sync with these five slot hotkeys.
            ApplySlotHotkeysToAllBuildables(
                Slot1.Value.Key.Value,
                Slot2.Value.Key.Value,
                Slot3.Value.Key.Value,
                Slot4.Value.Key.Value,
                Slot5.Value.Key.Value);

            _isDirty.Value = FindIsDirty();
        }

        private void ApplySlotHotkeysToAllBuildables(KeyCode slot1, KeyCode slot2, KeyCode slot3, KeyCode slot4, KeyCode slot5)
        {
            // Factories
            _hotkeysModel.DroneStation = slot1;
            _hotkeysModel.AirFactory = slot2;
            _hotkeysModel.NavalFactory = slot3;
            _hotkeysModel.DroneStation4 = slot4;
            _hotkeysModel.DroneStation8 = slot5;

            // Defensives
            _hotkeysModel.ShipTurret = slot1;
            _hotkeysModel.AirTurret = slot2;
            _hotkeysModel.Mortar = slot3;
            _hotkeysModel.SamSite = slot4;
            _hotkeysModel.TeslaCoil = slot5;

            // Offensives
            _hotkeysModel.Artillery = slot1;
            _hotkeysModel.Railgun = slot2;
            _hotkeysModel.RocketLauncher = slot3;
            _hotkeysModel.MLRS = slot4;
            _hotkeysModel.GatlingMortar = slot5;

            // Tacticals
            _hotkeysModel.Shield = slot1;
            _hotkeysModel.Booster = slot2;
            _hotkeysModel.StealthGenerator = slot3;
            _hotkeysModel.SpySatellite = slot4;
            _hotkeysModel.ControlTower = slot5;

            // Ultras
            _hotkeysModel.Deathstar = slot1;
            _hotkeysModel.NukeLauncher = slot2;
            _hotkeysModel.Ultralisk = slot3;
            _hotkeysModel.KamikazeSignal = slot4;
            _hotkeysModel.Broadsides = slot5;

            // Aircraft
            _hotkeysModel.Bomber = slot1;
            _hotkeysModel.Gunship = slot2;
            _hotkeysModel.Fighter = slot3;
            _hotkeysModel.SteamCopter = slot4;
            _hotkeysModel.Broadsword = slot5;

            // Ships
            _hotkeysModel.AttackBoat = slot1;
            _hotkeysModel.Frigate = slot2;
            _hotkeysModel.Destroyer = slot3;
            _hotkeysModel.Archon = slot4;
            _hotkeysModel.AttackRIB = slot5;
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void ResetToSavedState()
        {
            ResetToModel(_hotkeysModel);
        }

        public void ResetToDefaults()
        {
            ResetToModel(HotkeysModel.CreateDefault());
        }

        private void ResetToModel(HotkeysModel hotkeysModel)
        {
            // Navigation
            playerCruiserRow.ResetToDefaults(hotkeysModel.PlayerCruiser);
            overviewRow.ResetToDefaults(hotkeysModel.Overview);
            enemyCruiserRow.ResetToDefaults(hotkeysModel.EnemyCruiser);

            // Game speed
            //pauseSpeedRow.ResetToDefaults(hotkeysModel.PauseSpeed);
            slowMotionRow.ResetToDefaults(hotkeysModel.SlowMotion);
            normalSpeedRow.ResetToDefaults(hotkeysModel.NormalSpeed);
            fastForwardRow.ResetToDefaults(hotkeysModel.FastForward);
            toggleSpeedRow.ResetToDefaults(hotkeysModel.ToggleSpeed);

            // Building categories
            factoriesRow.ResetToDefaults(hotkeysModel.Factories);
            defensivesRow.ResetToDefaults(hotkeysModel.Defensives);
            offensivesRow.ResetToDefaults(hotkeysModel.Offensives);
            tacticalsRow.ResetToDefaults(hotkeysModel.Tacticals);
            ultrasRow.ResetToDefaults(hotkeysModel.Ultras);

            // Slots represent the 5 visible build buttons, so initialise them from the canonical factory mapping.
            Slot1.ResetToDefaults(hotkeysModel.DroneStation);
            Slot2.ResetToDefaults(hotkeysModel.AirFactory);
            Slot3.ResetToDefaults(hotkeysModel.NavalFactory);
            Slot4.ResetToDefaults(hotkeysModel.DroneStation4);
            Slot5.ResetToDefaults(hotkeysModel.DroneStation8);
        }
    }
}