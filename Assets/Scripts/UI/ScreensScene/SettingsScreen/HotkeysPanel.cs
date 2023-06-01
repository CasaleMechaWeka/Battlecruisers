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

    public class HotkeysPanel : Panel, IHotkeysPanel
    {
        private IHotkeysModel _hotkeysModel;

        [Header("Navigation")]
        public HotkeyRow playerCruiserRow;
        public HotkeyRow overviewRow, enemyCruiserRow;

        [Header("Game speed")]
        public HotkeyRow pauseSpeedRow, slowMotionRow;
        public HotkeyRow normalSpeedRow, fastForwardRow, toggleSpeedRow;

        [Header("Building categories")]
        public HotkeyRow factoriesRow;
        public HotkeyRow defensivesRow, offensivesRow, tacticalsRow, ultrasRow;
        
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
        public HotkeyRow frigateRow, destroyerRow, archonRow, attackRIBRow;

        private ISettableBroadcastingProperty<bool> _isDirty;
        public IBroadcastingProperty<bool> IsDirty { get; private set; }

        public event EventHandler<HotkeyRowEnabledEventArgs> RowEnabled;

        public void Initialise(IHotkeysModel hotkeysModel)
        {
            Helper.AssertIsNotNull(playerCruiserRow, overviewRow, enemyCruiserRow);
            Helper.AssertIsNotNull(slowMotionRow, normalSpeedRow, fastForwardRow);
            Helper.AssertIsNotNull(droneStationRow, airFactoryRow, navalFactoryRow);
            Helper.AssertIsNotNull(shipTurretRow, airTurretRow, mortarRow, samSiteRow, teslaCoilRow);
            Helper.AssertIsNotNull(artilleryRow, broadsidesRow, rocketLauncherRow);
            Helper.AssertIsNotNull(shieldRow, boosterRow, stealthGeneratorRow, spySatelliteRow, controlTowerRow);
            Helper.AssertIsNotNull(deathstarRow, nukeLauncherRow, ultraliskRow, kamikazeSignalRow, broadsidesRow);
            Helper.AssertIsNotNull(bomberRow, gunshipRow, fighterRow);
            Helper.AssertIsNotNull(attackBoatRow, frigateRow, destroyerRow, archonRow);
            Assert.IsNotNull(hotkeysModel);

            _hotkeysModel = hotkeysModel;

            _isDirty = new SettableBroadcastingProperty<bool>(initialValue: false);
            IsDirty = new BroadcastingProperty<bool>(_isDirty);

            IList<HotkeyRow> rows = new List<HotkeyRow>();

            SetupNavigationRows(rows);
            SetupGameSpeedRows(rows);
            SetupBulidingCategoryRows(rows);
            SetupFactoryRows(rows);
            SetupDefensiveRows(rows);
            SetupOffensivesRows(rows);
            SetupTacticalsRows(rows);
            SetupUltrasRows(rows);
            SetupAircraftRows(rows);
            SetupShipRows(rows);

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
            rows.Add(pauseSpeedRow);
            pauseSpeedRow.Initialise(InputBC.Instance, _hotkeysModel.PauseSpeed, this);

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

        private void SetupFactoryRows(IList<HotkeyRow> rows)
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
        }

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
            return
                // Navigation
                playerCruiserRow.Value.Key.Value != _hotkeysModel.PlayerCruiser
                || overviewRow.Value.Key.Value != _hotkeysModel.Overview
                || enemyCruiserRow.Value.Key.Value != _hotkeysModel.EnemyCruiser
                // Game speed
                || pauseSpeedRow.Value.Key.Value != _hotkeysModel.PauseSpeed
                || slowMotionRow.Value.Key.Value != _hotkeysModel.SlowMotion
                || normalSpeedRow.Value.Key.Value != _hotkeysModel.NormalSpeed
                || fastForwardRow.Value.Key.Value != _hotkeysModel.FastForward
                || toggleSpeedRow.Value.Key.Value != _hotkeysModel.ToggleSpeed
                // Building categories
                || factoriesRow.Value.Key.Value != _hotkeysModel.Factories
                || defensivesRow.Value.Key.Value != _hotkeysModel.Defensives
                || offensivesRow.Value.Key.Value != _hotkeysModel.Offensives
                || tacticalsRow.Value.Key.Value != _hotkeysModel.Tacticals
                || ultrasRow.Value.Key.Value != _hotkeysModel.Ultras
                // Factories
                || droneStationRow.Value.Key.Value != _hotkeysModel.DroneStation
                || airFactoryRow.Value.Key.Value != _hotkeysModel.AirFactory
                || navalFactoryRow.Value.Key.Value != _hotkeysModel.NavalFactory
                || droneStation4Row.Value.Key.Value != _hotkeysModel.DroneStation4
                || droneStation8Row.Value.Key.Value != _hotkeysModel.DroneStation8
                // Defensives
                || shipTurretRow.Value.Key.Value != _hotkeysModel.ShipTurret
                || airTurretRow.Value.Key.Value != _hotkeysModel.AirTurret
                || mortarRow.Value.Key.Value != _hotkeysModel.Mortar
                || samSiteRow.Value.Key.Value != _hotkeysModel.SamSite
                || teslaCoilRow.Value.Key.Value != _hotkeysModel.TeslaCoil
                // Offensives
                || artilleryRow.Value.Key.Value != _hotkeysModel.Artillery
                || railgunRow.Value.Key.Value != _hotkeysModel.Railgun
                || rocketLauncherRow.Value.Key.Value != _hotkeysModel.RocketLauncher
                || MLRSRow.Value.Key.Value != _hotkeysModel.MLRS
                || gatlingMortarRow.Value.Key.Value != _hotkeysModel.GatlingMortar
                // Tacticals
                || shieldRow.Value.Key.Value != _hotkeysModel.Shield
                || boosterRow.Value.Key.Value != _hotkeysModel.Booster
                || stealthGeneratorRow.Value.Key.Value != _hotkeysModel.StealthGenerator
                || spySatelliteRow.Value.Key.Value != _hotkeysModel.SpySatellite
                || controlTowerRow.Value.Key.Value != _hotkeysModel.ControlTower
                // Ultras
                || deathstarRow.Value.Key.Value != _hotkeysModel.Deathstar
                || nukeLauncherRow.Value.Key.Value != _hotkeysModel.NukeLauncher
                || ultraliskRow.Value.Key.Value != _hotkeysModel.Ultralisk
                || kamikazeSignalRow.Value.Key.Value != _hotkeysModel.KamikazeSignal
                || broadsidesRow.Value.Key.Value != _hotkeysModel.Broadsides
                // Aircraft
                || bomberRow.Value.Key.Value != _hotkeysModel.Bomber
                || gunshipRow.Value.Key.Value != _hotkeysModel.Gunship
                || fighterRow.Value.Key.Value != _hotkeysModel.Fighter
                || steamCopterRow.Value.Key.Value != _hotkeysModel.SteamCopter
                || broadswordRow.Value.Key.Value != _hotkeysModel.Broadsword
                // Ships
                || attackBoatRow.Value.Key.Value != _hotkeysModel.AttackBoat
                || frigateRow.Value.Key.Value != _hotkeysModel.Frigate
                || destroyerRow.Value.Key.Value != _hotkeysModel.Destroyer
                || archonRow.Value.Key.Value != _hotkeysModel.Archon
                || attackRIBRow.Value.Key.Value != _hotkeysModel.AttackRIB;
        }

        public void UpdateHokeysModel()
        {
            // Navigation
            _hotkeysModel.PlayerCruiser = playerCruiserRow.Value.Key.Value;
            _hotkeysModel.Overview = overviewRow.Value.Key.Value;
            _hotkeysModel.EnemyCruiser = enemyCruiserRow.Value.Key.Value;

            // Game speed
            _hotkeysModel.PauseSpeed = pauseSpeedRow.Value.Key.Value;
            _hotkeysModel.SlowMotion = slowMotionRow.Value.Key.Value;
            _hotkeysModel.NormalSpeed = normalSpeedRow.Value.Key.Value;
            _hotkeysModel.FastForward = fastForwardRow.Value.Key.Value;
            _hotkeysModel.ToggleSpeed = toggleSpeedRow.Value.Key.Value;


            // Building categories
            _hotkeysModel.Factories = factoriesRow.Value.Key.Value;
            _hotkeysModel.Defensives= defensivesRow.Value.Key.Value;
            _hotkeysModel.Offensives = offensivesRow.Value.Key.Value;
            _hotkeysModel.Tacticals = tacticalsRow.Value.Key.Value;
            _hotkeysModel.Ultras = ultrasRow.Value.Key.Value;

            // Factories
            _hotkeysModel.DroneStation = droneStationRow.Value.Key.Value;
            _hotkeysModel.AirFactory = airFactoryRow.Value.Key.Value;
            _hotkeysModel.NavalFactory = navalFactoryRow.Value.Key.Value;
            _hotkeysModel.DroneStation4 = droneStation4Row.Value.Key.Value;
            _hotkeysModel.DroneStation8 = droneStation8Row.Value.Key.Value;

            // Defensives
            _hotkeysModel.ShipTurret = shipTurretRow.Value.Key.Value;
            _hotkeysModel.AirTurret= airTurretRow.Value.Key.Value;
            _hotkeysModel.Mortar = mortarRow.Value.Key.Value;
            _hotkeysModel.SamSite = samSiteRow.Value.Key.Value;
            _hotkeysModel.TeslaCoil = teslaCoilRow.Value.Key.Value;

            // Offensives
            _hotkeysModel.Artillery = artilleryRow.Value.Key.Value;
            _hotkeysModel.Railgun = railgunRow.Value.Key.Value;
            _hotkeysModel.RocketLauncher = rocketLauncherRow.Value.Key.Value;
            _hotkeysModel.MLRS = MLRSRow.Value.Key.Value;
            _hotkeysModel.GatlingMortar = gatlingMortarRow.Value.Key.Value;

            // Tacticals
            _hotkeysModel.Shield = shieldRow.Value.Key.Value;
            _hotkeysModel.Booster= boosterRow.Value.Key.Value;
            _hotkeysModel.StealthGenerator = stealthGeneratorRow.Value.Key.Value;
            _hotkeysModel.SpySatellite = spySatelliteRow.Value.Key.Value;
            _hotkeysModel.ControlTower = controlTowerRow.Value.Key.Value;

            // Ultras
            _hotkeysModel.Deathstar = deathstarRow.Value.Key.Value;
            _hotkeysModel.NukeLauncher = nukeLauncherRow.Value.Key.Value;
            _hotkeysModel.Ultralisk = ultraliskRow.Value.Key.Value;
            _hotkeysModel.KamikazeSignal = kamikazeSignalRow.Value.Key.Value;
            _hotkeysModel.Broadsides = broadsidesRow.Value.Key.Value;

            // Aircraft
            _hotkeysModel.Bomber = bomberRow.Value.Key.Value;
            _hotkeysModel.Gunship = gunshipRow.Value.Key.Value;
            _hotkeysModel.Fighter = fighterRow.Value.Key.Value;
            _hotkeysModel.SteamCopter = steamCopterRow.Value.Key.Value;
            _hotkeysModel.Broadsword = broadswordRow.Value.Key.Value;

            // Ships
            _hotkeysModel.AttackBoat = attackBoatRow.Value.Key.Value;
            _hotkeysModel.Frigate = frigateRow.Value.Key.Value;
            _hotkeysModel.Destroyer= destroyerRow.Value.Key.Value;
            _hotkeysModel.Archon = archonRow.Value.Key.Value;
            _hotkeysModel.AttackRIB = attackRIBRow.Value.Key.Value;

            _isDirty.Value = FindIsDirty();
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

        private void ResetToModel(IHotkeysModel hotkeysModel)
        {
            // Navigation
            playerCruiserRow.ResetToDefaults(hotkeysModel.PlayerCruiser);
            overviewRow.ResetToDefaults(hotkeysModel.Overview);
            enemyCruiserRow.ResetToDefaults(hotkeysModel.EnemyCruiser);
            
            // Game speed
            pauseSpeedRow.ResetToDefaults(hotkeysModel.PauseSpeed);
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

            // Factories
            droneStationRow.ResetToDefaults(hotkeysModel.DroneStation);
            airFactoryRow.ResetToDefaults(hotkeysModel.AirFactory);
            navalFactoryRow.ResetToDefaults(hotkeysModel.NavalFactory);
            droneStation4Row.ResetToDefaults(hotkeysModel.DroneStation4);
            droneStation8Row.ResetToDefaults(hotkeysModel.DroneStation8);

            // Defensives
            shipTurretRow.ResetToDefaults(hotkeysModel.ShipTurret);
            airTurretRow.ResetToDefaults(hotkeysModel.AirFactory);
            mortarRow.ResetToDefaults(hotkeysModel.Mortar);
            samSiteRow.ResetToDefaults(hotkeysModel.SamSite);
            teslaCoilRow.ResetToDefaults(hotkeysModel.TeslaCoil);

            // Offensives
            artilleryRow.ResetToDefaults(hotkeysModel.Artillery);
            railgunRow.ResetToDefaults(hotkeysModel.Railgun);
            rocketLauncherRow.ResetToDefaults(hotkeysModel.RocketLauncher);
            MLRSRow.ResetToDefaults(hotkeysModel.MLRS);
            gatlingMortarRow.ResetToDefaults(hotkeysModel.GatlingMortar);

            // Tacticals
            shieldRow.ResetToDefaults(hotkeysModel.Shield);
            boosterRow.ResetToDefaults(hotkeysModel.Booster);
            stealthGeneratorRow.ResetToDefaults(hotkeysModel.StealthGenerator);
            spySatelliteRow.ResetToDefaults(hotkeysModel.SpySatellite);
            controlTowerRow.ResetToDefaults(hotkeysModel.ControlTower);

            // Ultras
            deathstarRow.ResetToDefaults(hotkeysModel.Deathstar);
            nukeLauncherRow.ResetToDefaults(hotkeysModel.NukeLauncher);
            ultraliskRow.ResetToDefaults(hotkeysModel.Ultralisk);
            kamikazeSignalRow.ResetToDefaults(hotkeysModel.KamikazeSignal);
            broadsidesRow.ResetToDefaults(hotkeysModel.Broadsides);

            // Aircraft
            bomberRow.ResetToDefaults(hotkeysModel.Bomber);
            gunshipRow.ResetToDefaults(hotkeysModel.Gunship);
            fighterRow.ResetToDefaults(hotkeysModel.Fighter);
            steamCopterRow.ResetToDefaults(hotkeysModel.SteamCopter);
            broadswordRow.ResetToDefaults(hotkeysModel.Broadsword);

            // Ships
            attackBoatRow.ResetToDefaults(hotkeysModel.AttackBoat);
            frigateRow.ResetToDefaults(hotkeysModel.Frigate);
            destroyerRow.ResetToDefaults(hotkeysModel.Destroyer);
            archonRow.ResetToDefaults(hotkeysModel.Archon);
            attackRIBRow.ResetToDefaults(hotkeysModel.AttackRIB);
        }
    }
}