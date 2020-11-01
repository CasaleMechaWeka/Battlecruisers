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

        // Factories
        event EventHandler DroneStation, AirFactory, NavalFactory;

        // Defensives
        event EventHandler ShipTurret, AirTurret, Mortar, SamSite, TeslaCoil;

        // Offensives
        event EventHandler Artillery, Railgun, RocketLauncher;

        // Tacticals
        event EventHandler Shield, Booster, StealthGenerator, SpySatellite, ControlTower;

        // Ultras
        event EventHandler Deathstar, NukeLauncher, Ultralisk, KamikazeSignal, Broadsides;

        // Aircraft
        event EventHandler Bomber, Gunship, Fighter;

        // Ships
        event EventHandler AttackBoat, Frigate, Destroyer, Archon;
    }
}