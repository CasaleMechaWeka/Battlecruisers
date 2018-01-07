using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface ISoundManager
    {
        void PlaySound(string soundName, Vector2 position);
    }
}
