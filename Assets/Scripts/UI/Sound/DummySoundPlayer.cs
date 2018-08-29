using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public class DummySoundPlayer : ISoundPlayer
    {
        public void PlaySound(ISoundKey soundKey)
        {
            // empty
        }

        public void PlaySound(ISoundKey soundKey, Vector2 position)
        {
            // empty
        }
    }
}
