using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface ISoundPlayer
    {
        void PlaySound(ISoundKey soundKey);
        void PlaySound(ISoundKey soundKey, Vector2 position);
    }
}
