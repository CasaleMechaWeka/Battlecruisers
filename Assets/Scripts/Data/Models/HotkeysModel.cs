using BattleCruisers.Hotkeys;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    [Serializable]
    public class HotkeysModel : IHotkeysModel, IHotkeyList
    {
        #region Navigation
        [SerializeField]
        private KeyCode _playerCruiser;
        public KeyCode PlayerCruiser
        {
            get => _playerCruiser;
            set => _playerCruiser = value;
        }

        [SerializeField]
        private KeyCode _overview;
        public KeyCode Overview
        {
            get => _overview;
            set => _overview = value;
        }

        [SerializeField]
        private KeyCode _enemyCruiser;
        public KeyCode EnemyCruiser
        {
            get => _enemyCruiser;
            set => _enemyCruiser = value;
        }
        #endregion Navigation

        #region Game speed

        private KeyCode _pauseSpeed;
        public KeyCode PauseSpeed
        {
            get => _pauseSpeed;
            set => _pauseSpeed = value;
        }
        private KeyCode _slowMotion;
        public KeyCode SlowMotion
        {
            get => _slowMotion;
            set => _slowMotion = value;
        }

        private KeyCode _play;
        public KeyCode NormalSpeed
        {
            get => _play;
            set => _play = value;
        }

        private KeyCode _fastForward;
        public KeyCode FastForward
        {
            get => _fastForward;
            set => _fastForward = value;
        }

        private KeyCode _toggleSpeed;
        public KeyCode ToggleSpeed
        {
            get => _toggleSpeed;
            set => _toggleSpeed = value;
        }
        #endregion Game speed

        #region Building categories
        [SerializeField]
        private KeyCode _factories;
        public KeyCode Factories
        {
            get => _factories;
            set => _factories = value;
        }

        [SerializeField]
        private KeyCode _defensives;
        public KeyCode Defensives
        {
            get => _defensives;
            set => _defensives = value;
        }

        [SerializeField]
        private KeyCode _offensives;
        public KeyCode Offensives
        {
            get => _offensives;
            set => _offensives = value;
        }

        [SerializeField]
        private KeyCode _tacticals;
        public KeyCode Tacticals
        {
            get => _tacticals;
            set => _tacticals = value;
        }

        [SerializeField]
        private KeyCode _ultras;
        public KeyCode Ultras
        {
            get => _ultras;
            set => _ultras = value;
        }
        #endregion Building categories

        #region Factories
        [SerializeField]
        private KeyCode _droneStation;
        public KeyCode DroneStation
        {
            get => _droneStation;
            set => _droneStation = value;
        }

        [SerializeField]
        private KeyCode _airFactory;
        public KeyCode AirFactory
        {
            get => _airFactory;
            set => _airFactory = value;
        }

        [SerializeField]
        private KeyCode _navalFactory;
        public KeyCode NavalFactory
        {
            get => _navalFactory;
            set => _navalFactory = value;
        }

        [SerializeField]
        private KeyCode _droneStation4;
        public KeyCode DroneStation4
        {
            get => _droneStation4;
            set => _droneStation4 = value;
        }

        [SerializeField]
        private KeyCode _droneStation8;
        public KeyCode DroneStation8
        {
            get => _droneStation8;
            set => _droneStation8 = value;
        }
        #endregion Factories

        #region Defensives
        [SerializeField]
        private KeyCode _shipTurret;
        public KeyCode ShipTurret
        {
            get => _shipTurret;
            set => _shipTurret = value;
        }

        [SerializeField]
        private KeyCode _airTurret;
        public KeyCode AirTurret
        {
            get => _airTurret;
            set => _airTurret = value;
        }

        [SerializeField]
        private KeyCode _mortar;
        public KeyCode Mortar
        {
            get => _mortar;
            set => _mortar = value;
        }

        [SerializeField]
        private KeyCode _samSite;
        public KeyCode SamSite
        {
            get => _samSite;
            set => _samSite = value;
        }

        [SerializeField]
        private KeyCode _teslaCoil;
        public KeyCode TeslaCoil
        {
            get => _teslaCoil;
            set => _teslaCoil = value;
        }
        #endregion Defensives

        #region Offensives
        [SerializeField]
        private KeyCode _artillery;
        public KeyCode Artillery
        {
            get => _artillery;
            set => _artillery = value;
        }

        [SerializeField]
        private KeyCode _railgun;
        public KeyCode Railgun
        {
            get => _railgun;
            set => _railgun = value;
        }

        [SerializeField]
        private KeyCode _rocketLauncher;
        public KeyCode RocketLauncher
        {
            get => _rocketLauncher;
            set => _rocketLauncher = value;
        }

        [SerializeField]
        private KeyCode _MLRS;
        public KeyCode MLRS
        {
            get => _MLRS;
            set => _MLRS = value;
        }

        [SerializeField]
        private KeyCode _gatlingMortar;
        public KeyCode GatlingMortar
        {
            get => _gatlingMortar;
            set => _gatlingMortar = value;
        }
        #endregion Offensives

        #region Tacticals
        [SerializeField]
        private KeyCode _shield;
        public KeyCode Shield
        {
            get => _shield;
            set => _shield = value;
        }

        [SerializeField]
        private KeyCode _booster;
        public KeyCode Booster
        {
            get => _booster;
            set => _booster = value;
        }

        [SerializeField]
        private KeyCode _stealthGenerator;
        public KeyCode StealthGenerator
        {
            get => _stealthGenerator;
            set => _stealthGenerator = value;
        }

        [SerializeField]
        private KeyCode _spySatellite;
        public KeyCode SpySatellite
        {
            get => _spySatellite;
            set => _spySatellite = value;
        }

        [SerializeField]
        private KeyCode _controlTower;
        public KeyCode ControlTower
        {
            get => _controlTower;
            set => _controlTower = value;
        }
        #endregion Tacticals

        #region Ultras
        [SerializeField]
        private KeyCode _deathStar;
        public KeyCode Deathstar
        {
            get => _deathStar;
            set => _deathStar = value;
        }

        [SerializeField]
        private KeyCode _nukeLauncher;
        public KeyCode NukeLauncher
        {
            get => _nukeLauncher;
            set => _nukeLauncher = value;
        }

        [SerializeField]
        private KeyCode _ultralisk;
        public KeyCode Ultralisk
        {
            get => _ultralisk;
            set => _ultralisk = value;
        }

        [SerializeField]
        private KeyCode _kamikazeSignal;
        public KeyCode KamikazeSignal
        {
            get => _kamikazeSignal;
            set => _kamikazeSignal = value;
        }

        [SerializeField]
        private KeyCode _broadsides;
        public KeyCode Broadsides
        {
            get => _broadsides;
            set => _broadsides = value;
        }
        #endregion Ultras

        #region Aircraft
        [SerializeField]
        private KeyCode _bomber;
        public KeyCode Bomber
        {
            get => _bomber;
            set => _bomber = value;
        }

        [SerializeField]
        private KeyCode _gunship;
        public KeyCode Gunship
        {
            get => _gunship;
            set => _gunship = value;
        }

        [SerializeField]
        private KeyCode _fighter;
        public KeyCode Fighter
        {
            get => _fighter;
            set => _fighter = value;
        }

        [SerializeField]
        private KeyCode _steamCopter;
        public KeyCode SteamCopter
        {
            get => _steamCopter;
            set => _steamCopter = value;
        }
        #endregion Aircraft

        #region Ships
        [SerializeField]
        private KeyCode _attackBoat;
        public KeyCode AttackBoat
        {
            get => _attackBoat;
            set => _attackBoat = value;
        }

        [SerializeField]
        private KeyCode _frigate;
        public KeyCode Frigate
        {
            get => _frigate;
            set => _frigate = value;
        }

        [SerializeField]
        private KeyCode _destroyer;
        public KeyCode Destroyer
        {
            get => _destroyer;
            set => _destroyer = value;
        }

        [SerializeField]
        private KeyCode _archon;
        public KeyCode Archon
        {
            get => _archon;
            set => _archon = value;
        }

        [SerializeField]
        private KeyCode _attackRIB;
        public KeyCode AttackRIB
        {
            get => _attackRIB;
            set => _attackRIB = value;
        }
        #endregion Ships

        public HotkeysModel()
        {
            // Navigation
            PlayerCruiser = KeyCode.Z;
            if (Application.systemLanguage == SystemLanguage.German)
            {
                PlayerCruiser = KeyCode.Y;
            }
            Overview = KeyCode.X;
            EnemyCruiser = KeyCode.C;

            // Game speed
            PauseSpeed = KeyCode.Alpha1;
            SlowMotion = KeyCode.Alpha2;
            NormalSpeed = KeyCode.Alpha3;
            FastForward = KeyCode.Alpha4;
            ToggleSpeed = KeyCode.Space;

            // Building categories
            Factories = KeyCode.A;
            Defensives = KeyCode.S;
            Offensives = KeyCode.D;
            Tacticals = KeyCode.F;
            Ultras = KeyCode.G;

            // Factories
            DroneStation = KeyCode.Q;
            AirFactory = KeyCode.W;
            NavalFactory = KeyCode.E;
            DroneStation4 = KeyCode.R;
            DroneStation8 = KeyCode.T;

            // Defensives
            ShipTurret = KeyCode.Q;
            AirTurret = KeyCode.W;
            Mortar = KeyCode.E;
            SamSite = KeyCode.R;
            TeslaCoil = KeyCode.T;

            // Offensives
            Artillery = KeyCode.Q;
            Railgun = KeyCode.W;
            RocketLauncher = KeyCode.E;
            MLRS = KeyCode.R;
            GatlingMortar = KeyCode.T;

            // Tacticals
            Shield = KeyCode.Q;
            Booster = KeyCode.W;
            StealthGenerator = KeyCode.E;
            SpySatellite = KeyCode.R;
            ControlTower = KeyCode.T;

            // Ultras
            Deathstar = KeyCode.Q;
            NukeLauncher = KeyCode.W;
            Ultralisk = KeyCode.E;
            KamikazeSignal = KeyCode.R;
            Broadsides = KeyCode.T;

            // Aircraft
            Bomber = KeyCode.Q;
            Gunship = KeyCode.W;
            Fighter = KeyCode.E;
            SteamCopter = KeyCode.R;

            // Ships
            AttackBoat = KeyCode.Q;
            Frigate = KeyCode.W;
            Destroyer = KeyCode.E;
            Archon = KeyCode.R;
            AttackRIB = KeyCode.T;
        }

        public static HotkeysModel CreateDefault()
        {
            return new HotkeysModel();
        }

        public override bool Equals(object obj)
        {
            HotkeysModel other = obj as HotkeysModel;

            return
                other != null
                // Navigation
                && PlayerCruiser == other.PlayerCruiser
                && Overview == other.Overview
                && EnemyCruiser == other.EnemyCruiser
                // Game speed
                && SlowMotion == other.SlowMotion
                && NormalSpeed == other.NormalSpeed
                && FastForward == other.FastForward
                && ToggleSpeed == other.ToggleSpeed
                // Building categories
                && Factories == other.Factories
                && Defensives == other.Defensives
                && Offensives == other.Offensives
                && Tacticals == other.Tacticals
                && Ultras == other.Ultras
                // Factories
                && DroneStation == other.DroneStation
                && AirFactory == other.AirFactory
                && NavalFactory == other.NavalFactory
                // Defensives
                && ShipTurret == other.ShipTurret
                && AirTurret == other.AirTurret
                && Mortar == other.Mortar
                && SamSite == other.SamSite
                && TeslaCoil == other.TeslaCoil
                // Offensives
                && Artillery == other.Artillery
                && Railgun == other.Railgun
                && RocketLauncher == other.RocketLauncher
                && MLRS == other.MLRS
                && GatlingMortar == other.GatlingMortar
                // Tacticals
                && Shield == other.Shield
                && Booster == other.Booster
                && StealthGenerator == other.StealthGenerator
                && SpySatellite == other.SpySatellite
                && ControlTower == other.ControlTower
                // Ultras
                && Deathstar == other.Deathstar
                && NukeLauncher == other.NukeLauncher
                && Ultralisk == other.Ultralisk
                && KamikazeSignal == other.KamikazeSignal
                && Broadsides == other.Broadsides
                // Aircraft
                && Bomber == other.Bomber
                && Gunship == other.Gunship
                && Fighter == other.Fighter
                // Boats
                && AttackBoat == other.AttackBoat
                && Frigate == other.Frigate
                && Destroyer == other.Destroyer
                && Archon == other.Archon;
        }

        public override int GetHashCode()
        {
            return 
                this.GetHashCode(
                    PlayerCruiser, Overview, EnemyCruiser,
                    SlowMotion, NormalSpeed, FastForward,
                    Factories, Defensives, Offensives, Tacticals, Ultras,
                    DroneStation, AirTurret, NavalFactory,
                    ShipTurret, AirTurret, Mortar, SamSite, TeslaCoil,
                    Artillery, Railgun, RocketLauncher,
                    Shield, Booster, StealthGenerator, SpySatellite, ControlTower,
                    Deathstar, NukeLauncher, Ultralisk, KamikazeSignal, Railgun,
                    Bomber, Gunship, Fighter,
                    AttackBoat, Frigate, Destroyer, Archon);
        }
    }
}