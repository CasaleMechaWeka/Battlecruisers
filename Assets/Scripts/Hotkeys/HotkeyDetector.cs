using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using BattleCruisers.Buildables.Buildings;
using UnityEngine;

namespace BattleCruisers.Hotkeys
{
    public class HotkeyDetector : IHotkeyDetector
    {
        private readonly IHotkeyList _hotkeyList;
        private readonly IInput _input;
        private readonly IUpdater _updater;
        private readonly IBroadcastingFilter _filter;
        private IUIManager _UIManager;

        // Navigation
        public event EventHandler PlayerCruiser, Overview, EnemyCruiser;

        // Game speed
        public event EventHandler PauseSpeed, SlowMotion, NormalSpeed, FastForward, ToggleSpeed;

        // Buliding categories
        public event EventHandler Factories, Defensives, Offensives, Tacticals, Ultras;

        // Factories
        public event EventHandler FactoryButton1, FactoryButton2, FactoryButton3, FactoryButton4, FactoryButton5;

        // Defensives
        public event EventHandler DefensiveButton1, DefensiveButton2, DefensiveButton3, DefensiveButton4, DefensiveButton5;

        // Offensives
        public event EventHandler OffensiveButton1, OffensiveButton2, OffensiveButton3, OffensiveButton4, OffensiveButton5;

        // Tacticals
        public event EventHandler TacticalButton1, TacticalButton2, TacticalButton3, TacticalButton4, TacticalButton5;

        // Ultras
        public event EventHandler UltraButton1, UltraButton2, UltraButton3, UltraButton4, UltraButton5;

        // Aircraft
        public event EventHandler AircraftButton1, AircraftButton2, AircraftButton3, AircraftButton4, AircraftButton5;

        // Ships
        public event EventHandler ShipButton1, ShipButton2, ShipButton3, ShipButton4, ShipButton5;

