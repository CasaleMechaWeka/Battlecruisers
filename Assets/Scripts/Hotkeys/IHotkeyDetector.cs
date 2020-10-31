using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys
{
    public interface IHotkeyDetector : IManagedDisposable
    {
        // Navigation
        event EventHandler PlayerCruiser, Overview, EnemyCruiser;

        // Building categories
        event EventHandler Factories, Defensives, Offensives, Tacticals, Ultras;

        // Buildable buttons
        event EventHandler AttackBoat, Frigate, Destroyer, Archon;
    }
}