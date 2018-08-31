using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface ISoundPlayer
    {
        // FELIX  Only used by SequentialSoundPlayer?  Create 2 interfaces?
        void PlaySound(ISoundKey soundKey);
        // FELIX  Only used by SoundPlayer?  Create 2 interfaces?
        void PlaySound(ISoundKey soundKey, Vector2 position);
    }
}
