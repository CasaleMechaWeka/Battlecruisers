using UnityEngine;

namespace BattleCruisers.Hotkeys
{
    public interface IHotkeyList
    {
        // Navigation
        KeyCode PlayerCruiser { get; }
        KeyCode Overview { get; }
        KeyCode EnemyCruiser { get; }

        // Game speed
        KeyCode PauseSpeed { get; }
        KeyCode SlowMotion { get; }
        KeyCode NormalSpeed { get; }
        KeyCode FastForward { get; }
        KeyCode ToggleSpeed { get; }

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
        KeyCode DroneStation4 { get; }
        KeyCode DroneStation8 { get; }

        // Defensives
        KeyCode ShipTurret { get; }
        KeyCode AirTurret { get; }
        KeyCode Mortar { get; }
        KeyCode SamSite { get; }
        KeyCode TeslaCoil { get; }

        // Offensives
        KeyCode Artillery { get; }
        KeyCode Railgun { get; }
        KeyCode RocketLauncher { get; }
        KeyCode MLRS { get; }
        KeyCode GatlingMortar { get; }

        // Tacticals
        KeyCode Shield { get; }
        KeyCode Booster { get; }
        KeyCode StealthGenerator { get; }
        KeyCode SpySatellite { get; }
        KeyCode ControlTower { get; }

        // Ultras
        KeyCode Deathstar { get; }
        KeyCode NukeLauncher { get; }
        KeyCode Ultralisk { get; }
        KeyCode KamikazeSignal { get; }
        KeyCode Broadsides { get; }

        // Aircraft
        KeyCode Bomber { get; }
        KeyCode Gunship { get; }
        KeyCode Fighter { get; }
        KeyCode SteamCopter { get; }
        KeyCode Broadsword { get; }

        // Boats
        KeyCode AttackBoat { get; }
        KeyCode Frigate { get; }
        KeyCode Destroyer { get; }
        KeyCode Archon { get; }
        KeyCode AttackRIB { get; }
    }
}