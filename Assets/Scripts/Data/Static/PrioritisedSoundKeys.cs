using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data.Static
{
    public static class PrioritisedSoundKeys
    {
        public static class Completed
        {
            public static class Buildings
            {
                public static PrioritisedSoundKey AirFactory { get; } = new PrioritisedSoundKey(SoundKeys.Completed.AirFactory, SoundPriority.VeryLow);
                public static PrioritisedSoundKey AntiAirTurret { get; } = new PrioritisedSoundKey(SoundKeys.Completed.AntiAirTurret, SoundPriority.VeryLow);
                public static PrioritisedSoundKey AntiShipTurret { get; } = new PrioritisedSoundKey(SoundKeys.Completed.AntiShipTurret, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Artillery { get; } = new PrioritisedSoundKey(SoundKeys.Completed.Artillery, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Booster { get; } = new PrioritisedSoundKey(SoundKeys.Completed.Booster, SoundPriority.VeryLow);
                public static PrioritisedSoundKey DroneStation { get; } = new PrioritisedSoundKey(SoundKeys.Completed.DroneStation, SoundPriority.VeryLow);
                public static PrioritisedSoundKey ControlTower { get; } = new PrioritisedSoundKey(SoundKeys.Completed.ControlTower, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Mortar { get; } = new PrioritisedSoundKey(SoundKeys.Completed.Mortar, SoundPriority.VeryLow);
                public static PrioritisedSoundKey NavalFactory { get; } = new PrioritisedSoundKey(SoundKeys.Completed.NavalFactory, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Railgun { get; } = new PrioritisedSoundKey(SoundKeys.Completed.Railgun, SoundPriority.VeryLow);
                public static PrioritisedSoundKey RocketLauncher { get; } = new PrioritisedSoundKey(SoundKeys.Completed.RocketLauncher, SoundPriority.VeryLow);
                public static PrioritisedSoundKey SamSite { get; } = new PrioritisedSoundKey(SoundKeys.Completed.SamSite, SoundPriority.VeryLow);
                public static PrioritisedSoundKey SpySatellite { get; } = new PrioritisedSoundKey(SoundKeys.Completed.SpySatellite, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Shields { get; } = new PrioritisedSoundKey(SoundKeys.Completed.Shields, SoundPriority.VeryLow);
                public static PrioritisedSoundKey StealthGenerator { get; } = new PrioritisedSoundKey(SoundKeys.Completed.StealthGenerator, SoundPriority.VeryLow);
                public static PrioritisedSoundKey TeslaCoil { get; } = new PrioritisedSoundKey(SoundKeys.Completed.TeslaCoil, SoundPriority.VeryLow);
            }

            public static PrioritisedSoundKey Ultra { get; } = new PrioritisedSoundKey(SoundKeys.Completed.Ultra, SoundPriority.VeryHigh);
        }

        public static class Events
        {
            public static class Cruiser
            {
                public static PrioritisedSoundKey UnderAttack { get; } = new PrioritisedSoundKey(SoundKeys.Events.CruiserUnderAttack, SoundPriority.Normal);
                public static PrioritisedSoundKey SignificantlyDamaged { get; } = new PrioritisedSoundKey(SoundKeys.Events.CruiserSignificantlyDamaged, SoundPriority.High);
            }

            public static class Drones
            {
                public static PrioritisedSoundKey NewDronesReady { get; } = new PrioritisedSoundKey(SoundKeys.Events.DronesNewDronesReady, SoundPriority.VeryLow);
                public static PrioritisedSoundKey Idle { get; } = new PrioritisedSoundKey(SoundKeys.Events.DronesIdle, SoundPriority.Low);
                public static PrioritisedSoundKey NotEnoughDronesToBuild { get; } = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToBuild, SoundPriority.VeryHigh);
                public static PrioritisedSoundKey NotEnoughDronesToFocus { get; } = new PrioritisedSoundKey(SoundKeys.Events.DronesNotEnoughDronesToFocus, SoundPriority.High);
                public static PrioritisedSoundKey Focusing { get; } = new PrioritisedSoundKey(SoundKeys.Events.DronesFocusing, SoundPriority.High);
                public static PrioritisedSoundKey AllFocused { get; } = new PrioritisedSoundKey(SoundKeys.Events.DronesAllFocused, SoundPriority.High);
                public static PrioritisedSoundKey Dispersing { get; } = new PrioritisedSoundKey(SoundKeys.Events.DronesDispersing, SoundPriority.High);
            }

            public static class Targetting
            {
                public static PrioritisedSoundKey NewTarget { get; } = new PrioritisedSoundKey(SoundKeys.Events.TargettingNewTarget, SoundPriority.Normal);
                public static PrioritisedSoundKey TargetCleared { get; } = new PrioritisedSoundKey(SoundKeys.Events.TargettingDeselected, SoundPriority.Normal);
            }

            public static PrioritisedSoundKey EnemyStartedUltra { get; } = new PrioritisedSoundKey(SoundKeys.Events.EnemyStartedUltra, SoundPriority.VeryHigh);
            public static PrioritisedSoundKey IncompleteFactory { get; } = new PrioritisedSoundKey(SoundKeys.Events.FactoryIncomplete, SoundPriority.VeryHigh);
            public static PrioritisedSoundKey PopulationLimitReached { get; } = new PrioritisedSoundKey(SoundKeys.Events.PopulationLimitReached, SoundPriority.VeryHigh);
        }
    }
}