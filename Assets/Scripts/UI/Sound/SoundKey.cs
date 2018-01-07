namespace BattleCruisers.UI.Sound
{
    public class SoundKey : ISoundKey
    {
        public SoundType Type { get; private set; }
        public string Name { get; private set; }

        public SoundKey(SoundType type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
