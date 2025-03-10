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

        public override string ToString()
        {
            string primaryString = PrimaryKey.ToString();
            int dashIndex = primaryString.IndexOf('-');

            if (dashIndex == -1)
            {
                return primaryString;
            }
            else
            {
                return primaryString.Substring(0, dashIndex);
            }
        }
    }
}