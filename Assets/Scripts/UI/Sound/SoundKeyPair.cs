using BattleCruisers.Utils;

namespace BattleCruisers.UI.Sound
{
    public class SoundKeyPair
    {
        public ISoundKey PrimaryKey { get; }
        public ISoundKey SecondaryKey { get; }

        public SoundKeyPair(ISoundKey primaryKey, ISoundKey secondaryKey)
        {
            Helper.AssertIsNotNull(primaryKey, secondaryKey);

            PrimaryKey = primaryKey;
            SecondaryKey = secondaryKey;
        }
    }
}