using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys
{
    public interface IPvPHotkeyDetector : IPvPManagedDisposable
    {
        // Navigation
        event EventHandler PlayerCruiser, Overview, EnemyCruiser;

        // Game speed
        event EventHandler PauseSpeed, SlowMotion, NormalSpeed, FastForward, ToggleSpeed;

        // Building categories
        event EventHandler Factories, Defensives, Offensives, Tacticals, Ultras;

        // Factories
        event EventHandler DroneStation, AirFactory, NavalFactory, DroneStation4, DroneStation8;

        // Defensives
        event EventHandler ShipTurret, AirTurret, Mortar, SamSite, TeslaCoil;

        // Offensives
        event EventHandler Artillery, Railgun, RocketLauncher, MLRS, GatlingMortar;

        // Tacticals
        event EventHandler Shield, Booster, StealthGenerator, SpySatellite, ControlTower;

        // Ultras
        event EventHandler Deathstar, NukeLauncher, Ultralisk, KamikazeSignal, Broadsides;

        // Aircraft
        event EventHandler Bomber, Gunship, Fighter, SteamCopter;

        // Ships
        event EventHandler AttackBoat, Frigate, Destroyer, Archon, AttackRIB;
    }
}