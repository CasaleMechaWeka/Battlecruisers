using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public class AudioClipPlayer : IAudioClipPlayer
    {
        public void PlaySound(IAudioClipWrapper soundClip, Vector2 position)
        {
            AudioSource.PlayClipAtPoint(soundClip.AudioClip, position);
        }
    }
}
