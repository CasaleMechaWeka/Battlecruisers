using System;

namespace BattleCruisers.Hotkeys
{
    public class NullHotkeyDetector : IHotkeyDetector
    {
#pragma warning disable 67  // Unused event
        // Navigation
        public event EventHandler PlayerCruiser, Overview, EnemyCruiser;

        // Game speed
        public event EventHandler PauseSpeed, SlowMotion, NormalSpeed, FastForward;

        // Building categories
        public event EventHandler Factories, Defensives, Offensives, Tacticals, Ultras;

        // Factories
        public event EventHandler DroneStation, AirFactory, NavalFactory, DroneStation4, DroneStation8;

        // Defensives
        public event EventHandler ShipTurret, AirTurret, Mortar, SamSite, TeslaCoil;

        // Offensives
        public event EventHandler Artillery, Railgun, RocketLauncher, MLRS, GatlingMortar;

        // Tacticals
        public event EventHandler Shield, Booster, StealthGenerator, SpySatellite, ControlTower;

        // Ultras
        public event EventHandler Deathstar, NukeLauncher, Ultralisk, KamikazeSignal, Broadsides;

        // Aircraft
        public event EventHandler Bomber, Gunship, Fighter, SteamCopter;

        // Ships
        public event EventHandler AttackBoat, Frigate, Destroyer, Archon, AttackRIB;
#pragma warning restore 67  // Unused event

        public void DisposeManagedState() { }
    }
}