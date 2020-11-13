using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.Hotkeys
{
    public class HotkeyDetector : IHotkeyDetector
    {
        private readonly IHotkeyList _hotkeyList;
        private readonly IInput _input;
        private readonly IUpdater _updater;
        private readonly IBroadcastingFilter _filter;

        // Navigation
        public event EventHandler PlayerCruiser, Overview, EnemyCruiser;

        // Game speed
        public event EventHandler SlowMotion, Play, FastForward;

        // Buliding categories
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

        public HotkeyDetector(
            IHotkeyList hotkeyList, 
            IInput input, 
            IUpdater updater,
            IBroadcastingFilter filter)
        {
            Helper.AssertIsNotNull(hotkeyList, input, updater, filter);

            _hotkeyList = hotkeyList;
            _input = input;
            _updater = updater;
            _filter = filter;

            _updater.Updated += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (!_filter.IsMatch)
            {
                return;
            }

            // Navigation
            if (_input.GetKeyUp(_hotkeyList.PlayerCruiser))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.PlayerCruiser: {_hotkeyList.PlayerCruiser}");
                PlayerCruiser?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Overview))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Overview: {_hotkeyList.Overview}");
                Overview?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.EnemyCruiser))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.EnemyCruiser: {_hotkeyList.EnemyCruiser}");
                EnemyCruiser?.Invoke(this, EventArgs.Empty);
            }

            // Game speed
            if (_input.GetKeyUp(_hotkeyList.SlowMotion))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.SlowMotion: {_hotkeyList.SlowMotion}");
                SlowMotion?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Play))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Play: {_hotkeyList.Play}");
                Play?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.FastForward))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.FastForward: {_hotkeyList.FastForward}");
                FastForward?.Invoke(this, EventArgs.Empty);
            }

            // Building categories
            if (_input.GetKeyUp(_hotkeyList.Factories))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Factories: {_hotkeyList.Factories}");
                Factories?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Defensives))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Defensives: {_hotkeyList.Defensives}");
                Defensives?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Offensives))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Offensives: {_hotkeyList.Offensives}");
                Offensives?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Tacticals))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Tacticals: {_hotkeyList.Tacticals}");
                Tacticals?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Ultras))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Ultras: {_hotkeyList.Ultras}");
                Ultras?.Invoke(this, EventArgs.Empty);
            }

            // Factories
            if (_input.GetKeyUp(_hotkeyList.DroneStation))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.DroneStation: {_hotkeyList.DroneStation}");
                DroneStation?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.AirFactory))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.AirFactory: {_hotkeyList.AirFactory}");
                AirFactory?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.NavalFactory))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.NavalFactory: {_hotkeyList.NavalFactory}");
                NavalFactory?.Invoke(this, EventArgs.Empty);
            }

            // Defensives
            if (_input.GetKeyUp(_hotkeyList.ShipTurret))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.ShipTurret: {_hotkeyList.ShipTurret}");
                ShipTurret?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.AirTurret))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.AirTurret: {_hotkeyList.AirTurret}");
                AirTurret?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Mortar))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Mortar: {_hotkeyList.Mortar}");
                Mortar?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.SamSite))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.SamSite: {_hotkeyList.SamSite}");
                SamSite?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.TeslaCoil))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.TeslaCoil: {_hotkeyList.TeslaCoil}");
                TeslaCoil?.Invoke(this, EventArgs.Empty);
            }

            // Offensives
            if (_input.GetKeyUp(_hotkeyList.Artillery))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Artillery: {_hotkeyList.Artillery}");
                Artillery?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Railgun))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Railgun: {_hotkeyList.Railgun}");
                Railgun?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.RocketLauncher))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.RocketLauncher: {_hotkeyList.RocketLauncher}");
                RocketLauncher?.Invoke(this, EventArgs.Empty);
            }

            // Tacticals
            if (_input.GetKeyUp(_hotkeyList.Shield))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Shield: {_hotkeyList.Shield}");
                Shield?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Booster))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Booster: {_hotkeyList.Booster}");
                Booster?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.StealthGenerator))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.StealthGenerator: {_hotkeyList.StealthGenerator}");
                StealthGenerator?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.SpySatellite))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.SpySatellite: {_hotkeyList.SpySatellite}");
                SpySatellite?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.ControlTower))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.ControlTower: {_hotkeyList.ControlTower}");
                ControlTower?.Invoke(this, EventArgs.Empty);
            }

            // Ultras
            if (_input.GetKeyUp(_hotkeyList.Deathstar))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Deathstar: {_hotkeyList.Deathstar}");
                Deathstar?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.NukeLauncher))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.NukeLauncher: {_hotkeyList.NukeLauncher}");
                NukeLauncher?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Ultralisk))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Ultralisk: {_hotkeyList.Ultralisk}");
                Ultralisk?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.KamikazeSignal))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.KamikazeSignal: {_hotkeyList.KamikazeSignal}");
                KamikazeSignal?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Broadsides))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Broadsides: {_hotkeyList.Broadsides}");
                Broadsides?.Invoke(this, EventArgs.Empty);
            }

            // Aircraft
            if (_input.GetKeyUp(_hotkeyList.Bomber))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Bomber: {_hotkeyList.Bomber}");
                Bomber?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Gunship))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Gunship: {_hotkeyList.Gunship}");
                Gunship?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Fighter))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Fighter: {_hotkeyList.Fighter}");
                Fighter?.Invoke(this, EventArgs.Empty);
            }

            // Boats
            if (_input.GetKeyUp(_hotkeyList.AttackBoat))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.AttackBoat: {_hotkeyList.AttackBoat}");
                AttackBoat?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Frigate))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Frigate: {_hotkeyList.Frigate}");
                Frigate?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Destroyer))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Destroyer: {_hotkeyList.Destroyer}");
                Destroyer?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Archon))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Archon: {_hotkeyList.Archon}");
                Archon?.Invoke(this, EventArgs.Empty);
            }
        }

        public void DisposeManagedState()
        {
            _updater.Updated -= _updater_Updated;
        }
    }
}