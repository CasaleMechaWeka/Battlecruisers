using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data.Static
{
    public static class PrioritisedSoundKeys
    {
        public static class Events
        {
            // Cruiser
            public static PrioritisedSoundKey CruiserUnderAttack { get; private set; }
            public static PrioritisedSoundKey CruiserSignificantlyDamaged { get; private set; }

            // Drones
            public static PrioritisedSoundKey DronesNewDronesReady { get; private set; }
            public static PrioritisedSoundKey DronesIdle { get; private set; }

            static Events()
            {
                // Cruiser
                CruiserUnderAttack = new PrioritisedSoundKey(SoundKeys.Events.CruiserUnderAttack, SoundPriority.Normal);
                CruiserSignificantlyDamaged = new PrioritisedSoundKey(SoundKeys.Events.CruiserSignificantlyDamaged, SoundPriority.VeryHigh);

                // Drones
                DronesNewDronesReady = new PrioritisedSoundKey(SoundKeys.Events.DronesNewDronesReady, SoundPriority.VeryLow);
                DronesIdle = new PrioritisedSoundKey(SoundKeys.Events.DronesIdle, SoundPriority.Low);
            }
        }
    }
}