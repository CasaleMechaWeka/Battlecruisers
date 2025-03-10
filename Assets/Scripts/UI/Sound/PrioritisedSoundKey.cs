namespace BattleCruisers.UI.Sound
{
    public enum SoundPriority
    {
        VeryHigh = 5,
        High = 4,
        Normal = 3,
        Low = 2,
        VeryLow = 1
    }

    public class PrioritisedSoundKey
    {
        public ISoundKey Key { get; }
        public SoundPriority Priority { get; }

        public PrioritisedSoundKey(ISoundKey key, SoundPriority priority)
        {
            Key = key;
            Priority = priority;
        }
    }
}