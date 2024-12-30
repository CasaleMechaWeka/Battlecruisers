using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound
{
    public class PvPSoundKeyPair
    {
        public ISoundKey PrimaryKey { get; }
        public ISoundKey SecondaryKey { get; }

        public PvPSoundKeyPair(ISoundKey primaryKey, ISoundKey secondaryKey)
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