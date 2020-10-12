using UnityEngine;

namespace BattleCruisers.Hotkeys
{
    public interface IHotkeyList
    {
        // Navigation
        KeyCode PlayerCruiser { get; }
        KeyCode Overview { get; }
        KeyCode EnemyCruiser { get; }
    }
}