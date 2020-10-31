using BattleCruisers.Data.Models;
using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using UnityCommon.Properties;
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

        // FELIX  Split up class?
        public HotkeyRow playerCruiserRow, overviewRow, enemyCruiserRow;
        public HotkeyRow attackBoatRow, frigateRow, destroyerRow, archonRow;

        private ISettableBroadcastingProperty<bool> _isDirty;
        public IBroadcastingProperty<bool> IsDirty { get; private set; }

        public event EventHandler<HotkeyRowEnabledEventArgs> RowEnabled;

        public void Initialise(IHotkeysModel hotkeysModel)
        {
            Helper.AssertIsNotNull(playerCruiserRow, overviewRow, enemyCruiserRow);
            Helper.AssertIsNotNull(attackBoatRow, frigateRow, destroyerRow, archonRow);
            Assert.IsNotNull(hotkeysModel);

            _hotkeysModel = hotkeysModel;

            _isDirty = new SettableBroadcastingProperty<bool>(initialValue: false);
            IsDirty = new BroadcastingProperty<bool>(_isDirty);

            IList<HotkeyRow> rows = new List<HotkeyRow>();

            // Navigation
            rows.Add(playerCruiserRow);
            playerCruiserRow.Initialise(InputBC.Instance, _hotkeysModel.PlayerCruiser, this);

            rows.Add(overviewRow);
            overviewRow.Initialise(InputBC.Instance, _hotkeysModel.Overview, this);

            rows.Add(enemyCruiserRow);
            enemyCruiserRow.Initialise(InputBC.Instance, _hotkeysModel.EnemyCruiser, this);

            // Ships
            rows.Add(attackBoatRow);
            attackBoatRow.Initialise(InputBC.Instance, _hotkeysModel.AttackBoat, this);

            rows.Add(frigateRow);
            frigateRow.Initialise(InputBC.Instance, _hotkeysModel.Frigate, this);

            rows.Add(destroyerRow);
            destroyerRow.Initialise(InputBC.Instance, _hotkeysModel.Destroyer, this);

            rows.Add(archonRow);
            archonRow.Initialise(InputBC.Instance, _hotkeysModel.Archon, this);

            foreach (HotkeyRow row in rows)
            {
                row.Enabled += Row_Enabled;
                row.Value.Key.ValueChanged += Key_ValueChanged;
            }
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
                // Ships
                || attackBoatRow.Value.Key.Value != _hotkeysModel.AttackBoat
                || frigateRow.Value.Key.Value != _hotkeysModel.Frigate
                || destroyerRow.Value.Key.Value != _hotkeysModel.Destroyer
                || archonRow.Value.Key.Value != _hotkeysModel.Archon;
        }

        public void UpdateHokeysModel()
        {
            // Navigation
            _hotkeysModel.PlayerCruiser = playerCruiserRow.Value.Key.Value;
            _hotkeysModel.Overview = overviewRow.Value.Key.Value;
            _hotkeysModel.EnemyCruiser = enemyCruiserRow.Value.Key.Value;

            // Ships
            _hotkeysModel.AttackBoat = attackBoatRow.Value.Key.Value;
            _hotkeysModel.Frigate = frigateRow.Value.Key.Value;
            _hotkeysModel.Destroyer= destroyerRow.Value.Key.Value;
            _hotkeysModel.Archon = archonRow.Value.Key.Value;
        }

        public override void Hide()
        {
            base.Hide();
            Reset();
        }

        public void Reset()
        {
            // Navigation
            playerCruiserRow.Reset();
            overviewRow.Reset();
            enemyCruiserRow.Reset();

            // Ships
            attackBoatRow.Reset();
            frigateRow.Reset();
            destroyerRow.Reset();
            archonRow.Reset();
        }
    }
}