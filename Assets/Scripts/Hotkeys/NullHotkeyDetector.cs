using System;

namespace BattleCruisers.Hotkeys
{
    public class NullHotkeyDetector : IHotkeyDetector
    {
        // Navigation
        public event EventHandler PlayerCruiser, Overview, EnemyCruiser;

        // Building categories
        public event EventHandler Factories, Defensives, Offensives, Tacticals, Ultras;

        // Factories
        public event EventHandler DroneStation, AirFactory, NavalFactory;

        // Defensives
        public event EventHandler ShipTurret, AirTurret, Mortar, SamSite, TeslaCoil;

        // Offensives
        public event EventHandler Artillery, Railgun, RocketLauncher;

        // Tacticals
        public event EventHandler Shield, Booster, StealthGenerator, SpySatellite, ControlTower;

        // Ultras
        public event EventHandler Deathstar, NukeLauncher, Ultralisk, KamikazeSignal, Broadsides;

        // Aircraft
        public event EventHandler Bomber, Gunship, Fighter;

        // Ships
        public event EventHandler AttackBoat, Frigate, Destroyer, Archon;

        public void DisposeManagedState() { }
    }
}