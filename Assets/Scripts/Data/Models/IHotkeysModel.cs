using UnityEngine;

namespace BattleCruisers.Data.Models
{
    public interface IHotkeysModel
    {
        // Navigation
        KeyCode PlayerCruiser { get; set; }
        KeyCode Overview { get; set; }
        KeyCode EnemyCruiser { get; set; }

        // Building categories
        KeyCode Factories { get; set; }
        KeyCode Defensives { get; set; }
        KeyCode Offensives { get; set; }
        KeyCode Tacticals { get; set; }
        KeyCode Ultras { get; set; }

        // Boats
        KeyCode AttackBoat { get; set; }
        KeyCode Frigate { get; set; }
        KeyCode Destroyer { get; set; }
        KeyCode Archon { get; set; }
    }
}