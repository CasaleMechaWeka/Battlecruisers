using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface ISoundManager
    {
        void PlaySound(ISoundKey soundKey, Vector2 position);
    }
}
