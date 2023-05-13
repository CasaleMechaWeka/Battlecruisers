using BattleCruisers.Data.Models;
using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

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
        
        [FormerlySerializedAs("droneStationRow")] [Header("Buildables")] //TODO this needs to be synced with the unity editor
        public HotkeyRow BuildablesRow;
        public HotkeyRow Buildable1, Buildable2, Buildable3, Buildable4, Buildable5;

        private ISettableBroadcastingProperty<bool> _isDirty;
        public IBroadcastingProperty<bool> IsDirty { get; private set; }

        public event EventHandler<HotkeyRowEnabledEventArgs> RowEnabled;

        public void Initialise(IHotkeysModel hotkeysModel)
        {
            Helper.AssertIsNotNull(playerCruiserRow, overviewRow, enemyCruiserRow);
            Helper.AssertIsNotNull(slowMotionRow, normalSpeedRow, fastForwardRow);
            Helper.AssertIsNotNull(BuildablesRow, Buildable1, Buildable2, Buildable3, Buildable4, Buildable5);
            Assert.IsNotNull(hotkeysModel);

            _hotkeysModel = hotkeysModel;

            _isDirty = new SettableBroadcastingProperty<bool>(initialValue: false);
            IsDirty = new BroadcastingProperty<bool>(_isDirty);

            IList<HotkeyRow> rows = new List<HotkeyRow>();

            SetupNavigationRows(rows);
            SetupGameSpeedRows(rows);
            SetupBulidingCategoryRows(rows);
            SetupFactoryRows(rows);

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
            rows.Add(BuildablesRow);
            BuildablesRow.Initialise(InputBC.Instance, _hotkeysModel.DroneStation, this);
            
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
                || BuildablesRow.Value.Key.Value != _hotkeysModel.DroneStation;
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
            _hotkeysModel.DroneStation = BuildablesRow.Value.Key.Value;

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
            BuildablesRow.ResetToDefaults(hotkeysModel.DroneStation);
        }
    }
}