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

            // FELIX  Remove once have StealthField specific sound :)
            public static PrioritisedSoundKey Building { get; private set; }

            public static PrioritisedSoundKey Ultra { get; private set; }

            static Completed()
            {
                Building = new PrioritisedSoundKey(SoundKeys.Completed.Building, SoundPriority.Low);
                Ultra = new PrioritisedSoundKey(SoundKeys.Completed.Ultra, SoundPriority.VeryHigh);
            }
        }

        public static class Events
        {
            // Cruiser
            public static PrioritisedSoundKey CruiserUnderAttack { get; private set; }
            public static PrioritisedSoundKey CruiserSignificantlyDamaged { get; private set; }

            // Drones
            public static PrioritisedSoundKey DronesNewDronesReady { get; private set; }
            public static PrioritisedSoundKey DronesIdle { get; private set; }
            public static PrioritisedSoundKey DronesNotEnoughDrones { get; private set; }

            public static PrioritisedSoundKey EnemyStartedUltra { get; private set; }

            static Events()
            {
                // Cruiser
                CruiserUnderAttack = new PrioritisedSoundKey(SoundKeys.Events.CruiserUnderAttack, SoundPriority.Normal);
                CruiserSignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.Events.CruiserSignificantlyDamaged, SoundPriority.High);

                // Drones
                DronesNewDronesReady = new PrioritisedSoundKey(SoundKeys.Events.DronesNewDronesReady, SoundPriority.VeryLow);
                DronesIdle = new PrioritisedSoundKey(SoundKeys.Events.DronesIdle, SoundPriority.Low);
                DronesNotEnoughDrones = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDrones, SoundPriority.VeryHigh);

                EnemyStartedUltra = new PrioritisedSoundKey(SoundKeys.Events.EnemyStartedUltra, SoundPriority.VeryHigh);
            }
        }
    }
}