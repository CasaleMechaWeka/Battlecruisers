using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface IAudioClipPlayer
    {
        void PlaySound(IAudioClipWrapper soundClip, Vector3 position);
    }
}
