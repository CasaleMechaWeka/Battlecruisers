using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data.Static
{
    public static class PrioritisedSoundKeys
    {
        public static class Completed
        {
            public static PrioritisedSoundKey Aircraft { get; private set; }
            public static PrioritisedSoundKey Building { get; private set; }
            public static PrioritisedSoundKey Ship { get; private set; }
            public static PrioritisedSoundKey Ultra { get; private set; }

            static Completed()
            {
                Aircraft = new PrioritisedSoundKey(SoundKeys.Completed.Aircraft, SoundPriority.VeryLow);
                Building = new PrioritisedSoundKey(SoundKeys.Completed.Building, SoundPriority.Low);
                Ship = new PrioritisedSoundKey(SoundKeys.Completed.Ship, SoundPriority.VeryLow);
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

            public static PrioritisedSoundKey EnemyStartedUltra { get; private set; }

            static Events()
            {
                // Cruiser
                CruiserUnderAttack = new PrioritisedSoundKey(SoundKeys.Events.CruiserUnderAttack, SoundPriority.Normal);
                CruiserSignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.Events.CruiserSignificantlyDamaged, SoundPriority.High);

                // Drones
                DronesNewDronesReady = new PrioritisedSoundKey(SoundKeys.Events.DronesNewDronesReady, SoundPriority.VeryLow);
                DronesIdle = new PrioritisedSoundKey(SoundKeys.Events.DronesIdle, SoundPriority.Low);

                EnemyStartedUltra = new PrioritisedSoundKey(SoundKeys.Events.EnemyStartedUltra, SoundPriority.VeryHigh);
            }
        }
    }
}