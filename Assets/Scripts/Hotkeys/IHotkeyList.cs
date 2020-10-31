using UnityEngine;

namespace BattleCruisers.Hotkeys
{
    public interface IHotkeyList
    {
        // Navigation
        KeyCode PlayerCruiser { get; }
        KeyCode Overview { get; }
        KeyCode EnemyCruiser { get; }

        // Building categories
        KeyCode Factories { get; }
        KeyCode Defensives { get; }
        KeyCode Offensives { get; }
        KeyCode Tacticals { get; }
        KeyCode Ultras { get; }

        // Boats
        KeyCode AttackBoat { get; }
        KeyCode Frigate { get; }
        KeyCode Destroyer { get; }
        KeyCode Archon { get; }
    }
}