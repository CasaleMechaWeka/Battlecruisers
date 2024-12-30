using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound
{
    public enum PvPSoundPriority
    {
        VeryHigh = 5,
        High = 4,
        Normal = 3,
        Low = 2,
        VeryLow = 1
    }

    public class PvPPrioritisedSoundKey
    {
        public ISoundKey Key { get; }
        public PvPSoundPriority Priority { get; }

        public PvPPrioritisedSoundKey(ISoundKey key, PvPSoundPriority priority)
        {
            Key = key;
            Priority = priority;
        }
    }
}