        public HotkeyDetector(
            IHotkeyList hotkeyList, 
            IInput input, 
            IUpdater updater,
            IBroadcastingFilter filter,
            IUIManager uIManager)
        {
            Helper.AssertIsNotNull(hotkeyList, input, updater, filter);

            _hotkeyList = hotkeyList;
            _input = input;
            _updater = updater;
            _filter = filter;
            _UIManager = uIManager;

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
            /*if (_input.GetKeyUp(_hotkeyList.PauseSpeed))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.SlowMotion: {_hotkeyList.PauseSpeed}");
                PauseSpeed?.Invoke(this, EventArgs.Empty);
            }*/
            if (_input.GetKeyUp(_hotkeyList.SlowMotion))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.SlowMotion: {_hotkeyList.SlowMotion}");
                SlowMotion?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.NormalSpeed))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Play: {_hotkeyList.NormalSpeed}");
                NormalSpeed?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.FastForward))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.FastForward: {_hotkeyList.FastForward}");
                FastForward?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.ToggleSpeed))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.FastForward: {_hotkeyList.ToggleSpeed}");
                ToggleSpeed?.Invoke(this, EventArgs.Empty);
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
                FactoryButton1?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.AirFactory))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.AirFactory: {_hotkeyList.AirFactory}");
                FactoryButton2?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.NavalFactory))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.NavalFactory: {_hotkeyList.NavalFactory}");
                FactoryButton3?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.DroneStation4))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.DroneStation: {_hotkeyList.DroneStation4}");
                FactoryButton4?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.DroneStation8))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.DroneStation: {_hotkeyList.DroneStation8}");
                FactoryButton5?.Invoke(this, EventArgs.Empty);
            }


            // Defensives
            if (_input.GetKeyUp(_hotkeyList.ShipTurret))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.ShipTurret: {_hotkeyList.ShipTurret}");
                DefensiveButton1?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.AirTurret))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.AirTurret: {_hotkeyList.AirTurret}");
                DefensiveButton2?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Mortar))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Mortar: {_hotkeyList.Mortar}");
                DefensiveButton3?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.SamSite))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.SamSite: {_hotkeyList.SamSite}");
                DefensiveButton4?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.TeslaCoil))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.TeslaCoil: {_hotkeyList.TeslaCoil}");
                DefensiveButton5?.Invoke(this, EventArgs.Empty);
            }


            // Offensives
            if (_input.GetKeyUp(_hotkeyList.Artillery))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Artillery: {_hotkeyList.Artillery}");
                OffensiveButton1?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Railgun))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Railgun: {_hotkeyList.Railgun}");
                OffensiveButton2?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.RocketLauncher))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.RocketLauncher: {_hotkeyList.RocketLauncher}");
                OffensiveButton3?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.MLRS))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.RocketLauncher: {_hotkeyList.MLRS}");
                OffensiveButton4?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.GatlingMortar))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.RocketLauncher: {_hotkeyList.GatlingMortar}");
                OffensiveButton5?.Invoke(this, EventArgs.Empty);
            }


            // Tacticals
            if (_input.GetKeyUp(_hotkeyList.Shield))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Shield: {_hotkeyList.Shield}");
                TacticalButton1?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Booster))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Booster: {_hotkeyList.Booster}");
                TacticalButton2?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.StealthGenerator))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.StealthGenerator: {_hotkeyList.StealthGenerator}");
                TacticalButton3?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.SpySatellite))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.SpySatellite: {_hotkeyList.SpySatellite}");
                TacticalButton4?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.ControlTower))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.ControlTower: {_hotkeyList.ControlTower}");
                TacticalButton5?.Invoke(this, EventArgs.Empty);
            }

            // Ultras
            if (_input.GetKeyUp(_hotkeyList.Deathstar))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Deathstar: {_hotkeyList.Deathstar}");
                UltraButton1?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.NukeLauncher))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.NukeLauncher: {_hotkeyList.NukeLauncher}");
                UltraButton2?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Ultralisk))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Ultralisk: {_hotkeyList.Ultralisk}");
                UltraButton3?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.KamikazeSignal))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.KamikazeSignal: {_hotkeyList.KamikazeSignal}");
                UltraButton4?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Broadsides))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Broadsides: {_hotkeyList.Broadsides}");
                UltraButton5?.Invoke(this, EventArgs.Empty);
            }



            // Aircraft
            if (_input.GetKeyUp(_hotkeyList.Bomber))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Bomber: {_hotkeyList.Bomber}");
                AircraftButton1?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Gunship))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Gunship: {_hotkeyList.Gunship}");
                AircraftButton2?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Fighter))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Fighter: {_hotkeyList.Fighter}");
                AircraftButton3?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.SteamCopter))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Fighter: {_hotkeyList.SteamCopter}");
                AircraftButton4?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Broadsword))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Fighter: {_hotkeyList.Broadsword}");
                AircraftButton4?.Invoke(this, EventArgs.Empty);
            }

            // Naval
            if (_input.GetKeyUp(_hotkeyList.AttackBoat))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.AttackBoat: {_hotkeyList.AttackBoat}");
                ShipButton1?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Frigate))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Frigate: {_hotkeyList.Frigate}");
                ShipButton2?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Destroyer))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Destroyer: {_hotkeyList.Destroyer}");
                ShipButton3?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.Archon))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Archon: {_hotkeyList.Archon}");
                ShipButton4?.Invoke(this, EventArgs.Empty);
            }
            if (_input.GetKeyUp(_hotkeyList.AttackRIB))
            {
                Logging.Log(Tags.HOTKEYS, $"Got _hotkeyList.Archon: {_hotkeyList.AttackRIB}");
                ShipButton5?.Invoke(this, EventArgs.Empty);
            }
            
        }

        public void DisposeManagedState()
        {
            _updater.Updated -= _updater_Updated;
        }
    }
}