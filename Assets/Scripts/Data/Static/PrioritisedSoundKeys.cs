using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data.Static
{
    public static class PrioritisedSoundKeys
    {
        public static class Completed
        {
            public static class Buildings
            {
                public static PrioritisedSoundKey Building { get; }
                public static PrioritisedSoundKey AirFactory { get; }
                public static PrioritisedSoundKey AntiAirTurret { get; }
                public static PrioritisedSoundKey AntiShipTurret { get; }
                public static PrioritisedSoundKey Artillery { get; }
                public static PrioritisedSoundKey Booster { get; }
                public static PrioritisedSoundKey DroneStation { get; }
                public static PrioritisedSoundKey ControlTower { get; }
                public static PrioritisedSoundKey Mortar { get; }
                public static PrioritisedSoundKey NavalFactory { get; }
                public static PrioritisedSoundKey Railgun { get; }
                public static PrioritisedSoundKey RocketLauncher { get; }
                public static PrioritisedSoundKey SamSite { get; }
                public static PrioritisedSoundKey SpySatellite { get; }
                public static PrioritisedSoundKey Shields { get; }
                public static PrioritisedSoundKey StealthGenerator { get; }
                public static PrioritisedSoundKey TeslaCoil { get; }

                static Buildings()
                {
                    AirFactory = new PrioritisedSoundKey(SoundKeys.Completed.AirFactory, SoundPriority.VeryLow);
                    AntiAirTurret = new PrioritisedSoundKey(SoundKeys.Completed.AntiAirTurret, SoundPriority.VeryLow);
                    AntiShipTurret = new PrioritisedSoundKey(SoundKeys.Completed.AntiShipTurret, SoundPriority.VeryLow);
                    Artillery = new PrioritisedSoundKey(SoundKeys.Completed.Artillery, SoundPriority.VeryLow);
                    Booster = new PrioritisedSoundKey(SoundKeys.Completed.Booster, SoundPriority.VeryLow);
                    DroneStation = new PrioritisedSoundKey(SoundKeys.Completed.DroneStation, SoundPriority.VeryLow);
                    ControlTower = new PrioritisedSoundKey(SoundKeys.Completed.ControlTower, SoundPriority.VeryLow);
                    Mortar = new PrioritisedSoundKey(SoundKeys.Completed.Mortar, SoundPriority.VeryLow);
                    NavalFactory = new PrioritisedSoundKey(SoundKeys.Completed.NavalFactory, SoundPriority.VeryLow);
                    Railgun = new PrioritisedSoundKey(SoundKeys.Completed.Railgun, SoundPriority.VeryLow);
                    RocketLauncher = new PrioritisedSoundKey(SoundKeys.Completed.RocketLauncher, SoundPriority.VeryLow);
                    SamSite = new PrioritisedSoundKey(SoundKeys.Completed.SamSite, SoundPriority.VeryLow);
                    SpySatellite = new PrioritisedSoundKey(SoundKeys.Completed.SpySatellite, SoundPriority.VeryLow);
                    Shields = new PrioritisedSoundKey(SoundKeys.Completed.Shields, SoundPriority.VeryLow);
                    StealthGenerator = new PrioritisedSoundKey(SoundKeys.Completed.StealthGenerator, SoundPriority.VeryLow);
                    TeslaCoil = new PrioritisedSoundKey(SoundKeys.Completed.TeslaCoil, SoundPriority.VeryLow);
                }
            }

            public static class Units
            {
                public static PrioritisedSoundKey AttackBoat { get; }
                public static PrioritisedSoundKey Frigate { get; }
                public static PrioritisedSoundKey Destroyer { get; }
                public static PrioritisedSoundKey Bomber { get; }
                public static PrioritisedSoundKey Gunship { get; }
                public static PrioritisedSoundKey Fighter { get; }
                public static PrioritisedSoundKey Satellite { get; }

                static Units()
                {
                    AttackBoat = new PrioritisedSoundKey(SoundKeys.Completed.AttackBoat, SoundPriority.VeryLow);
                    Frigate = new PrioritisedSoundKey(SoundKeys.Completed.Frigate, SoundPriority.VeryLow);
                    Destroyer = new PrioritisedSoundKey(SoundKeys.Completed.Destroyer, SoundPriority.VeryLow);
                    Bomber = new PrioritisedSoundKey(SoundKeys.Completed.Bomber, SoundPriority.VeryLow);
                    Gunship = new PrioritisedSoundKey(SoundKeys.Completed.Gunship, SoundPriority.VeryLow);
                    Fighter = new PrioritisedSoundKey(SoundKeys.Completed.Fighter, SoundPriority.VeryLow);
                    Satellite = new PrioritisedSoundKey(SoundKeys.Completed.SpySatellite, SoundPriority.VeryLow);
                }
            }

            public static PrioritisedSoundKey Ultra { get; }

            static Completed()
            {
                Ultra = new PrioritisedSoundKey(SoundKeys.Completed.Ultra, SoundPriority.VeryHigh);
            }
        }

        public static class Events
        {
            public static class Cruiser
            {
                public static PrioritisedSoundKey UnderAttack { get; }
                public static PrioritisedSoundKey SignificantlyDamaged { get; }

                static Cruiser()
                {
                    UnderAttack = new PrioritisedSoundKey(SoundKeys.Events.CruiserUnderAttack, SoundPriority.Normal);
                    SignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.Events.CruiserSignificantlyDamaged, SoundPriority.High);
                }
            }

            public static class Drones
            {
                public static PrioritisedSoundKey NewDronesReady { get; }
                public static PrioritisedSoundKey Idle { get; }
                public static PrioritisedSoundKey NotEnoughDronesToBuild { get; }
                public static PrioritisedSoundKey NotEnoughDronesToFocus { get; }
                public static PrioritisedSoundKey Focusing { get; }
                public static PrioritisedSoundKey AllFocused { get; }
                public static PrioritisedSoundKey Dispersing { get; }

                static Drones()
                {
                    NewDronesReady = new PrioritisedSoundKey(SoundKeys.Events.DronesNewDronesReady, SoundPriority.VeryLow);
                    Idle = new PrioritisedSoundKey(SoundKeys.Events.DronesIdle, SoundPriority.Low);
                    NotEnoughDronesToBuild = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToBuild, SoundPriority.VeryHigh);
                    NotEnoughDronesToFocus = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToFocus, SoundPriority.High);
                    Focusing = new PrioritisedSoundKey(SoundKeys.Events.DronesFocusing, SoundPriority.High);
                    AllFocused = new PrioritisedSoundKey(SoundKeys.Events.DronesAllFocused, SoundPriority.High);
                    Dispersing = new PrioritisedSoundKey(SoundKeys.Events.DronesDispersing, SoundPriority.High);
                }
            }

            public static class Targetting
            {
                public static PrioritisedSoundKey NewTarget { get; }
                public static PrioritisedSoundKey TargetCleared { get; }
            
                static Targetting()
                {
                    NewTarget = new PrioritisedSoundKey(SoundKeys.Events.TargettingNewTarget, SoundPriority.Normal);
                    TargetCleared = new PrioritisedSoundKey(SoundKeys.Events.TargettingDeselected, SoundPriority.Normal);
                }
            }

            public static PrioritisedSoundKey EnemyStartedUltra { get; }
            public static PrioritisedSoundKey ShieldsDown { get; }

            static Events()
            {
                EnemyStartedUltra = new PrioritisedSoundKey(SoundKeys.Events.EnemyStartedUltra, SoundPriority.VeryHigh);
                ShieldsDown = new PrioritisedSoundKey(SoundKeys.Events.ShieldsDown, SoundPriority.Normal);
            }
        }
    }
}