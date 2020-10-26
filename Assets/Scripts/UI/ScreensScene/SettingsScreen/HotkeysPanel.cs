using BattleCruisers.Data.Models;
using BattleCruisers.Hotkeys;
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
        public HotkeyRow playerCruiserRow, overviewRow;

        public event EventHandler<HotkeyRowEnabledEventArgs> RowEnabled;

        public void Initialise(IHotkeyList hotkeyList)
        {
            Helper.AssertIsNotNull(playerCruiserRow, overviewRow);
            Assert.IsNotNull(hotkeyList);

            IList<HotkeyRow> rows = new List<HotkeyRow>();

            rows.Add(playerCruiserRow);
            playerCruiserRow.Initialise(InputBC.Instance, hotkeyList.PlayerCruiser, this);

            rows.Add(overviewRow);
            overviewRow.Initialise(InputBC.Instance, hotkeyList.Overview, this);

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