namespace BattleCruisers.UI.Sound
{
    public enum SoundPriority
    {
        VeryHigh = 4,
        High = 3,
        Normal = 2,
        Low = 1
    }

    public class PrioritisedSoundKey
    {
        public ISoundKey Key { get; private set; }
        public SoundPriority Priority { get; private set; }

        public PrioritisedSoundKey(ISoundKey key, SoundPriority priority)
        {
            Key = key;
            Priority = priority;
        }
    }
}