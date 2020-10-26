using BattleCruisers.UI.Panels;
using System;
using UnityCommon.Properties;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public interface IHotkeysPanel : IPanel
    {
        IBroadcastingProperty<bool> IsDirty { get; }

        event EventHandler<HotkeyRowEnabledEventArgs> RowEnabled;

        void UpdateHokeysModel();
    }
}