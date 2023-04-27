using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound
{
    // PERF  Struct candidate?
    public class PvPSoundKey : IPvPSoundKey
    {
        public PvPSoundType Type { get; }
        public string Name { get; }

        public PvPSoundKey(PvPSoundType type, string name)
        {
            Type = type;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            PvPSoundKey other = obj as PvPSoundKey;
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
