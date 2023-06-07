using UnityEngine;

namespace BattleCruisers.Data.Models
{
    public interface IHotkeysModel
    {
        // Navigation
        KeyCode PlayerCruiser { get; set; }
        KeyCode Overview { get; set; }
        KeyCode EnemyCruiser { get; set; }

        // Game speed
        KeyCode PauseSpeed { get; set; }
        KeyCode SlowMotion { get; set; }
        KeyCode NormalSpeed { get; set; }
        KeyCode FastForward { get; set; }
        KeyCode ToggleSpeed { get; set; }

        // Building categories
        KeyCode Factories { get; set; }
        KeyCode Defensives { get; set; }
        KeyCode Offensives { get; set; }
        KeyCode Tacticals { get; set; }
        KeyCode Ultras { get; set; }

        // Factories
        KeyCode DroneStation { get; set; }
        KeyCode AirFactory { get; set; }
        KeyCode NavalFactory { get; set; }
        KeyCode DroneStation4 { get; set;}
        KeyCode DroneStation8 { get; set;}

        // Offensives
        KeyCode Artillery { get; set; }
        KeyCode Railgun { get; set; }
        KeyCode RocketLauncher { get; set; }
        KeyCode MLRS { get; set; }
        KeyCode GatlingMortar { get; set; }

        // Tacticals
        KeyCode Shield { get; set; }
        KeyCode Booster { get; set; }
        KeyCode StealthGenerator { get; set; }
        KeyCode SpySatellite { get; set; }
        KeyCode ControlTower { get; set; }

        // Ultras
        KeyCode Deathstar { get; set; }
        KeyCode NukeLauncher { get; set; }
        KeyCode Ultralisk { get; set; }
        KeyCode KamikazeSignal { get; set; }
        KeyCode Broadsides { get; set; }

        // Aircraft
        KeyCode Bomber { get; set; }
        KeyCode Gunship { get; set; }
        KeyCode Fighter { get; set; }
        KeyCode SteamCopter { get; set; }
        KeyCode Broadsword { get; set; }

        // Defensives
        KeyCode ShipTurret { get; set; }
        KeyCode AirTurret { get; set; }
        KeyCode Mortar { get; set; }
        KeyCode SamSite { get; set; }
        KeyCode TeslaCoil { get; set; }

        // Boats
        KeyCode AttackBoat { get; set; }
        KeyCode Frigate { get; set; }
        KeyCode Destroyer { get; set; }
        KeyCode Archon { get; set; }
        KeyCode AttackRIB { get; set; }
    }
}