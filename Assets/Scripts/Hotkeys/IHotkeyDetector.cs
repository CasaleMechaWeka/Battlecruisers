using System;

namespace BattleCruisers.Hotkeys
{
    public interface IHotkeyDetector
    {
        // Navigation
        event EventHandler PlayerCruiser, Overview, EnemyCruiser;
    }
}