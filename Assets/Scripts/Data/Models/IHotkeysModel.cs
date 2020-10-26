using UnityEngine;

namespace BattleCruisers.Data.Models
{
    public interface IHotkeysModel
    {
        // Navigation
        KeyCode PlayerCruiser { get; set; }
        KeyCode Overview { get; set; }
        KeyCode EnemyCruiser { get; set; }
    }
}