using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.Hotkeys
{
    // FELIX  Test
    public class HotkeyDetector : IHotkeyDetector
    {
        private readonly IHotkeyList _hotkeyList;
        private readonly IInput _input;
        private readonly IUpdater _updater;

        // Navigation
        public event EventHandler PlayerCruiser, Overview, EnemyCruiser;

        // Buliding categories
        public event EventHandler Factories, Defensives, Offensives, Tacticals, Ultras;

        // Factories
        public event EventHandler DroneStation, AirFactory, NavalFactory;

        // Defensives
        public event EventHandler ShipTurret, AirTurret, Mortar, SamSite, TeslaCoil;

        // Ships
        public event EventHandler AttackBoat, Frigate, Destroyer, Archon;

        public HotkeyDetector(IHotkeyList hotkeyList, IInput input, IUpdater updater)
        {
            Helper.AssertIsNotNull(hotkeyList, input, updater);

            _hotkeyList = hotkeyList;
            _input = input;
            _updater = updater;

            _updater.Updated += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
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