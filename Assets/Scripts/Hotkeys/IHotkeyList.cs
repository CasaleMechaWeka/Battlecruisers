using UnityEngine;

namespace BattleCruisers.Hotkeys
{
    public interface IHotkeyList
    {
        // Navigation
        KeyCode PlayerCruiser { get; }
        KeyCode Overview { get; }
        KeyCode EnemyCruiser { get; }

        // Boats
        KeyCode AttackBoat { get; }
        KeyCode Frigate { get; }
        KeyCode Destroyer { get; }
        KeyCode Archon { get; }
    }
}