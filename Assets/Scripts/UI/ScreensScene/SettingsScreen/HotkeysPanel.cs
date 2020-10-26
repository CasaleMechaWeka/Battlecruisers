using BattleCruisers.UI.Panels;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public HotkeyRow playerCruiserRow;

        public event EventHandler<HotkeyRowEnabledEventArgs> RowEnabled;

        public void Initialise()
        {
            Assert.IsNotNull(playerCruiserRow);

            IList<HotkeyRow> rows = new List<HotkeyRow>();

            rows.Add(playerCruiserRow);
            playerCruiserRow.Initialise(InputBC.Instance, KeyCode.LeftArrow, this);

            foreach (HotkeyRow row in rows)
            {
                row.Enabled += Row_Enabled;
            }
        }

        private void Row_Enabled(object sender, EventArgs e)
        {
            HotkeyRow row = sender.Parse<HotkeyRow>();
            RowEnabled?.Invoke(this, new HotkeyRowEnabledEventArgs(row));
        }
    }
}