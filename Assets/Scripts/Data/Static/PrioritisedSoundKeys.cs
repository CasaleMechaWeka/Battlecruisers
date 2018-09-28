using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data.Static
{
    public static class PrioritisedSoundKeys
    {
        public static class Completed
        {
            public static class Buildings
            {
                public static PrioritisedSoundKey Building { get; private set; }
                public static PrioritisedSoundKey AirFactory { get; private set; }
                public static PrioritisedSoundKey AntiAirTurret { get; private set; }
                public static PrioritisedSoundKey AntiShipTurret { get; private set; }
                public static PrioritisedSoundKey Artillery { get; private set; }
                public static PrioritisedSoundKey Booster { get; private set; }
                public static PrioritisedSoundKey DroneStation { get; private set; }
                public static PrioritisedSoundKey ControlTower { get; private set; }
                public static PrioritisedSoundKey Mortar { get; private set; }
                public static PrioritisedSoundKey NavalFactory { get; private set; }
                public static PrioritisedSoundKey Railgun { get; private set; }
                public static PrioritisedSoundKey RocketLauncher { get; private set; }
                public static PrioritisedSoundKey SamSite { get; private set; }
                public static PrioritisedSoundKey SpySatellite { get; private set; }
                public static PrioritisedSoundKey Shields { get; private set; }
                public static PrioritisedSoundKey StealthGenerator { get; private set; }
                public static PrioritisedSoundKey TeslaCoil { get; private set; }

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
                public static PrioritisedSoundKey AttackBoat { get; private set; }
                public static PrioritisedSoundKey Frigate { get; private set; }
                public static PrioritisedSoundKey Destroyer { get; private set; }
                public static PrioritisedSoundKey Bomber { get; private set; }
                public static PrioritisedSoundKey Gunship { get; private set; }
                public static PrioritisedSoundKey Fighter { get; private set; }
                public static PrioritisedSoundKey Satellite { get; private set; }

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

            public static PrioritisedSoundKey Ultra { get; private set; }

            static Completed()
            {
                Ultra = new PrioritisedSoundKey(SoundKeys.Completed.Ultra, SoundPriority.VeryHigh);
            }
        }

        public static class Events
        {
            public static class Cruiser
            {
                public static PrioritisedSoundKey UnderAttack { get; private set; }
                public static PrioritisedSoundKey SignificantlyDamaged { get; private set; }

                static Cruiser()
                {
                    UnderAttack = new PrioritisedSoundKey(SoundKeys.Events.CruiserUnderAttack, SoundPriority.Normal);
                    SignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.Events.CruiserSignificantlyDamaged, SoundPriority.High);
                }
            }

            public static class Drones
            {
                public static PrioritisedSoundKey NewDronesReady { get; private set; }
                public static PrioritisedSoundKey Idle { get; private set; }
                public static PrioritisedSoundKey NotEnoughDronesToBuild { get; private set; }
                public static PrioritisedSoundKey NotEnoughDronesToFocus { get; private set; }
                public static PrioritisedSoundKey Focusing { get; private set; }
                public static PrioritisedSoundKey AllFocused { get; private set; }
                public static PrioritisedSoundKey Dispersing { get; private set; }

                static Drones()
                {
                    // Drones
                    NewDronesReady = new PrioritisedSoundKey(SoundKeys.Events.DronesNewDronesReady, SoundPriority.VeryLow);
                    Idle = new PrioritisedSoundKey(SoundKeys.Events.DronesIdle, SoundPriority.Low);
                    NotEnoughDronesToBuild = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDrones, SoundPriority.VeryHigh);
                    // FELIX:  Use different sound once Peter provides :)
                    NotEnoughDronesToFocus = new PrioritisedSoundKey(SoundKeys.Events.DronesFocusing, SoundPriority.High);
                    Focusing = new PrioritisedSoundKey(SoundKeys.Events.DronesFocusing, SoundPriority.High);
                    AllFocused = new PrioritisedSoundKey(SoundKeys.Events.DronesAllFocused, SoundPriority.High);
                    // FELIX:  Use different sound once Peter provides :)
                    Dispersing = new PrioritisedSoundKey(SoundKeys.Events.DronesFocusing, SoundPriority.High);
                }
            }

            public static class Targetting
            {
                public static PrioritisedSoundKey NewTarget { get; private set; }
                public static PrioritisedSoundKey TargetCleared { get; private set; }
            
                static Targetting()
                {
                    NewTarget = new PrioritisedSoundKey(SoundKeys.Events.TargettingNewTarget, SoundPriority.Normal);
                    // FELIX:  Use different sound once Peter provides :)
                    TargetCleared = new PrioritisedSoundKey(SoundKeys.Events.TargettingNewTarget, SoundPriority.Normal);
                }
            }

            public static PrioritisedSoundKey EnemyStartedUltra { get; private set; }
            public static PrioritisedSoundKey ShieldsDown { get; private set; }

            static Events()
            {
                EnemyStartedUltra = new PrioritisedSoundKey(SoundKeys.Events.EnemyStartedUltra, SoundPriority.VeryHigh);
                ShieldsDown = new PrioritisedSoundKey(SoundKeys.Events.ShieldsDown, SoundPriority.Normal);
            }
        }
    }
}