using BattleCruisers.UI.Panels;
using System;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public interface IHotkeysPanel : IPanel
    {
        IBroadcastingProperty<bool> IsDirty { get; }

        event EventHandler<HotkeyRowEnabledEventArgs> RowEnabled;

        void UpdateHokeysModel();
    }
}