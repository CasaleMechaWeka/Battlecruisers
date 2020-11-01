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

        // Factories
        KeyCode DroneStation { get; }
        KeyCode AirFactory { get; }
        KeyCode NavalFactory { get; }

        // Defensives
        KeyCode ShipTurret { get; }
        KeyCode AirTurret { get; }
        KeyCode Mortar { get; }
        KeyCode SamSite { get; }
        KeyCode TeslaCoil { get; }

        // Boats
        KeyCode AttackBoat { get; }
        KeyCode Frigate { get; }
        KeyCode Destroyer { get; }
        KeyCode Archon { get; }
    }
}