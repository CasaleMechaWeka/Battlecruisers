using BattleCruisers.Utils;

namespace BattleCruisers.UI.Sound
{
    // PERF  Struct candidate?
    public class SoundKey : ISoundKey
    {
        public SoundType Type { get; }
        public string Name { get; }

        public SoundKey(SoundType type, string name)
        {
            Type = type;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            SoundKey other = obj as SoundKey;
            return
                other != null
                && Type == other.Type
                && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Type, Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
